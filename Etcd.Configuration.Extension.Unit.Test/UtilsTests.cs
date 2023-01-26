using Etcd.Configuration.Extension.Models;
using Etcd.Configuration.Extension.Utils;

namespace Etcd.Configuration.Extension.Unit.Test
{
    [TestClass]
    public class UtilsTests
    {
        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("SYZ")]
        [DataRow("SYZ,XYZ")]
        [DataRow("SYZ:TEST")]
        public void Utils_Generate_Keys_Wrong_Condition(string data)
        {
            //Act
            var list=data.GenerateKeys();
            //Assert
            Assert.IsNotNull(list);
            Assert.AreEqual(list.Count, 0);
        }

        [DataTestMethod]
        [DataRow("Test:json,Test:yml")]
        [DataRow("Test#test,Test:json")]
        [DataRow("Test:string, test:yml")]
        public void Utils_Generate_Keys_Right_Condition(string data)
        {
            //Act
            var list = data.GenerateKeys();
            //Assert
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count==1);
        }

        [DataTestMethod]
        [DataRow("Test:json","Test", 0)]
        [DataRow(" Test:string","Test", 1)]
        public void Utils_Generate_Keys_Value_Check(string data, string result, int valueTypes)
        {
            //Act
            var list = data.GenerateKeys();
            //Assert
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count == 1);
            Assert.IsTrue(list[0].KeyName.Equals(result));
            Assert.IsTrue(list[0].ValueType==(ValueTypes)valueTypes);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Utils_Generate_Keys_Exception_Check()
        {
            //arrange
            string data = ":";
            //Act
            var list = data.GenerateKeys();

        }
    }
}