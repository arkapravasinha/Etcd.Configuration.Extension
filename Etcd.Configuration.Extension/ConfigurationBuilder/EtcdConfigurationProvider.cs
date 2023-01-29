using Etcd.Configuration.Extension.Utils;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Etcd.Configuration.Extension.Exceptions;
using Etcd.Configuration.Extension.ConfigurationSource;
using Etcd.Configuration.Extension.Models;
using System.IO;
using dotnet_etcd.interfaces;

namespace Etcd.Configuration.Extension.ConfigurationBuilder
{
    /// <summary>
    /// This is used to Create and Load the Configuration
    /// </summary>
    internal sealed class EtcdConfigurationProvider : ConfigurationProvider, IDisposable
    {
        //Private Variables
        private readonly EtcdConfigurationSource _etcdConfigurationSource;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IEtcdClient _etcdClient;
        private Task? _watcher;

        //Constructor
        public EtcdConfigurationProvider( EtcdConfigurationSource etcdConfigurationSource, IEtcdClient etcdClient)
        {
            _etcdConfigurationSource = etcdConfigurationSource;
            _cancellationTokenSource = new CancellationTokenSource();
            _etcdClient = etcdClient;
        }


        public override void Load()
        {
            if (_watcher != null)
                return;
            var cancellationToken = _cancellationTokenSource.Token;

            DoLoad(false).GetAwaiter().GetResult();

            // Polling starts after the initial load to ensure no concurrent access to the key from this instance
            if (_etcdConfigurationSource.ReloadOnChange)
            {
                _watcher = Task.Run(() => Watcher(cancellationToken), cancellationToken);
            }
        }

        /// <summary>
        /// Watcher to reload the configuration if Changes ocurred
        /// </summary>
        /// <param name="cancellationToken"></param>
        private void Watcher(CancellationToken cancellationToken)
        {
            if (_etcdClient == null) return;
            try
            {
                var keys = _etcdConfigurationSource.Keys.GenerateKeys()?.Select(x => x.KeyName)?.ToArray();
                _etcdClient.Watch(keys, (x) =>
                {
                    var data = x?.Where(y => (y?.Type == Mvccpb.Event.Types.EventType.Put || y?.Type == Mvccpb.Event.Types.EventType.Delete));
                    if (data != null && data.Any())
                    {
                        DoLoad(true).GetAwaiter().GetResult();
                    }
                }, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                _etcdConfigurationSource.OnWatchFailure?.Invoke(new EtcdConfigWatchException("Watch Failed", ex));
            }
            
        }

        /// <summary>
        /// Main Load Method, Responsible for Load or Reloading configuration
        /// </summary>
        /// <param name="isReload">Whether it is a Reload or First Load</param>
        /// <returns>Task</returns>
        private async Task DoLoad(bool isReload)
        {
            if (_etcdClient == null) return;
            try
            {
                Dictionary<string,string?> data = new Dictionary<string,string?>();
                var keys = _etcdConfigurationSource.Keys.GenerateKeys();
                foreach (var key in keys)
                {
                    var value = await _etcdClient.GetValAsync(key.KeyName);
                    switch (key.ValueType)
                    {
                        case ValueTypes.JSON:
                            LoadJsonConfiguration(value,ref data, key);
                            break;
                        case ValueTypes.STRING:
                            LoadStringConfiguration(ref data, key, value);
                            break;
                        default:
                            break;
                    }

                }
                Data= data;
                if (isReload)
                    OnReload();
            }
            catch (Exception ex)
            {
                if (isReload)
                    _etcdConfigurationSource.OnLoadFailure?.Invoke(new EtcdConfigOnLoadException($"Load Failed during Reload", ex));
                else
                    _etcdConfigurationSource.OnLoadFailure?.Invoke(new EtcdConfigOnLoadException($"Load Failed", ex));
                
            }

        }


        /// <summary>
        /// Converting String Data to Key Value
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void LoadStringConfiguration(ref Dictionary<string, string?> data, Key key, string value)
        {
            if (!_etcdConfigurationSource.UseFullPathForKeys)
            {
                var modifiedKey = key.KeyName.Split("/")?.Last();
                if (!string.IsNullOrEmpty(modifiedKey))
                    data.Add(modifiedKey, value);
            }
            else
                data.Add(key.KeyName, value);
        }


        /// <summary>
        /// Disposing Clients and Cancellation Token
        /// </summary>
        public void Dispose()
        {
            _etcdClient?.Dispose();
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        /// <summary>
        /// Convert JSON Data to Key Value
        /// </summary>
        /// <param name="jsondata"></param>
        /// <param name="data"></param>
        /// <param name="key"></param>
        private void LoadJsonConfiguration(string jsondata, ref Dictionary<string,string?> data, Key key)
        {
            if (string.IsNullOrEmpty(jsondata) || data == null)
                return;
            using Stream memoryStream=new MemoryStream(Encoding.UTF8.GetBytes(jsondata));
            var kvpairs = _etcdConfigurationSource.JsonParser.Parse(memoryStream);
            foreach(var kv in kvpairs)
            {
                string KeyPath = kv.Key;
                if (_etcdConfigurationSource.UseFullPathForKeys)
                    KeyPath = string.Concat(key.KeyName, ":", KeyPath);

                if (data.ContainsKey(KeyPath))
                    data[KeyPath] = kv.Value;
                else
                    data.Add(KeyPath, kv.Value);
            }
        }
    }
}
