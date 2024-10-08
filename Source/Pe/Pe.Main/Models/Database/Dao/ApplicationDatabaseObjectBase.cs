using System;
using System.Globalization;
using System.Text;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao
{
    public abstract class ApplicationDatabaseObjectBase: DatabaseAccessObjectBase
    {
        protected ApplicationDatabaseObjectBase(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region property

        private ICryptography Cryptography { get; } = new Cryptography();

        #endregion

        #region function

        protected string FromColor(Color color)
        {
            return color.ToString(CultureInfo.InvariantCulture);
        }

        protected Color ToColor(string raw)
        {
            var colorConverter = new ColorConverter();
            var color = (Color)colorConverter.ConvertFrom(raw)!;
            return color;
        }

        protected int ToInt(long value)
        {
            return (int)Math.Clamp(value, int.MinValue, int.MaxValue);
        }

        /// <inheritdoc cref="Cryptography.EncryptStringByCurrentUser" />
        protected byte[] EncryptByCurrentUser(string s)
        {
            return Cryptography.EncryptBinaryByCurrentUser(s);
        }

        /// <summary>
        /// <inheritdoc cref="Cryptography.DecryptBinaryByCurrentUser" />
        /// </summary>
        /// <param name="binary"></param>
        /// <returns></returns>
        protected string DecryptByCurrentUser(byte[]? binary)
        {
            if(binary is null || binary.Length == 0) {
                return string.Empty;
            }

            try {
                var rawBinary = Cryptography.DecryptBinaryByCurrentUser(binary);
                return Encoding.UTF8.GetString(rawBinary);
            } catch {
                return string.Empty;
            }
        }

        #endregion
    }
}
