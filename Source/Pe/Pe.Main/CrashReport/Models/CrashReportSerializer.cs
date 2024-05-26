using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using ContentTypeTextNet.Pe.Core.Models.Serialization;

namespace ContentTypeTextNet.Pe.Main.CrashReport.Models
{
    //TODO: JsonTextSerializer に統合
    internal sealed class CrashReportSerializer: SerializerBase
    {
        public CrashReportSerializer()
        { }

        #region SerializerBase

        protected override TResult LoadImpl<TResult>(Stream stream)
        {
            using var reader = CreateReader(stream);
            var json = reader.ReadToEnd();
            var result = System.Text.Json.JsonSerializer.Deserialize<TResult>(json);
            if(result is null) {
                throw new SerializationException();
            }

            return result;
        }

        protected override void SaveImpl(object value, Stream stream)
        {
            using var writer = CreateWriter(stream);
            var json = System.Text.Json.JsonSerializer.Serialize(value); // TODO: ストリーム直接でいいと思う
            writer.Write(json);
        }

        #endregion
    }
}
