using System;
using System.Buffers;
using System.IO;
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

        /// <summary>
        /// <see cref="byte"/>配列使用時のスタック配置サイズ上限。
        /// </summary>
        public int StackSize { get; init; } = 512;

        public ArrayPool<byte> ArrayPool { get; init; } = ArrayPool<byte>.Shared;

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
            var length = (int)stream.Length;
            var allocHeap = StackSize < length;

            byte[]? heapBuffer = null;
            Span<byte> buffer = allocHeap
                ? (heapBuffer = ArrayPool.Rent(length)).AsSpan()
                : stackalloc byte[length]
            ;

            stream.Read(buffer);
            var reader = new Utf8JsonReader(buffer, ReaderOptions);
            try {
                var rawResult = JsonSerializer.Deserialize<TResult>(ref reader);
                if(rawResult is TResult result) {
                    return result;
                }
            } finally {
                if(heapBuffer is not null) {
                    ArrayPool.Return(heapBuffer);
                }
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

