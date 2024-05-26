using System.Xml;


namespace ContentTypeTextNet.Pe.Core.Models.Serialization
{
    public abstract class XmlSerializerBase: SerializerBase
    {
        #region function

        protected virtual XmlReaderSettings CreateXmlReaderSettings()
        {
            return new XmlReaderSettings() {
                CloseInput = false,
            };
        }

        protected virtual XmlWriterSettings CreateXmlWriterSettings()
        {
            return new XmlWriterSettings() {
                CloseOutput = false,
                //Encoding = Encoding,
                NewLineHandling = NewLineHandling.Entitize,
            };
        }

        #endregion
    }
}

