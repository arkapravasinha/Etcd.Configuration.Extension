using System;

namespace Etcd.Configuration.Extension.Exceptions
{
    /// <summary>
    /// Exception thrown while loading from ETCD is failed 
    /// </summary>
    public class EtcdConfigOnLoadException:Exception
    {
        public EtcdConfigOnLoadException(string message) : base(message)
        {

        }

        public EtcdConfigOnLoadException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
