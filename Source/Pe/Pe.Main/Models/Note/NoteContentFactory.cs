using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;

namespace ContentTypeTextNet.Pe.Main.Models.Note
{
    public class NoteContentFactory
    {
        #region property

        public Encoding Encoding { get; set; } = EncodingConverter.DefaultEncoding;
        public string RichTextFormat { get; set; } = DataFormats.Rtf;

        #endregion

        #region function

        public string CreatePlain() => string.Empty;

        public string CreateRichText() => @"{\rtf1}";


        #endregion
    }
}
