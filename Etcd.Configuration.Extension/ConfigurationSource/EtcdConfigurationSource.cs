using dotnet_etcd;
using Etcd.Configuration.Extension.Client;
using Etcd.Configuration.Extension.ConfigurationBuilder;
using Etcd.Configuration.Extension.Exceptions;
using Etcd.Configuration.Extension.Parser;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Etcd.Configuration.Extension.ConfigurationSource
{
    internal sealed class EtcdConfigurationSource : IConfigurationSource, IEtcdConfigurationOptions
    {
        public JsonParser JsonParser { get; set; } = new JsonParser();
        public string Hosts { get; set; } = string.Empty;
        public int Port { get; set; } = 2379;
        public string ServerName { get; set; } = "my-etcd-server";

        public int MaxAttempts { get; set; } = 5;
        public int InitialBackoffSeconds { get; set; } = 1;
        public int MaxBackoffSeconds { get; set; } = 5;
        public double BackoffMultiplier { get; set; } = 1.5;

        public int MaxTokens { get; set; } = 10;
        public double TokenRatio { get; set; } = 0.1;

        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public HttpClientHandler? HttpClientHandlerForEtcd { get; set; } = null;

        public bool Ssl { get; set; } = false;

        public string Keys { get; set; } = string.Empty;

        public bool ReloadOnChenge { get; set; } = false;

        public Action<EtcdConfigOnLoadException>? OnLoadFailure { get; set; } = null;

        public Action<EtcdConfigWatchException>? OnWatchFailure { get; set; } = null;
        public Action<EtcdGrpcClientException>? OnClientCreationFailure { get; set; } = null;
        public bool UseFullPathForKeys { get; set; } = false;

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {

            var etcdClient = new EtcdGrpcClientFactory(this).GetClient();
            return new EtcdConfigurationProvider(this, etcdClient);
        }

    }
}
