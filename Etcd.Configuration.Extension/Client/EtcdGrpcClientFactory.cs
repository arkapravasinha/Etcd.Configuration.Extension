using dotnet_etcd;
using Etcd.Configuration.Extension.ConfigurationSource;
using Etcd.Configuration.Extension.Exceptions;
using Grpc.Core;
using Grpc.Net.Client.Configuration;
using System;

namespace Etcd.Configuration.Extension.Client
{
    /// <summary>
    /// This is used to create an Etcd Grpc Client
    /// </summary>
    internal sealed class EtcdGrpcClientFactory: IEtcdGrpcClientFactory
    {
        //Private Variables
        private readonly EtcdConfigurationSource _etcdConfigurationSource;

        //Constructor
        public EtcdGrpcClientFactory(EtcdConfigurationSource etcdConfigurationSource)
        {
            _etcdConfigurationSource = etcdConfigurationSource;
        }

        public EtcdClient GetClient()
        {
            try 
            {
                var client = new EtcdClient(_etcdConfigurationSource.Hosts, _etcdConfigurationSource.Port, _etcdConfigurationSource.ServerName,
                                             _etcdConfigurationSource.HttpClientHandlerForEtcd, _etcdConfigurationSource.Ssl, false, null,
                                             new MethodConfig()
                                             {
                                                 Names = { MethodName.Default },
                                                 RetryPolicy = new RetryPolicy
                                                 {
                                                     MaxAttempts = _etcdConfigurationSource.MaxAttempts,
                                                     InitialBackoff = TimeSpan.FromSeconds(_etcdConfigurationSource.InitialBackoffSeconds),
                                                     MaxBackoff = TimeSpan.FromSeconds(_etcdConfigurationSource.MaxBackoffSeconds),
                                                     BackoffMultiplier = _etcdConfigurationSource.BackoffMultiplier,
                                                     RetryableStatusCodes = { StatusCode.Unavailable }
                                                 }
                                             },
                                             new RetryThrottlingPolicy()
                                             {
                                                 MaxTokens = _etcdConfigurationSource.MaxTokens,
                                                 TokenRatio = _etcdConfigurationSource.TokenRatio
                                             });
                if(!string.IsNullOrEmpty(_etcdConfigurationSource.UserName) && !string.IsNullOrEmpty(_etcdConfigurationSource.Password))
                {
                    client.Authenticate(new Etcdserverpb.AuthenticateRequest()
                    {
                        Name = _etcdConfigurationSource.UserName,
                        Password = _etcdConfigurationSource.Password
                    });
                }
                return client;
            }
            catch(Exception ex)
            {
                var exception = new EtcdGrpcClientException("Unable to Create ETCD Grpc Client, check Inner exception to get more details", ex);
                _etcdConfigurationSource.OnClientCreationFailure?.Invoke(exception);
#pragma warning disable CS8603 // Possible null reference return.
                return null;
#pragma warning restore CS8603 // Possible null reference return.
            }
        }
    }
}
