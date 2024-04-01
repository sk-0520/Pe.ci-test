using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models
{
    public class CryptographyTest
    {
        #region function

        [Theory]
        [InlineData(1, 0)]
        [InlineData(1, 1)]
        [InlineData(10000, 0)]
        [InlineData(10000, 1)]
        [InlineData(10000, 999)]
        public void EncryptAndDecryptByteTest(int count, int seed)
        {
            var random = new Random(seed);
            byte[] input = new byte[count];
            random.NextBytes(input);

            var cryptography = new Cryptography();

            var output = cryptography.EncryptBinaryByCurrentUser(input);
            var actual = cryptography.DecryptBinaryByCurrentUser(output);
            Assert.Equal(input, actual);
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(1, 1)]
        [InlineData(10000, 0)]
        [InlineData(10000, 1)]
        [InlineData(10000, 999)]
        public void EncryptAndDecryptStringTest(int count, int seed)
        {
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random(seed);
            var cryptography = new Cryptography();

            var input = new string(Enumerable.Repeat(characters, count).Select(s => s[random.Next(s.Length)]).ToArray());
            var output = cryptography.EncryptStringByCurrentUser(input);
            var actual = cryptography.DecryptStringByCurrentUser(output);
            Assert.Equal(input, actual);
        }

        #endregion
    }
}
