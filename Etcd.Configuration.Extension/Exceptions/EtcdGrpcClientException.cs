using System;

namespace Etcd.Configuration.Extension.Exceptions
{
    /// <summary>
    /// Exception thrown if Client is not able to Connnect to ETCD Instance via grpc
    /// </summary>
    public class EtcdGrpcClientException : Exception
    {
        public EtcdGrpcClientException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
