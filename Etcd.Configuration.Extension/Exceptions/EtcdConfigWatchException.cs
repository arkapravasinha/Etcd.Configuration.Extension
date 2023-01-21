using System;

namespace Etcd.Configuration.Extension.Exceptions
{
    /// <summary>
    /// Exception thrown while watching the keys for any change
    /// </summary>
    public class EtcdConfigWatchException:Exception
    {
        public EtcdConfigWatchException(string message) : base(message)
        {

        }

        public EtcdConfigWatchException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
