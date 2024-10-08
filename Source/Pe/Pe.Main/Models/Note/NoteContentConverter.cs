using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Note
{
    public class NoteContentConverter
    {
        public NoteContentConverter(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        /// <inheritdoc cref="ILoggerFactory"/>
        private ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        private ILogger Logger { get; }
        public Encoding Encoding { get; init; } = EncodingConverter.DefaultEncoding;
        public string RichTextFormat { get; init; } = DataFormats.Rtf;

        #endregion

        public string ToRichText(string plainText, IFont font, Color foregroundColor)
        {
            var document = new FlowDocument();
            using(Initializer.Begin(document)) {
                var fontConverter = new FontConverter(LoggerFactory);
                document.FontFamily = fontConverter.MakeFontFamily(font.FamilyName, SystemFonts.MessageFontFamily);
                document.FontSize = font.Size;
                document.FontWeight = fontConverter.ToWeight(font.IsBold);
                document.FontStyle = fontConverter.ToStyle(font.IsItalic);
                document.Foreground = FreezableUtility.GetSafeFreeze(new SolidColorBrush(foregroundColor));

                var lines = TextUtility.ReadLines(plainText)
                    .Select(i => new Run(i))
                    .Select(i => new Paragraph(i))
                ;
                document.Blocks.AddRange(lines);
            }

            return ToRtfString(document);
        }

        public string ToRtfString(FlowDocument document)
        {
            var range = new TextRange(document.ContentStart, document.ContentEnd);
            using(var stream = new MemoryReleaseStream()) {
                range.Save(stream, RichTextFormat);
                var contentValue = Encoding.GetString(stream.ToArray());
                return contentValue;
            }
        }

        public FlowDocument ToFlowDocument(string content)
        {
            var document = new FlowDocument();
            using(Initializer.Begin(document)) {
                var range = new TextRange(document.ContentStart, document.ContentEnd);
                using(var stream = new MemoryReleaseStream(Encoding.GetBytes(content))) {
                    stream.Seek(0, SeekOrigin.Begin);
                    range.Load(stream, RichTextFormat);
                }
            }

            return document;
        }

        public Stream ToRtfStream(string rtf)
        {
            var binary = Encoding.UTF8.GetBytes(rtf);
            var stream = new MemoryReleaseStream(binary);
            return stream;
        }

        public string ToPlain(string rtfText)
        {
            var converter = new RtfConverter();
            return converter.ToString(rtfText);
        }
    }
}
