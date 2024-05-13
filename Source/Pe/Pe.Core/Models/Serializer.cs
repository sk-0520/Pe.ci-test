#define ENABLED_NETCoreJSON

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using ContentTypeTextNet.Pe.Standard.Base;

#if ENABLED_NETCoreJSON
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
#endif

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// シリアライズ・デシリアライズ処理を統括。
    /// </summary>
    public abstract class SerializerBase
    {
        #region property

        /// <summary>
        /// 文字列の場合のエンコーディング。
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8; // new UTF8Encoding(false);
        /// <summary>
        /// バッファサイズ。
        /// </summary>
        public int BufferSize { get; set; } = 4 * 1024;
        /// <summary>
        /// <see cref="Stream"/> 生成機。
        /// </summary>
        public Func<Stream>? InnerStreamFactory { get; set; }

        #endregion

        #region function

        /// <summary>
        /// 内部ストリーム生成処理。
        /// </summary>
        /// <remarks>
        /// <para><see cref="InnerStreamFactory"/>を使用するが、未設定時は<see cref="MemoryStream"/>と<see cref="BufferSize"/>が使用される。</para>
        /// </remarks>
        /// <returns></returns>
        protected Stream CreateInnerStream() => InnerStreamFactory?.Invoke() ?? new MemoryReleaseStream(BufferSize);
        /// <summary>
        /// ストリームからリーダーを取得。
        /// </summary>
        /// <remarks>
        /// <para><see cref="Encoding"/>, <see cref="BufferSize"/>が使用される。</para>
        /// </remarks>
        /// <param name="stream"></param>
        /// <returns></returns>
        protected TextReader GetReader(Stream stream) => new StreamReader(stream, Encoding, true, BufferSize, true);
        /// <summary>
        /// ストリームからライターを取得。
        /// </summary>
        /// <remarks>
        /// <para><see cref="Encoding"/>, <see cref="BufferSize"/>が使用される。</para>
        /// </remarks>
        /// <param name="stream"></param>
        /// <returns></returns>
        protected TextWriter GetWriter(Stream stream) => new StreamWriter(stream, Encoding, BufferSize, true);

        /// <summary>
        /// オブジェクトを複製する。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">複製したいオブジェクト。</param>
        /// <returns></returns>
        public T Clone<T>(object source)
        {
            if(source == null) {
                throw new ArgumentNullException(nameof(source));
            }

            using(var stream = CreateInnerStream()) {
                Save(source, stream);
                stream.Position = 0;
                return Load<T>(stream);
            }
        }


        /// <summary>
        /// オブジェクトを複製する。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">複製したいオブジェクト。</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "HAA0601:Value type to reference type conversion causing boxing allocation")]
        public TResult Clone<TResult, TSource>(TSource source)
        {
            if(source == null) {
                throw new ArgumentNullException(nameof(source));
            }

            using(var stream = CreateInnerStream()) {
                Save(source, stream);
                stream.Position = 0;
                return Load<TResult>(stream);
            }
        }

        /// <summary>
        /// 復元処理。
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="stream">復元対象ストリーム。</param>
        /// <returns></returns>
        /// <exception cref="SerializationException"></exception>
        public abstract TResult Load<TResult>(Stream stream);
        /// <summary>
        /// 保存処理。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="stream">保存先ストリーム。</param>
        public abstract void Save(object value, Stream stream);

        #endregion
    }

#if ENABLED_JsonNet
    public class JsonNetSerializer : SerializerBase
    {
    #region SerializerBase

        public override TResult Load<TResult>(Stream stream)
        {
            using(var reader = GetReader(stream))
            using(var jsonReader = new Newtonsoft.Json.JsonTextReader(reader)) {
                var serializer = new Newtonsoft.Json.JsonSerializer();
                return serializer.Deserialize<TResult>(jsonReader);
            }
        }

        public override void Save(object value, Stream stream)
        {
            using(var writer = GetWriter(stream))
            using(var jsonWriter = new Newtonsoft.Json.JsonTextWriter(writer)) {
                var serializer = new Newtonsoft.Json.JsonSerializer();
                serializer.Serialize(jsonWriter, value);
            }
        }

    #endregion
    }
#endif

#if ENABLED_NETCoreJSON

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

        public override TResult Load<TResult>(Stream stream)
        {
            var buffer = new byte[stream.Length];
            stream.Read(buffer);
            var reader = new Utf8JsonReader(buffer, ReaderOptions);
            var rawResult = JsonSerializer.Deserialize<TResult>(ref reader);
            if(rawResult is TResult result) {
                return result;
            }

            throw new SerializationException();
        }

        public override void Save(object value, Stream stream)
        {
            using(var writer = new Utf8JsonWriter(stream, WriterOptions)) {
                JsonSerializer.Serialize(writer, value);
            }
        }

        #endregion
    }

