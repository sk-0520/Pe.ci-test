using System.Text;
using System.Xml;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Main.CrashReport.Models
{
    internal class CrashReportSerializer: XmlDataContractSerializer
    {
        public CrashReportSerializer()
        {
        }

        #region XmlDataContractSerializer

        protected override XmlReaderSettings CreateXmlReaderSettings()
        {
            return new XmlReaderSettings() {
                CloseInput = false,
            };
        }

        protected override XmlWriterSettings CreateXmlWriterSettings()
        {
            return new XmlWriterSettings() {
                CloseOutput = false,
                NewLineHandling = NewLineHandling.Entitize,
                Encoding = Encoding.Unicode,
            };
        }

        #endregion



    }
}
