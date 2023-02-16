using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ContentTypeTextNet.Pe.Standard.Base.Models
{
    public static class HashUtility
    {
        #region function

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
                    return SHA1.Create();

                case "SHA256":
                case "SHA-256":
                    return SHA256.Create();

                case "SHA384":
                case "SHA-384":
                    return SHA384.Create();

                case "SHA512":
                case "SHA-512":
                    return SHA512.Create();

                case "MD5":
                    return MD5.Create();

                case "System.Security.Cryptography.SHA1":
                case "System.Security.Cryptography.SHA256":
                case "System.Security.Cryptography.SHA384":
                case "System.Security.Cryptography.SHA512":
                case "System.Security.Cryptography.MD5":
                case "System.Security.Cryptography.HashAlgorithm":
                    throw new NotSupportedException(algorithmName);

                default:
                    throw new NotImplementedException(algorithmName);
            }
        }

        #endregion
    }
}
