using Etcd.Configuration.Extension.ConfigurationSource;
using Microsoft.Extensions.Configuration;
using System;

namespace Etcd.Configuration.Extension.Extensions
{
    public static class EtcdConfigurationBuilderExtensions
    {
        /// <summary>
        /// Add EtcdConfiguration Builder to IConfigurationBuilder
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="etcdConfigurationOptions">Options to Configure ETCD Configuration Loader</param>
        /// <returns></returns>
        public static IConfigurationBuilder AddEtcdConfiguration(this IConfigurationBuilder builder, Action<IEtcdConfigurationOptions> etcdConfigurationOptions)
        {
            var etcdConfigSource = new EtcdConfigurationSource();
            etcdConfigurationOptions(etcdConfigSource);
            return builder.Add(etcdConfigSource);
        }
    }
}
