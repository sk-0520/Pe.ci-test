using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Main.CrashReport.Models
{
    //TODO: JsonTextSerializer に統合
    internal sealed class CrashReportSerializer: SerializerBase
    {
        public CrashReportSerializer()
        { }

        #region SerializerBase

        public override TResult Load<TResult>(Stream stream)
        {
            using var reader = GetReader(stream);
            var json = reader.ReadToEnd();
            var result = System.Text.Json.JsonSerializer.Deserialize<TResult>(json);
            if(result is null) {
                throw new SerializationException();
            }

            return result;
        }

        public override void Save(object value, Stream stream)
        {
            using var writer = GetWriter(stream);
            var json = System.Text.Json.JsonSerializer.Serialize(value); // TODO: ストリーム直接でいいと思う
            writer.Write(json);
        }

        #endregion
    }
}
