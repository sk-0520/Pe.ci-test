using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Base.Test
{
    public class HashUtilityTest
    {
        [Theory]
        [InlineData("900150983cd24fb0d6963f7d28e17f72", HashAlgorithmKind.MD5, "abc")]
        [InlineData("a9993e364706816aba3e25717850c26c9cd0d89d", HashAlgorithmKind.SHA1, "abc")]
        [InlineData("ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad", HashAlgorithmKind.SHA256, "abc")]
        [InlineData("cb00753f45a35e8bb5a03d699ac65007272c32ab0eded1631a8b605a43ff5bed8086072ba1e7cc2358baeca134c825a7", HashAlgorithmKind.SHA384, "abc")]
        [InlineData("ddaf35a193617abacc417349ae20413112e6fa4e89a97ea20a9eeee64b55d39a2192992a274fc1a836ba3c23a3feebbd454d4423643ce80e2a9ac94fa54ca49f", HashAlgorithmKind.SHA512, "abc")]
        public void Create_enum_Test(string expected, HashAlgorithmKind algorithmKind, string input)
        {
            using var hash = HashUtility.Create(algorithmKind);

            var binary = Encoding.UTF8.GetBytes(input);
            var result = hash.ComputeHash(binary);
            var actual = BitConverter.ToString(result).Replace("-", "").ToLowerInvariant();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(HashAlgorithmKind.Unknown)]
        [InlineData((HashAlgorithmKind)0-1)]
        [InlineData((HashAlgorithmKind)0+6)]
        public void Create_enum_Throw_NotImplementedException_Test(HashAlgorithmKind algorithmKind)
        {
            Assert.Throws<NotImplementedException>(() => HashUtility.Create(algorithmKind));
        }

        [Theory]
        [InlineData("900150983cd24fb0d6963f7d28e17f72", "MD5", "abc")]
        [InlineData("a9993e364706816aba3e25717850c26c9cd0d89d", "SHA", "abc")]
        [InlineData("a9993e364706816aba3e25717850c26c9cd0d89d", "SHA1", "abc")]
        [InlineData("ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad", "SHA256", "abc")]
        [InlineData("ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad", "SHA-256", "abc")]
        [InlineData("cb00753f45a35e8bb5a03d699ac65007272c32ab0eded1631a8b605a43ff5bed8086072ba1e7cc2358baeca134c825a7", "SHA384", "abc")]
        [InlineData("cb00753f45a35e8bb5a03d699ac65007272c32ab0eded1631a8b605a43ff5bed8086072ba1e7cc2358baeca134c825a7", "SHA-384", "abc")]
        [InlineData("ddaf35a193617abacc417349ae20413112e6fa4e89a97ea20a9eeee64b55d39a2192992a274fc1a836ba3c23a3feebbd454d4423643ce80e2a9ac94fa54ca49f", "SHA512", "abc")]
        [InlineData("ddaf35a193617abacc417349ae20413112e6fa4e89a97ea20a9eeee64b55d39a2192992a274fc1a836ba3c23a3feebbd454d4423643ce80e2a9ac94fa54ca49f", "SHA-512", "abc")]
        public void Create_Name_Test(string expected, string algorithmName, string input)
        {
            using var hash = HashUtility.Create(algorithmName);
            var binary = Encoding.UTF8.GetBytes(input);
            var result = hash.ComputeHash(binary);
            var actual = BitConverter.ToString(result).Replace("-", "").ToLowerInvariant();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("System.Security.Cryptography.SHA1")]
        [InlineData("System.Security.Cryptography.SHA256")]
        [InlineData("System.Security.Cryptography.SHA384")]
        [InlineData("System.Security.Cryptography.SHA512")]
        [InlineData("System.Security.Cryptography.MD5")]
        [InlineData("System.Security.Cryptography.HashAlgorithm")]
        public void Create_Name_Throw_NotSupportedException_Test(string algorithmName)
        {
            Assert.Throws<NotSupportedException>(() => HashUtility.Create(algorithmName));
        }

        [Theory]
        [InlineData("MD4")]
        [InlineData("💩")]
        public void Create_Name_Throw_NotImplementedException_Test(string algorithmName)
        {
            Assert.Throws<NotImplementedException>(() => HashUtility.Create(algorithmName));
        }
    }
}