#endif

    /// <summary>
    /// <see cref="DataContractJsonSerializer"/>を用いたシリアライズ・デシリアライズ処理。
    /// </summary>
    public class JsonDataSerializer: SerializerBase
    {
        #region SerializerBase

        public override TResult Load<TResult>(Stream stream)
        {
            using(var reader = GetReader(stream)) {
                var serializer = new DataContractJsonSerializer(typeof(TResult));
                var rawResult = serializer.ReadObject(stream);

                if(rawResult is TResult result) {
                    return result;
                }

                throw new SerializationException();
            }
        }

        public override void Save(object value, Stream stream)
        {
            using(var writer = GetWriter(stream)) {
                var serializer = new DataContractJsonSerializer(value.GetType());
                serializer.WriteObject(stream, value);
            }
        }

        #endregion
    }

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

    /// <summary>
    /// <see cref="System.Xml.Serialization.XmlSerializer"/>を用いたシリアライズ・デシリアライズ処理。
    /// </summary>
    public class XmlSerializer: XmlSerializerBase
    {
        #region XmlSerializerBase

        public override TResult Load<TResult>(Stream stream)
        {
            using(var reader = XmlReader.Create(stream, CreateXmlReaderSettings())) {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(TResult));
                var rawResult = serializer.Deserialize(stream);

                if(rawResult is TResult result) {
                    return result;
                }

                throw new SerializationException();
            }
        }

        public override void Save(object value, Stream stream)
        {
            using(var writer = XmlWriter.Create(stream, CreateXmlWriterSettings())) {
                var serializer = new System.Xml.Serialization.XmlSerializer(value.GetType());
                serializer.Serialize(writer, value);
            }
        }

        #endregion
    }

    public abstract class DataContractSerializerBase: XmlSerializerBase
    { }

    /// <summary>
    /// <see cref="DataContractSerializer"/>を用いたXMLシリアライズ・デシリアライズ処理。
    /// </summary>
    public class XmlDataContractSerializer: DataContractSerializerBase
    {
        #region DataContractSerializerBase

        public override TResult Load<TResult>(Stream stream)
        {
            using(var reader = XmlReader.Create(stream, CreateXmlReaderSettings())) {
                var serializer = new DataContractSerializer(typeof(TResult));
                var rawResult = serializer.ReadObject(stream);

                if(rawResult is TResult result) {
                    return result;
                }

                throw new SerializationException();
            }
        }

        public override void Save(object value, Stream stream)
        {
            using(var writer = XmlWriter.Create(stream, CreateXmlWriterSettings())) {
                var serializer = new DataContractSerializer(value.GetType());
                serializer.WriteObject(writer, value);
            }
        }

        #endregion
    }

    /// <summary>
    /// <see cref="DataContractSerializer"/>を用いたバイナリシリアライズ・デシリアライズ処理。
    /// </summary>
    public class BinaryDataContractSerializer: DataContractSerializerBase
    {
        #region DataContractSerializerBase

        public override TResult Load<TResult>(Stream stream)
        {
            // 閉じない方法がわっからん
            var quotas = new XmlDictionaryReaderQuotas();
            using(var reader = XmlDictionaryReader.CreateBinaryReader(stream, quotas)) {
                var serializer = new DataContractSerializer(typeof(TResult));
                var rawResult = serializer.ReadObject(stream);

                if(rawResult is TResult result) {
                    return result;
                }

                throw new SerializationException();
            }
        }

        public override void Save(object value, Stream stream)
        {
            using(var writer = XmlDictionaryWriter.CreateBinaryWriter(stream, null, null, false)) {
                var serializer = new DataContractSerializer(value.GetType());
                serializer.WriteObject(writer, value);
            }
        }

        #endregion
    }

    /// <summary>
    /// シリアライズ・デシリアライズ処理。
    /// </summary>
    public static class SerializeUtility
    {
        #region property

        public static Func<SerializerBase> SerializerCreator { get; set; } = () => new BinaryDataContractSerializer();

        #endregion

        #region function

        public static TResult Clone<TResult>(object value)
        {
            if(!(value is TResult)) {
                throw new ArgumentException($"cast error: {nameof(value)} is not ${typeof(TResult).FullName}");
            }

            var serializer = SerializerCreator();
            return serializer.Clone<TResult>(value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "HAA0601:Value type to reference type conversion causing boxing allocation")]
        public static TResult Clone<TResult>(TResult value)
            where TResult : new()
        {
            if(!(value is TResult)) {
                throw new ArgumentException($"cast error: {nameof(value)} is not ${typeof(TResult).FullName}");
            }

            var serializer = SerializerCreator();
            return serializer.Clone<TResult>(value);
        }

        #endregion
    }
}

