using System;
using System.Collections.Generic;
using System.Linq;

namespace Etcd.Configuration.Extension.Models
{
    internal class Keys
    {
        public List<Key> AllKeys { get; set; } = new List<Key>();
    }

    internal class Key
    {
        public string KeyName { get; set; } = string.Empty;
        public ValueTypes ValueType { get; set; } = ValueTypes.STRING;
    }


}
