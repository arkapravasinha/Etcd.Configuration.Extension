using Microsoft.Extensions.Configuration.Json;
using System.Collections.Generic;
using System.IO;

namespace Etcd.Configuration.Extension.Parser
{
    /// <summary>
    /// JsonParser Used to Parse Json Configuration on to IConfiguration
    /// </summary>
    internal sealed class JsonParser
    {
        public IDictionary<string, string?> Parse(Stream stream)
        {
            return JsonStreamParser.Parse(stream);
        }

        /// <summary>
        /// Json Stream Configuration Provider
        /// </summary>
        private sealed class JsonStreamParser : JsonStreamConfigurationProvider
        {
            private JsonStreamParser(JsonStreamConfigurationSource source)
                : base(source)
            {
            }

            internal static IDictionary<string, string?> Parse(Stream stream)
            {
                var provider = new JsonStreamParser(new JsonStreamConfigurationSource { Stream = stream });
                provider.Load();
                return provider.Data;
            }
        }
    }
}
