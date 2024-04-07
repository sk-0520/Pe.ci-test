using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Bridge.Models
{
    /// <summary>
    /// 暗号化処理。
    /// </summary>
    /// <remarks>
    /// <para>Pe から提供される。</para>
    /// </remarks>
    public interface ICryptography
    {
        #region function

        /// <summary>
        /// 現在ユーザーのみに復元可能な暗号化処理を実施。
        /// </summary>
        /// <param name="rawBinary">非暗号化データ</param>
        /// <returns>暗号化データ。</returns>
        /// <seealso cref="ProtectedData.Protect"/>
        byte[] EncryptBinaryByCurrentUser(byte[] rawBinary);

        /// <inheritdoc cref="EncryptBinaryByCurrentUser(byte[])"/>
        byte[] EncryptBinaryByCurrentUser(string raw);

        /// <inheritdoc cref="EncryptBinaryByCurrentUser(byte[])"/>
        string EncryptStringByCurrentUser(string raw);


        /// <summary>
        /// 暗号化データを現在ユーザーのみに限定した復元化処理を実施。
        /// </summary>
        /// <param name="encryptBinary"></param>
        /// <returns></returns>
        /// <seealso cref="ProtectedData.Unprotect"/>
        byte[] DecryptBinaryByCurrentUser(byte[] encryptBinary);

        /// <inheritdoc cref="DecryptBinaryByCurrentUser(byte[])"/>
        string DecryptStringByCurrentUser(byte[] encryptBinary);

        /// <inheritdoc cref="DecryptBinaryByCurrentUser(byte[])"/>
        string DecryptStringByCurrentUser(string encryptBase64);

        #endregion
    }
}
