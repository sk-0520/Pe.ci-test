using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

namespace ContentTypeTextNet.Pe.Main.Views
{
    public static class AvalonEditHelper
    {
        #region function

        public static void SetSyntaxHighlighting(HighlightingManager instance, TextEditor editor, Stream stream)
        {
            using var reader = new System.Xml.XmlTextReader(stream);
            var define = HighlightingLoader.Load(reader, instance);
            editor.SyntaxHighlighting = define;
        }

        public static void SetSyntaxHighlightingDefault(TextEditor editor, Stream stream)
        {
            var instance = HighlightingManager.Instance;
            SetSyntaxHighlighting(instance, editor, stream);
        }

        #endregion
    }
}
