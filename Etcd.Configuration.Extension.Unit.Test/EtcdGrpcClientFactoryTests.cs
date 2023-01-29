using Etcd.Configuration.Extension.Client;
using Etcd.Configuration.Extension.ConfigurationSource;
using Etcd.Configuration.Extension.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etcd.Configuration.Extension.Unit.Test
{
    [TestClass]
    public class EtcdGrpcClientFactoryTests
    {
        [TestMethod]
        public void GetClientTest_Null()
        {
            //Arrange
            var mock = new Mock<IEtcdConfigurationOptions>();
            //Act

            EtcdGrpcClientFactory etcdGrpcClientFactory = new EtcdGrpcClientFactory(mock.Object);
            var result = etcdGrpcClientFactory.GetClient();

            //Assert
            Assert.IsNull(result);

        }

        [TestMethod]
        public void GetClientTest_NotNull()
        {
            //Arrange
            IEtcdConfigurationOptions etcdConfigurationSource = new EtcdConfigurationSource();
            //Act

            EtcdGrpcClientFactory etcdGrpcClientFactory = new EtcdGrpcClientFactory(etcdConfigurationSource);
            var result = etcdGrpcClientFactory.GetClient();

            //Assert
            Assert.IsNull(result);

        }


        [TestMethod]
        [ExpectedException(typeof(EtcdGrpcClientException))]
        public void GetClientTest_Exception()
        {
            //Arrange
            IEtcdConfigurationOptions etcdConfigurationSource = new EtcdConfigurationSource();
            etcdConfigurationSource.OnClientCreationFailure = (x) =>
            {
                throw x;
            };
            //Act

            EtcdGrpcClientFactory etcdGrpcClientFactory = new EtcdGrpcClientFactory(etcdConfigurationSource);
            var result = etcdGrpcClientFactory.GetClient();

            //Assert
            Assert.IsNull(result);

        }

        [TestMethod]
        [ExpectedException(typeof(EtcdGrpcClientException))]
        public void GetClientTest_UserName_Exception()
        {
            //Arrange
            IEtcdConfigurationOptions etcdConfigurationSource = new EtcdConfigurationSource();
            etcdConfigurationSource.OnClientCreationFailure = (x) =>
            {
                throw x;
            };
            etcdConfigurationSource.UserName = "Test";
            etcdConfigurationSource.Password = "test";
            //Act

            EtcdGrpcClientFactory etcdGrpcClientFactory = new EtcdGrpcClientFactory(etcdConfigurationSource);
            var result = etcdGrpcClientFactory.GetClient();

            //Assert
            Assert.IsNull(result);

        }
    }
}
