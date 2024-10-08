using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.Base
{
    public enum HashAlgorithmKind
    {
        Unknown,
        SHA1,
        SHA256,
        SHA384,
        SHA512,
        MD5,
    }

    public static class HashUtility
    {
        #region function

        public static HashAlgorithm Create(HashAlgorithmKind hashAlgorithmKind)
        {
            return hashAlgorithmKind switch {
                HashAlgorithmKind.SHA1 => SHA1.Create(),
                HashAlgorithmKind.SHA256 => SHA256.Create(),
                HashAlgorithmKind.SHA384 => SHA384.Create(),
                HashAlgorithmKind.SHA512 => SHA512.Create(),
                HashAlgorithmKind.MD5 => MD5.Create(),
                _ => throw new NotImplementedException()
            };
        }

        /// <summary>
        /// .NET7 で使えなくなった <see cref="HashAlgorithm.Create(string)"/> のラッパー。
        /// </summary>
        /// <param name="algorithmName"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"><c>System.Security.*</c>系を指定。</exception>
        /// <exception cref="NotImplementedException"></exception>
        public static HashAlgorithm Create(string algorithmName)
        {
            switch(algorithmName.ToUpperInvariant()) {
                case "SHA":
                case "SHA1":
                    return Create(HashAlgorithmKind.SHA1);

                case "SHA256":
                case "SHA-256":
                    return Create(HashAlgorithmKind.SHA256);

                case "SHA384":
                case "SHA-384":
                    return Create(HashAlgorithmKind.SHA384);

                case "SHA512":
                case "SHA-512":
                    return Create(HashAlgorithmKind.SHA512);

                case "MD5":
                    return Create(HashAlgorithmKind.MD5);

                case "SYSTEM.SECURITY.CRYPTOGRAPHY.SHA1":
                case "SYSTEM.SECURITY.CRYPTOGRAPHY.SHA256":
                case "SYSTEM.SECURITY.CRYPTOGRAPHY.SHA384":
                case "SYSTEM.SECURITY.CRYPTOGRAPHY.SHA512":
                case "SYSTEM.SECURITY.CRYPTOGRAPHY.MD5":
                case "SYSTEM.SECURITY.CRYPTOGRAPHY.HASHALGORITHM":
                    throw new NotSupportedException(algorithmName);

                default:
                    throw new NotImplementedException(algorithmName);
            }
        }

        #endregion
    }
}
