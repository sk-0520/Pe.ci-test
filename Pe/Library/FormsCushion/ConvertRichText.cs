using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public string ToRichText(string text)
        {
            using(var richTextBox = new RichTextBox() {
                Text = text,
            }) {
                return richTextBox.Rtf;
            }
        }
    }
}
