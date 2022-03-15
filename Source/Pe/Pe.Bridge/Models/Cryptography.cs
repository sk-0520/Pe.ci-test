using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Bridge.Models
{
    public static class Cryptography
    {
        #region function

        /// <summary>
        /// 現在ユーザーのみに復元可能な暗号化処理を実施。
        /// </summary>
        /// <param name="rawBinary">非暗号化データ</param>
        /// <returns>暗号化データ。</returns>
        /// <seealso cref="ProtectedData.Protect"/>
        public static byte[] EncryptBinaryByUser(byte[] rawBinary)
        {
            return ProtectedData.Protect(rawBinary, null, DataProtectionScope.CurrentUser);
        }

        /// <inheritdoc cref="EncryptBinaryByUser"/>
        public static string EncryptStringByUser(string raw)
        {
            var rawBinary = Encoding.UTF8.GetBytes(raw);
            var encryptBinary = EncryptBinaryByUser(rawBinary);
            return Convert.ToBase64String(encryptBinary, Base64FormattingOptions.None);
        }

        /// <summary>
        /// 暗号化データを現在ユーザーのみに限定した復元化処理を実施。
        /// </summary>
        /// <param name="encryptBinary"></param>
        /// <returns></returns>
        /// <seealso cref="ProtectedData.Unprotect"/>
        public static byte[] DecryptBinaryByUser(byte[] encryptBinary)
        {
            return ProtectedData.Unprotect(encryptBinary, null, DataProtectionScope.CurrentUser);
        }

        /// <inheritdoc cref="DecryptBinaryByUser"/>
        public static string DecryptStringByUser(string encryptBase64)
        {
            var encryptBinary = Convert.FromBase64String(encryptBase64);
            var rawBinary = DecryptBinaryByUser(encryptBinary);
            return Encoding.UTF8.GetString(rawBinary);
        }

        #endregion
    }
}
