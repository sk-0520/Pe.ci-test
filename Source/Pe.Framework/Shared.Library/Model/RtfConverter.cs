using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
{
    public class RtfConverter
    {
        #region function

        public string ToString(string rtf)
        {
            using(var formRichTextBox = new System.Windows.Forms.RichTextBox() {
                Rtf = rtf
            }) {
                return formRichTextBox.Text;
            }
        }

        #endregion
    }
}
