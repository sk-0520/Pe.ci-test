using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models
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
