using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;


namespace ContentTypeTextNet.Pe.Core.Models.Serialization
{
    /// <summary>
    /// <see cref="DataContractJsonSerializer"/>を用いたシリアライズ・デシリアライズ処理。
    /// </summary>
    public class JsonDataSerializer: SerializerBase
    {
        #region SerializerBase

        protected override TResult LoadImpl<TResult>(Stream stream)
        {
            var serializer = new DataContractJsonSerializer(typeof(TResult));
            var rawResult = serializer.ReadObject(stream);

            if(rawResult is TResult result) {
                return result;
            }

            throw new SerializationException();
        }

        protected override void SaveImpl<TValue>(TValue value, Stream stream)
        {
            var serializer = new DataContractJsonSerializer(value.GetType());
            serializer.WriteObject(stream, value);
        }

        #endregion
    }
}

