using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    [TestClass]
    public class CryptographyTest
    {
        #region function

        [TestMethod]
        [DataRow(1, 0)]
        [DataRow(1, 1)]
        [DataRow(10000, 0)]
        [DataRow(10000, 1)]
        [DataRow(10000, 999)]
        public void EncryptAndDecryptByteTest(int count, int seed)
        {
            var random = new Random(seed);
            byte[] input = new byte[count];
            random.NextBytes(input);

            var cryptography = new Cryptography();

            var output = cryptography.EncryptBinaryByCurrentUser(input);
            var actual = cryptography.DecryptBinaryByCurrentUser(output);
            CollectionAssert.AreEqual(input, actual);
        }

        [TestMethod]
        [DataRow(1, 0)]
        [DataRow(1, 1)]
        [DataRow(10000, 0)]
        [DataRow(10000, 1)]
        [DataRow(10000, 999)]
        public void EncryptAndDecryptStringTest(int count, int seed)
        {
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random(seed);
            var cryptography = new Cryptography();

            var input = new string(Enumerable.Repeat(characters, count).Select(s => s[random.Next(s.Length)]).ToArray());
            var output = cryptography.EncryptStringByCurrentUser(input);
            var actual = cryptography.DecryptStringByCurrentUser(output);
            Assert.AreEqual(input, actual);
        }

        #endregion
    }
}
