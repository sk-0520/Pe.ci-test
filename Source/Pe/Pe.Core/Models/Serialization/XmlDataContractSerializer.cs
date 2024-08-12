using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using ContentTypeTextNet.Pe.Bridge.Models.Data;


namespace ContentTypeTextNet.Pe.Core.Models.Serialization
{
    /// <summary>
    /// <see cref="DataContractSerializer"/>を用いたXMLシリアライズ・デシリアライズ処理。
    /// </summary>
    public class XmlDataContractSerializer: DataContractSerializerBase
    {
        #region DataContractSerializerBase

        protected override TResult LoadImpl<TResult>(Stream stream)
        {
            using(var reader = XmlReader.Create(stream, CreateXmlReaderSettings())) {
                var serializer = new DataContractSerializer(typeof(TResult));
                var rawResult = serializer.ReadObject(reader);

                if(rawResult is TResult result) {
                    return result;
                }

                throw new SerializationException();
            }
        }

        protected override void SaveImpl<TValue>(TValue value, Stream stream)
        {
            using(var writer = XmlWriter.Create(stream, CreateXmlWriterSettings())) {
                var serializer = new DataContractSerializer(value.GetType());
                serializer.WriteObject(writer, value);
            }
        }

        #endregion
    }
}

