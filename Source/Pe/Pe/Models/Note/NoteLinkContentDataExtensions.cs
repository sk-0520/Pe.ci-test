using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Note
{
        [Obsolete]
    public static class NoteLinkContentDataExtensions
    {
        #region function

        [Obsolete]
        public static FileInfo ToFileInfo(this NoteLinkContentData @this)
        {
            var filePath = Environment.ExpandEnvironmentVariables(@this.FilePath?.Trim() ?? string.Empty);
            return new FileInfo(filePath);
        }

        [Obsolete]
        public static Encoding ToEncoding(this NoteLinkContentData @this)
        {
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
            return EncodingUtility.Parse(@this.EncodingName);
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
        }

        #endregion
    }
}
