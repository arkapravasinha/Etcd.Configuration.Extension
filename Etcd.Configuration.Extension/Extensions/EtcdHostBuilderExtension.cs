using Etcd.Configuration.Extension.ConfigurationSource;
using Microsoft.Extensions.Hosting;
using System;

namespace Etcd.Configuration.Extension.Extensions
{
    public static class EtcdHostBuilderExtension
    {
        /// <summary>
        /// Add EtcdConfiguration Builder to Host Builder
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="options">Options to Configure ETCD Configuration Loader</param>
        /// <returns></returns>
        public static IHostBuilder AddEtcdConfiguration(this IHostBuilder hostBuilder, Action<IEtcdConfigurationOptions> options)
        {
            hostBuilder.ConfigureAppConfiguration((builderContext, configurationBuilder) =>
            {
                configurationBuilder.AddEtcdConfiguration(options);
            });
            return hostBuilder;
        }
    }
}
