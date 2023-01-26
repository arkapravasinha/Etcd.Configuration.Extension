using Etcd.Configuration.Extension.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etcd.Configuration.Extension.Unit.Test
{
    [TestClass]
    public class JsonParserTests
    {
        [DataTestMethod]
        [DataRow("/Data/JsonParser.json")]
        public void Json_Parser_Parsing_Test(string filePath)
        {
            //Act
            var data = new FileStream(Environment.CurrentDirectory + filePath,FileMode.Open,FileAccess.Read);

            //Arrange
            JsonParser jsonParser = new JsonParser();
            var result = jsonParser.Parse(data);

            //Assert

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
            Assert.IsTrue(result.Any(x => x.Key.Equals("key1") && (x.Value?.Equals("value_key1") ?? false)));
            Assert.IsTrue(result.Any(x => x.Key.Equals("key2:child_key2_key1") && (x.Value?.Equals("value_key2_0") ?? false)));
            Assert.IsTrue(result.Any(x => x.Key.Equals("key2:child_key2_key2") && (x.Value?.Equals("value_key2_1") ?? false)));
        }
    }
}
