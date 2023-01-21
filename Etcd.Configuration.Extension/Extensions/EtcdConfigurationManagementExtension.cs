using Etcd.Configuration.Extension.ConfigurationSource;
using Microsoft.Extensions.Configuration;
using System;

namespace Etcd.Configuration.Extension.Extensions
{
    public static class EtcdConfigurationManagementExtension
    {
        /// <summary>
        /// Add EtcdConfiguration Builder to ConfigurationManager
        /// </summary>
        /// <param name="configurationManager"></param>
        /// <param name="options">Options to Configure ETCD Configuration Loader</param>
        public static void AddEtcdConfiguration(this ConfigurationManager configurationManager,
                                                                Action<IEtcdConfigurationOptions> options)
        {
            var etcdConfigSource = new EtcdConfigurationSource();
            options(etcdConfigSource);
            configurationManager.Sources.Add(etcdConfigSource);
        }
    }
}
