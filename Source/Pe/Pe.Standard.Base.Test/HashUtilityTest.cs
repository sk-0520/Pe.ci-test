using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Standard.Base.Test
{
    [TestClass]
    public class HashUtilityTest
    {
        [TestMethod]
        [DataRow("900150983cd24fb0d6963f7d28e17f72", "MD5", "abc")]
        [DataRow("a9993e364706816aba3e25717850c26c9cd0d89d", "SHA", "abc")]
        [DataRow("a9993e364706816aba3e25717850c26c9cd0d89d", "SHA1", "abc")]
        [DataRow("ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad", "SHA256", "abc")]
        [DataRow("ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad", "SHA-256", "abc")]
        [DataRow("cb00753f45a35e8bb5a03d699ac65007272c32ab0eded1631a8b605a43ff5bed8086072ba1e7cc2358baeca134c825a7", "SHA384", "abc")]
        [DataRow("cb00753f45a35e8bb5a03d699ac65007272c32ab0eded1631a8b605a43ff5bed8086072ba1e7cc2358baeca134c825a7", "SHA-384", "abc")]
        [DataRow("ddaf35a193617abacc417349ae20413112e6fa4e89a97ea20a9eeee64b55d39a2192992a274fc1a836ba3c23a3feebbd454d4423643ce80e2a9ac94fa54ca49f", "SHA512", "abc")]
        [DataRow("ddaf35a193617abacc417349ae20413112e6fa4e89a97ea20a9eeee64b55d39a2192992a274fc1a836ba3c23a3feebbd454d4423643ce80e2a9ac94fa54ca49f", "SHA-512", "abc")]
        public void CreateTest(string expected, string algorithmName, string input)
        {
            using var hash = HashUtility.Create(algorithmName);
            var binary = Encoding.UTF8.GetBytes(input);
            var result = hash.ComputeHash(binary);
            var actual = BitConverter.ToString(result).Replace("-", "").ToLowerInvariant();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow("System.Security.Cryptography.SHA1")]
        [DataRow("System.Security.Cryptography.SHA256")]
        [DataRow("System.Security.Cryptography.SHA384")]
        [DataRow("System.Security.Cryptography.SHA512")]
        [DataRow("System.Security.Cryptography.MD5")]
        [DataRow("System.Security.Cryptography.HashAlgorithm")]
        public void Create_Throw_NotSupportedException_Test(string algorithmName)
        {
            Assert.ThrowsException<NotSupportedException>(() => HashUtility.Create(algorithmName));
        }

        [TestMethod]
        [DataRow("MD4")]
        [DataRow("💩")]
        public void Create_Throw_NotImplementedException_Test(string algorithmName)
        {
            Assert.ThrowsException<NotImplementedException>(() => HashUtility.Create(algorithmName));
        }
    }
}
