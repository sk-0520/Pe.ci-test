using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.Pe.Library.FormsCushion
{
    /// <summary>
    /// <para>https://msdn.microsoft.com/ja-jp/library/cc488002.aspx</para>
    /// </summary>
    public class ConvertRichText
    {
        public string ToPlainText(string rtf)
        {
            using(var richTextBox = new RichTextBox() {
                Rtf = rtf,
            }) {
                return richTextBox.Text;
            }
        }

        string ToRichText(string text, Font font)
        {
            using(var richTextBox = new RichTextBox() {
                Text = text,
                Font = font,
            }) {
                return richTextBox.Rtf;
            }
        }
        public string ToRichText(string text, FontModel font)
        {
            var style = (FontStyle)0;
            if(font.Italic) style |= FontStyle.Italic;
            if(font.Bold) style |= FontStyle.Bold;

            using(var formsFont = new Font(font.Family, DrawingUtility.ConvertFontSizeFromWpf(font.Size), style)) {
                return ToRichText(text, formsFont);
            }
        }
    }
}
