using Etcd.Configuration.Extension.ConfigurationSource;
using Etcd.Configuration.Extension.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etcd.Configuration.Extension.Unit.Test
{
    [TestClass]
    public class ExtensionsTests
    {
        [TestMethod]
        public void EtcdConfigurationBuilderExtensions_Test()
        {
            //Arrange
            Mock<IConfigurationBuilder> builder = new Mock<IConfigurationBuilder>();
            EtcdConfigurationSource source= new EtcdConfigurationSource();

            //Act
            builder.Object.AddEtcdConfiguration(x =>
            {
                x=source;
            });
        }

        [TestMethod]
        public void EtcdConfigurationManagementExtension_Test()
        {
            //Arrange
            ConfigurationManager configurationManager=new ConfigurationManager();
            EtcdConfigurationSource source = new EtcdConfigurationSource();

            //Act
            configurationManager.AddEtcdConfiguration(x =>
            {
                x = source;
            });

            //Assert
            Assert.IsTrue(configurationManager.Sources.Where(x => x.GetType().Equals(typeof(EtcdConfigurationSource))).Any());
        }

        [TestMethod]
        public void EtcdHostBuilderExtension_Test()
        {
            //Arrange
            Mock<IHostBuilder> mock  = new Mock<IHostBuilder>();
            EtcdConfigurationSource source = new EtcdConfigurationSource();

            //Act
            mock.Object.AddEtcdConfiguration(x =>
            {
                x = source;
            });
        }
    }
}
