using dotnet_etcd;
using dotnet_etcd.interfaces;
using Etcd.Configuration.Extension.ConfigurationBuilder;
using Etcd.Configuration.Extension.ConfigurationSource;
using Etcd.Configuration.Extension.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Etcd.Configuration.Extension.Unit.Test
{
    [TestClass]
    public class EtcdConfigurationProviderTests
    {
        private Mock<IEtcdClient> _etcdClientMock;
        private EtcdConfigurationSource _configurationSource;

        [TestInitialize] 
        public void TestInitialize() 
        { 
            _etcdClientMock= new Mock<IEtcdClient>();
            _configurationSource= new EtcdConfigurationSource();
            
        }

        [TestCleanup]
        public void TestCleanup() 
        {
            _etcdClientMock = null;
            _configurationSource = null;
        }

        [DataTestMethod]
        [DataRow("test/testkey", "test/testkey:string","value")]
        [DataRow("test/testkey1", "test/testkey1:string", "value1")]
        public void ConfigurationProvider_Test_String(string genKey,string etcdKey,string value)
        {
            //Arrange
            _etcdClientMock.Setup(x => x.GetValAsync(genKey, null, null, default)).Returns(Task.FromResult<string>(value));
            _configurationSource.UseFullPathForKeys = true;
            _configurationSource.ReloadOnChange = false;
            _configurationSource.Keys = etcdKey;
            using EtcdConfigurationProvider etcdConfigurationProvider = new EtcdConfigurationProvider(_configurationSource, _etcdClientMock.Object);

            //Act
            etcdConfigurationProvider.Load();

            //Assert
            Assert.IsTrue(etcdConfigurationProvider.TryGet(genKey, out string? result));
            Assert.IsTrue(result?.Equals( value));

        }

        [DataTestMethod]
        [DataRow("test/testkey", "test/testkey:string", "value","testkey")]
        [DataRow("test/testkey1", "test/testkey1:string", "value1", "testkey1")]
        public void ConfigurationProvider_Test_String_Non_Full_Path(string genKey, string etcdKey, string value,string nonKey)
        {
            //Arrange
            _etcdClientMock.Setup(x => x.GetValAsync(genKey, null, null, default)).Returns(Task.FromResult<string>(value));
            _configurationSource.UseFullPathForKeys = false;
            _configurationSource.ReloadOnChange = false;
            _configurationSource.Keys = etcdKey;
            using EtcdConfigurationProvider etcdConfigurationProvider = new EtcdConfigurationProvider(_configurationSource, _etcdClientMock.Object);

            //Act
            etcdConfigurationProvider.Load();

            //Assert
            Assert.IsTrue(etcdConfigurationProvider.TryGet(nonKey, out string? result));
            Assert.IsTrue(result?.Equals(value));

        }

        [TestMethod]
        public void ConfigurationProvider_Test_Json()
        {
            //Arrange
            string path = "/Data/JsonParser.json";
            _etcdClientMock.Setup(x => x.GetValAsync("test/testkey", null, null, default)).Returns(Task.FromResult<string>(File.ReadAllText(Environment.CurrentDirectory + path)));
            _configurationSource.UseFullPathForKeys = true;
            _configurationSource.ReloadOnChange = false;
            _configurationSource.Keys = "test/testkey:json";
            using EtcdConfigurationProvider etcdConfigurationProvider = new EtcdConfigurationProvider(_configurationSource, _etcdClientMock.Object);

            //Act
            etcdConfigurationProvider.Load();

            //Assert
            Assert.IsTrue(etcdConfigurationProvider.TryGet("test/testkey:key1", out string? result));
            Assert.IsTrue(result?.Equals("value_key1"));

        }

        [TestMethod]
        public void ConfigurationProvider_Test_Json_Not_Full_Path()
        {
            //Arrange
            string path = "/Data/JsonParser.json";
            _etcdClientMock.Setup(x => x.GetValAsync("test/testkey", null, null, default)).Returns(Task.FromResult<string>(File.ReadAllText(Environment.CurrentDirectory + path)));
            _configurationSource.UseFullPathForKeys = false;
            _configurationSource.ReloadOnChange = false;
            _configurationSource.Keys = "test/testkey:json";
            using EtcdConfigurationProvider etcdConfigurationProvider = new EtcdConfigurationProvider(_configurationSource, _etcdClientMock.Object);

            //Act
            etcdConfigurationProvider.Load();

            //Assert
            Assert.IsTrue(etcdConfigurationProvider.TryGet("key1", out string? result));
            Assert.IsTrue(result?.Equals("value_key1"));

        }

        [DataTestMethod]
        [DataRow("test/testkey", "test/testkey:string", "value","changedvalue")]
        [DataRow("test/testkey1", "test/testkey1:string", "value1","changedvalue1")]
        public void ConfigurationProvider_Test_String_Watcher(string genKey, string etcdKey, string value, string changedValue)
        {
            //Arrange
            _etcdClientMock.Setup(x => x.GetValAsync(genKey, null, null, default)).Returns(Task.FromResult<string>(value));
            _etcdClientMock.Setup(x => x.Watch(genKey, It.IsAny<Action<WatchEvent[]>>(), null, null, It.IsAny<CancellationToken>()));
            _configurationSource.UseFullPathForKeys = true;
            _configurationSource.ReloadOnChange = true;
            _configurationSource.Keys = etcdKey;
            using EtcdConfigurationProvider etcdConfigurationProvider = new EtcdConfigurationProvider(_configurationSource, _etcdClientMock.Object);

            //Act
            etcdConfigurationProvider.Load();

            //Assert
            Assert.IsTrue(etcdConfigurationProvider.TryGet(genKey, out string? result));
            Assert.IsTrue(result?.Equals(value));

        }

        //[DataTestMethod]
        //[DataRow("test/testkey", "test/testkey:string", "value", "changedvalue")]
        //[DataRow("test/testkey1", "test/testkey1:string", "value1", "changedvalue1")]
        //[ExpectedException(typeof(EtcdConfigWatchException))]
        //public void ConfigurationProvider_Test_String_Watcher_exception(string genKey, string etcdKey, string value, string changedValue)
        //{
        //    //Arrange
        //    _etcdClientMock.Setup(x => x.GetValAsync(genKey, null, null, default)).Returns(Task.FromResult<string>(value));
        //    _etcdClientMock.Setup(x => x.Watch(genKey, It.IsAny<Action<WatchEvent[]>>(), null, null, It.IsAny<CancellationToken>()))
        //        .Throws<Exception>();
        //    _configurationSource.UseFullPathForKeys = true;
        //    _configurationSource.ReloadOnChange = true;
        //    _configurationSource.Keys = etcdKey;
        //    _configurationSource.OnWatchFailure = (x) =>
        //    {
        //        throw x;
        //    };
        //    using EtcdConfigurationProvider etcdConfigurationProvider = new EtcdConfigurationProvider(_configurationSource, _etcdClientMock.Object);


        //    //Act
        //    etcdConfigurationProvider.Load();

        //}

        [DataTestMethod]
        [DataRow("test/testkey", "test/testkey:string", "value", "changedvalue")]
        [DataRow("test/testkey1", "test/testkey1:string", "value1", "changedvalue1")]
        [ExpectedException(typeof(EtcdConfigOnLoadException))]
        public void ConfigurationProvider_Test_String_Loader_exception(string genKey, string etcdKey, string value, string changedValue)
        {
            //Arrange
            _etcdClientMock.Setup(x => x.GetValAsync(genKey, null, null, default)).Throws<Exception>();
            _configurationSource.UseFullPathForKeys = true;
            _configurationSource.ReloadOnChange = false;
            _configurationSource.Keys = etcdKey;
            _configurationSource.OnLoadFailure = (x) =>
            {
                throw x;
            };
            using EtcdConfigurationProvider etcdConfigurationProvider = new EtcdConfigurationProvider(_configurationSource, _etcdClientMock.Object);

            //Act
            etcdConfigurationProvider.Load();

        }

        [DataTestMethod]
        [DataRow("test/testkey", "test/testkey:string", "value", "changedvalue")]
        [DataRow("test/testkey1", "test/testkey1:string", "value1", "changedvalue1")]
        public void ConfigurationProvider_Test_String_Loader_NullClient(string genKey, string etcdKey, string value, string changedValue)
        {
            //Arrange
            _etcdClientMock.Setup(x => x.GetValAsync(genKey, null, null, default)).Throws<Exception>();
            _configurationSource.UseFullPathForKeys = true;
            _configurationSource.ReloadOnChange = false;
            _configurationSource.Keys = etcdKey;
            using EtcdConfigurationProvider etcdConfigurationProvider = new EtcdConfigurationProvider(_configurationSource, null);

            //Act
            etcdConfigurationProvider.Load();

            //Assert
            Assert.IsFalse(etcdConfigurationProvider.TryGet(genKey, out string? result));

        }


        [DataTestMethod]
        [DataRow("test/testkey", "test/testkey:string", "value", "changedvalue")]
        [DataRow("test/testkey1", "test/testkey1:string", "value1", "changedvalue1")]
        public void ConfigurationProvider_Test_String_Loader_NullClient_Reload(string genKey, string etcdKey, string value, string changedValue)
        {
            //Arrange
            _etcdClientMock.Setup(x => x.GetValAsync(genKey, null, null, default)).Throws<Exception>();
            _configurationSource.UseFullPathForKeys = true;
            _configurationSource.ReloadOnChange = true;
            _configurationSource.Keys = etcdKey;
            using EtcdConfigurationProvider etcdConfigurationProvider = new EtcdConfigurationProvider(_configurationSource, null);

            //Act
            etcdConfigurationProvider.Load();

            //Assert
            Assert.IsFalse(etcdConfigurationProvider.TryGet(genKey, out string? result));

        }


    }
}
