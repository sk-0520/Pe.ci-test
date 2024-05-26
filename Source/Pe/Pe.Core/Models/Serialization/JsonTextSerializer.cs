using System;
using System.Buffers;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace ContentTypeTextNet.Pe.Core.Models.Serialization
{
    public class JsonTextSerializer: SerializerBase
    {
        #region define

        public class VersionConverter: JsonConverter<Version>
        {
            public override Version Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var s = reader.GetString();
                if(string.IsNullOrWhiteSpace(s)) {
                    return null!;
                }
                return new Version(s);
            }

            public override void Write(Utf8JsonWriter writer, Version version, JsonSerializerOptions options)
            {
                if(version is null) {
                    writer.WriteNullValue();
                } else {
                    writer.WriteStringValue(version.ToString());
                }
            }
        }

        #endregion

        #region property

        public JsonReaderOptions ReaderOptions { get; set; } = new JsonReaderOptions() {
            AllowTrailingCommas = true,
            CommentHandling = JsonCommentHandling.Skip,
        };

        public JsonWriterOptions WriterOptions { get; set; } = new JsonWriterOptions() {
            Indented = true,
            Encoder = JavaScriptEncoder.Default,
        };

        #endregion

        #region SerializerBase

        protected override TResult LoadImpl<TResult>(Stream stream)
        {
            var options = new JsonSerializerOptions() {
                AllowTrailingCommas = ReaderOptions.AllowTrailingCommas,
                ReadCommentHandling = ReaderOptions.CommentHandling,
            };

            var rawResult = JsonSerializer.Deserialize(stream, typeof(TResult), options);
            if(rawResult is TResult result) {
                return result;
            }

            throw new SerializationException();
        }

        protected override void SaveImpl(object value, Stream stream)
        {
            using(var writer = new Utf8JsonWriter(stream, WriterOptions)) {
                JsonSerializer.Serialize(writer, value);
            }
        }

        #endregion
    }
}

