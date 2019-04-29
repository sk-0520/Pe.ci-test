using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Logic;

namespace ContentTypeTextNet.Pe.Main.Model.Note
{
    public class NoteContentConverter
    {
        public NoteContentConverter(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateTartget(GetType());
        }

        #region property

        ILogger Logger { get; }
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        #endregion

        public string ToRichText(string plainText, FontData fontData, Color foregroundColor, IDispatcherWapper dispatcherWapper)
        {
            var document = new FlowDocument();
            var fontConverter = new FontConverter(Logger.Factory);
            document.FontFamily = fontConverter.MakeFontFamily(fontData.FamilyName, SystemFonts.MessageFontFamily);
            document.FontSize = fontData.Size;
            document.FontWeight = fontConverter.ToWeight(fontData.IsBold);
            document.FontStyle = fontConverter.ToStyle(fontData.IsItalic);
            document.Foreground = dispatcherWapper.Get(() => FreezableUtility.GetSafeFreeze(new SolidColorBrush(foregroundColor)));

            document.Blocks.Add(new Paragraph(new Run(plainText)));


            var range = new TextRange(document.ContentStart, document.ContentEnd);
            using(var stream = new MemoryStream()) {
                range.Save(stream, DataFormats.Xaml);
                return Encoding.GetString(stream.ToArray());
            }
        }

    }
}
