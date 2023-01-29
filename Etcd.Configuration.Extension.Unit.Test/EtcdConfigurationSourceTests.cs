using Etcd.Configuration.Extension.ConfigurationSource;
using Etcd.Configuration.Extension.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etcd.Configuration.Extension.Unit.Test
{
    [TestClass]
    public class EtcdConfigurationSourceTests
    {
        [TestMethod]
        public void Configuration_Source_Test()
        {
            //Arranage
            EtcdConfigurationSource source = new EtcdConfigurationSource();
            source.BackoffMultiplier = 1;
            source.JsonParser = new Parser.JsonParser();
            source.Port= 1;
            source.ServerName = string.Empty;
            source.MaxAttempts = 1;
            source.InitialBackoffSeconds = 1;
            source.MaxBackoffSeconds = 1;
            source.MaxTokens = 1;
            source.TokenRatio= 1;
            source.HttpClientHandlerForEtcd=new HttpClientHandler();
            source.Ssl = true;
            source.OnWatchFailure = (x) =>
            {
                throw x;
            };
            

            //Assert
            Assert.IsNotNull(source.JsonParser);
            Assert.AreEqual(source.Port, 1);
            Assert.AreEqual(source.BackoffMultiplier, 1);
            Assert.AreEqual(source.Ssl, true);
            Assert.IsNotNull(source.HttpClientHandlerForEtcd);
            Assert.ThrowsException<EtcdConfigWatchException>(()=> { source.OnWatchFailure.Invoke(new EtcdConfigWatchException("test", new Exception())); }); 

        }
    }
}
