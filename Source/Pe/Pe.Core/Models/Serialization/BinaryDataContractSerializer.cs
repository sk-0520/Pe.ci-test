using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using ContentTypeTextNet.Pe.Bridge.Models.Data;


namespace ContentTypeTextNet.Pe.Core.Models.Serialization
{
    /// <summary>
    /// <see cref="DataContractSerializer"/>を用いたバイナリシリアライズ・デシリアライズ処理。
    /// </summary>
    public class BinaryDataContractSerializer: DataContractSerializerBase
    {
        #region DataContractSerializerBase

        protected override TResult LoadImpl<TResult>(Stream stream)
        {
            // 閉じない方法がわっからん
            var quotas = new XmlDictionaryReaderQuotas();
            using(var reader = XmlDictionaryReader.CreateBinaryReader(stream, quotas)) {
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
            using(var writer = XmlDictionaryWriter.CreateBinaryWriter(stream, null, null, false)) {
                var serializer = new DataContractSerializer(value.GetType());
                serializer.WriteObject(writer, value);
            }
        }

        #endregion
    }
}

