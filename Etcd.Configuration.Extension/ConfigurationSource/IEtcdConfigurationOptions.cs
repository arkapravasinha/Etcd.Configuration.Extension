using Etcd.Configuration.Extension.Exceptions;
using System;
using System.Net.Http;

namespace Etcd.Configuration.Extension.ConfigurationSource
{
    /// <summary>
    /// Options to Configure ETCD
    /// </summary>
    public interface IEtcdConfigurationOptions
    {
        /// <summary>
        /// ETCD Grpc Client BackOffMultipler, Defaults to 1.5
        /// </summary>
        double BackoffMultiplier { get; set; }

        /// <summary>
        /// ETCD Grpc Client Host URL, Defaults to Empty String, Pass the hosts in commaseparated manner
        /// </summary>
        string Hosts { get; set; }

        /// <summary>
        /// ETCD Grpc Client Handler, useful to write custom handlers, defaults to null
        /// </summary>
        HttpClientHandler? HttpClientHandlerForEtcd { get; set; }

        /// <summary>
        /// ETCD Grpc Client InitialBackoffSeconds, Defaults to 1
        /// </summary>
        int InitialBackoffSeconds { get; set; }

        /// <summary>
        /// Keys should in below format {key:string},{key:json} ie. "key1:string,key2:json"
        /// This is used to fetch the configurations, in ETCD , it should be the path to your values
        /// </summary>
        string Keys { get; set; }

        /// <summary>
        /// ETCD Grpc Client Max Attempt, defaults to 5
        /// </summary>
        int MaxAttempts { get; set; }

        /// <summary>
        /// ETCD Grpc Client MaxBackoffSeconds, defaults to 5
        /// </summary>
        int MaxBackoffSeconds { get; set; }

        /// <summary>
        /// ETCD Grpc Client MaxTokens, defaults to 10
        /// </summary>
        int MaxTokens { get; set; }

        /// <summary>
        /// This is used to configure the behavior during an exception while loading Configuration from ETCD 
        /// </summary>
        Action<EtcdConfigOnLoadException>? OnLoadFailure { get; set; }

        /// <summary>
        /// This is used to configure the behavior during an exception while watching Configuration from ETCD 
        /// </summary>
        Action<EtcdConfigWatchException>? OnWatchFailure { get; set; }

        /// <summary>
        /// This is used to configure the behavior during an exception while creating ETCD client 
        /// </summary>
        Action<EtcdGrpcClientException>? OnClientCreationFailure { get; set; }

        /// <summary>
        /// Password for ETCD Client, Only configure it if ETCD Authentication is enabled
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// Port for ETCD Client
        /// </summary>
        int Port { get; set; }

        /// <summary>
        /// If Reload is Required, this will change the configuration
        /// </summary>
        bool ReloadOnChange { get; set; }

        /// <summary>
        /// ETCD Client Server Name
        /// </summary>
        string ServerName { get; set; }

        /// <summary>
        /// Whether SSL is Required, defaults to false
        /// </summary>
        bool Ssl { get; set; }

        /// <summary>
        /// ETCD Client Token Ratio, defaults to 0.1
        /// </summary>
        double TokenRatio { get; set; }

        /// <summary>
        ///  Password for ETCD Client, Only configure it if ETCD Authentication is enabled
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// Whether to Have Full Key Path in Loaded Configuration
        /// </summary>
        bool UseFullPathForKeys { get; set; }

    }
}