using dotnet_etcd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etcd.Configuration.Extension.Client
{
    /// <summary>
    /// This is used to create an Etcd Grpc Client
    /// </summary>
    internal interface IEtcdGrpcClientFactory
    {

        /// <summary>
        /// Create and Return an Etcd Client
        /// </summary>
        /// <returns>An EtcdClient Instance of type <see cref="EtcdClient"/></returns>
        EtcdClient GetClient();
    }
}
