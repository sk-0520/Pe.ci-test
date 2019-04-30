using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;

namespace ContentTypeTextNet.Pe.Main.Model.Note
{
    public static class NoteLinkContentDataExtensions
    {
        #region function

        public static FileInfo ToFileInfo(this NoteLinkContentData @this)
        {
            var filePath = Environment.ExpandEnvironmentVariables(@this.FilePath?.Trim() ?? string.Empty);
            return new FileInfo(filePath);
        }

        public static Encoding ToEncoding(this NoteLinkContentData @this)
        {
            return EncodingUtility.Parse(@this.EncodingName);
        }

        #endregion
    }
}
