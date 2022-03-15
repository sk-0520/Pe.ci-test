using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Bridge.Test.Models
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
        public void EncryptAndDecryptTest(int count, int seed)
        {
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random(seed);

            var input = new string(Enumerable.Repeat(characters, count).Select(s => s[random.Next(s.Length)]).ToArray());
            var output = Cryptography.EncryptStringByUser(input);
            var actual = Cryptography.DecryptStringByUser(output);
            Assert.AreEqual(input, actual);
        }

        #endregion
    }
}
