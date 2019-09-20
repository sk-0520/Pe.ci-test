using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
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

        ILoggerFactory LoggerFactory { get; }
        ILogger Logger { get; }
        public Encoding Encoding { get; set; } = EncodingUtility.UTF8n;
        public string RichTextFormat { get; set; } = DataFormats.Rtf;

        #endregion

        public string ToRichText(string plainText, FontData fontData, Color foregroundColor)
        {
            var document = new FlowDocument();
            using(Initializer.BeginInitialize(document)) {
                var fontConverter = new FontConverter(LoggerFactory);
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
                document.FontFamily = fontConverter.MakeFontFamily(fontData.FamilyName, SystemFonts.MessageFontFamily);
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
                document.FontSize = fontData.Size;
                document.FontWeight = fontConverter.ToWeight(fontData.IsBold);
                document.FontStyle = fontConverter.ToStyle(fontData.IsItalic);
                document.Foreground = FreezableUtility.GetSafeFreeze(new SolidColorBrush(foregroundColor));

                document.Blocks.Add(new Paragraph(new Run(plainText)));
            }

            return ToRtfString(document);
        }

        public string ToRtfString(FlowDocument document)
        {
            var range = new TextRange(document.ContentStart, document.ContentEnd);
            using(var stream = new MemoryStream()) {
                range.Save(stream, RichTextFormat);
                var contentValue = Encoding.GetString(stream.ToArray());
                return contentValue;
            }
        }

        public FlowDocument ToFlowDocument(string content)
        {
            var document = new FlowDocument();
            using(Initializer.BeginInitialize(document)) {
                var range = new TextRange(document.ContentStart, document.ContentEnd);
                using(var stream = new MemoryStream(Encoding.GetBytes(content))) {
                    stream.Seek(0, SeekOrigin.Begin);
                    range.Load(stream, RichTextFormat);
                }
            }

            return document;
        }

        public string ToPlain(string rtfText)
        {
            var converter = new RtfConverter();
            return converter.ToString(rtfText);
        }

        public string ToLinkSettingString(NoteLinkContentData linkData)
        {
            var serializer = new XmlDataContractSerializer();
            using(var stream = new MemoryStream()) {
                serializer.Save(linkData, stream);
                return Encoding.GetString(stream.ToArray());
            }
        }
        public NoteLinkContentData ToLinkSetting(string rawSetting)
        {
            var serializer = new XmlDataContractSerializer();
            using(var stream = new MemoryStream()) {
                using(var writer = new StreamWriter(stream, Encoding)) {
                    writer.Write(rawSetting);
                    writer.Flush();
                    stream.Seek(0, SeekOrigin.Begin);
                    return serializer.Load<NoteLinkContentData>(stream);
                }
            }
        }
    }
}
