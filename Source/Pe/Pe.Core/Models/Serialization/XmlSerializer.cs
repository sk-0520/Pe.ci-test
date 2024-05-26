using System.IO;
using System.Runtime.Serialization;
using System.Xml;


namespace ContentTypeTextNet.Pe.Core.Models.Serialization
{
    /// <summary>
    /// <see cref="System.Xml.Serialization.XmlSerializer"/>を用いたシリアライズ・デシリアライズ処理。
    /// </summary>
    public class XmlSerializer: XmlSerializerBase
    {
        #region XmlSerializerBase

        protected override TResult LoadImpl<TResult>(Stream stream)
        {
            using(var reader = XmlReader.Create(stream, CreateXmlReaderSettings())) {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(TResult));
                var rawResult = serializer.Deserialize(reader);

                if(rawResult is TResult result) {
                    return result;
                }

                throw new SerializationException();
            }
        }

        protected override void SaveImpl(object value, Stream stream)
        {
            using(var writer = XmlWriter.Create(stream, CreateXmlWriterSettings())) {
                var serializer = new System.Xml.Serialization.XmlSerializer(value.GetType());
                serializer.Serialize(writer, value);
            }
        }

        #endregion
    }
}

