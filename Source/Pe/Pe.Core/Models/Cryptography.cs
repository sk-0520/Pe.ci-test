using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public class Cryptography: ICryptography
    {
        #region ICryptography

        /// <inheritdoc cref="ICryptography.EncryptBinaryByCurrentUser"/>
        public byte[] EncryptBinaryByCurrentUser(byte[] rawBinary)
        {
            return ProtectedData.Protect(rawBinary, null, DataProtectionScope.CurrentUser);
        }

        public byte[] EncryptBinaryByCurrentUser(string raw)
        {
            var rawBinary = Encoding.UTF8.GetBytes(raw);
            var encryptBinary = EncryptBinaryByCurrentUser(rawBinary);
            return encryptBinary;
        }


        /// <inheritdoc cref="ICryptography.EncryptStringByCurrentUser"/>
        public string EncryptStringByCurrentUser(string raw)
        {
            var encryptBinary = EncryptBinaryByCurrentUser(raw);
            return Convert.ToBase64String(encryptBinary, Base64FormattingOptions.None);
        }

        /// <inheritdoc cref="ICryptography.DecryptBinaryByCurrentUser"/>
        public byte[] DecryptBinaryByCurrentUser(byte[] encryptBinary)
        {
            return ProtectedData.Unprotect(encryptBinary, null, DataProtectionScope.CurrentUser);
        }

        /// <inheritdoc cref="ICryptography.DecryptBinaryByCurrentUser"/>
        public string DecryptStringByCurrentUser(byte[] encryptBinary)
        {
            var rawBinary = DecryptBinaryByCurrentUser(encryptBinary);
            return Encoding.UTF8.GetString(rawBinary);
        }

        /// <inheritdoc cref="ICryptography.DecryptBinaryByCurrentUser"/>
        public string DecryptStringByCurrentUser(string encryptBase64)
        {
            var encryptBinary = Convert.FromBase64String(encryptBase64);
            return DecryptStringByCurrentUser(encryptBinary);
        }

        #endregion
    }
}
