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

        /// <summary>
        /// デフォルト読み込み設定。
        /// </summary>
        public static JsonSerializerOptions DefaultReaderOptions { get; set; } = new JsonSerializerOptions() {
            AllowTrailingCommas = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
        };

        /// <summary>
        /// デフォルト書き込み設定。
        /// </summary>
        public static JsonWriterOptions DefaultWriterOptions { get; set; } = new JsonWriterOptions() {
            Indented = true,
            Encoder = JavaScriptEncoder.Default,
        };

        /// <summary>
        /// 読み込み設定。
        /// </summary>
        /// <remarks>
        /// <see langword="null" />の場合はデフォルト設定(<see cref="DefaultReaderOptions"/>)を使用する。
        /// </remarks>
        public JsonSerializerOptions? ReaderOptions { get; set; }

        /// <summary>
        /// 書き込み設定。
        /// </summary>
        /// <remarks>
        /// <see langword="null" />の場合はデフォルト設定(<see cref="DefaultWriterOptions"/>)を使用する。
        /// </remarks>
        public JsonWriterOptions? WriterOptions { get; set; }

        #endregion

        #region SerializerBase

        protected override TResult LoadImpl<TResult>(Stream stream)
        {
            var rawResult = JsonSerializer.Deserialize(stream, typeof(TResult), ReaderOptions ?? DefaultReaderOptions);
            if(rawResult is TResult result) {
                return result;
            }

            throw new SerializationException();
        }

        protected override void SaveImpl<TValue>(TValue value, Stream stream)
        {
            using(var writer = new Utf8JsonWriter(stream, WriterOptions ?? DefaultWriterOptions)) {
                JsonSerializer.Serialize(writer, value);
            }
        }

        #endregion
    }
}

