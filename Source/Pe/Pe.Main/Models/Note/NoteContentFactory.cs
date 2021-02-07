using System.Text;
using System.Windows;
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
