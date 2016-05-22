using System;
using System.Collections.Generic;
using Drawing = System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forms = System.Windows.Forms;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using System.Windows.Media;

namespace ContentTypeTextNet.Pe.Library.FormsCushion
{
    /// <summary>
    /// <para>https://msdn.microsoft.com/ja-jp/library/cc488002.aspx</para>
    /// </summary>
    public class RichTextConverter
    {
        public string ToPlainText(string rtf)
        {
            using(var richTextBox = new Forms.RichTextBox() {
                Rtf = rtf,
            }) {
                return richTextBox.Text;
            }
        }

        string ToRichText(string text, Drawing.Font font, Drawing.Color foreColor)
        {
            using(var richTextBox = new Forms.RichTextBox() {
                Text = text,
                Font = font,
                ForeColor = foreColor,
            }) {
                return richTextBox.Rtf;
            }
        }
        public string ToRichText(string text, FontModel font, Color foreColor)
        {
            var style = (Drawing.FontStyle)0;
            if(font.Italic) style |= Drawing.FontStyle.Italic;
            if(font.Bold) style |= Drawing.FontStyle.Bold;

            var drawingForeColor = Drawing.Color.FromArgb(foreColor.R, foreColor.G, foreColor.B);

            using(var formsFont = new Drawing.Font(font.Family, DrawingUtility.ConvertFontSizeFromWpf(font.Size), style)) {
                return ToRichText(text, formsFont, drawingForeColor);
            }
        }
    }
}
