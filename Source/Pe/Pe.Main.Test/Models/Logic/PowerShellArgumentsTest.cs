using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Logic
{
    [TestClass]
    public class PowerShellArgumentsTest
    {
        #region function

        [TestMethod]
        public void Create_Exception_Test()
        {
            var psa = new PowerShellArguments();
            Assert.ThrowsException<ArgumentException>(() => psa.Create("k y", "value"));
            Assert.ThrowsException<ArgumentException>(() => psa.Create("k ", "value"));
            Assert.ThrowsException<ArgumentException>(() => psa.Create(" k", "value"));
            Assert.ThrowsException<ArgumentException>(() => psa.Create(" ", "value"));
            Assert.ThrowsException<ArgumentException>(() => psa.Create("", "value"));
        }

        [TestMethod]
        [DataRow("-k", "\"\"", "k", "")]
        [DataRow("-k", "v", "k", "v")]
        [DataRow("-k", "v", "-k", "v")]
        [DataRow("-k", "\"v v\"", "k", "v v")]
        [DataRow("-k", "\"v\"\" \"\"v\"", "k", "v\" \"v")]
        public void CreateTest(string resultKey, string resultValue, string inputKey, string inputValue)
        {
            var psa = new PowerShellArguments();
            var p = KeyValuePair.Create(inputKey, inputValue);
            var actual1 = psa.Create(p);
            var actual2 = psa.Create(inputKey, inputValue);

            Assert.AreEqual(resultKey, actual1.Key);
            Assert.AreEqual(resultValue, actual1.Value);
            Assert.AreEqual(resultKey, actual2.Key);
            Assert.AreEqual(resultValue, actual2.Value);
        }

        [TestMethod]
        public void CreateParameters()
        {
            var psa = new PowerShellArguments();
            var parameters = psa.CreateParameters(false, new[]{
                KeyValuePair.Create("key1", "value1"),
                KeyValuePair.Create("-key2", "value2"),
                KeyValuePair.Create("-key3", "value 3"),
            });
            Assert.AreEqual(parameters[0], "-key1");
            Assert.AreEqual(parameters[1], "value1");

            Assert.AreEqual(parameters[2], "-key2");
            Assert.AreEqual(parameters[3], "value2");

            Assert.AreEqual(parameters[4], "-key3");
            Assert.AreEqual(parameters[5], "\"value 3\"");


        }

        [TestMethod]
        [DataRow("\"\"\"\"\"\"", "")]
        [DataRow("\"\"\"ab\"\"\"", "ab")]
        [DataRow("\"\"\"a b\"\"\"", "a b")]
        [DataRow("\"\"\"a\"\"b\"\"\"", "a\"b")]
        public void ToRemainingValueTest(string result, string input)
        {
            var psa = new PowerShellArguments();
            var actual = psa.ToRemainingValue(input);
            Assert.AreEqual(result, actual);
        }

        #endregion
    }
}
