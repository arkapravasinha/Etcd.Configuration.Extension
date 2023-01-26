using System;
using System.Collections.Generic;
using System.Linq;
using Etcd.Configuration.Extension.Models;

namespace Etcd.Configuration.Extension.Utils
{
    internal static class Utils
    {
        /// <summary>
        /// Convert the Keys to map with supported Parser
        /// </summary>
        /// <param name="keys"> Keys in below format {key:string},{key:json} ie. "key1:string,key2:json"</param>
        /// <returns>List of Key <see cref="Key"/></returns>
        /// <exception cref="ArgumentException"></exception>
        public static List<Key> GenerateKeys(this string keys)
        {
            try
            {
                List<Key> listKey = new List<Key>();
                if (!string.IsNullOrEmpty(keys) && keys.Contains(":"))
                {
                    string[] comaSeparatedKeys = keys.Split(",");
                    foreach (var singleKey in comaSeparatedKeys)
                    {
                        string[] keyandType = singleKey.Trim().Split(":");
                        if (keyandType.Length > 1 && Enum.TryParse<ValueTypes>(keyandType[1], true, out ValueTypes valueType))
                        {
                            listKey.Add(new Key() { KeyName = keyandType[0], ValueType = valueType });
                        }
                    }
                }
                return listKey;
            }
            catch
            {
                throw new ArgumentException("Keys are not proper should be like 'key1:string,key2:json' ");
            }
        }
    }
}
