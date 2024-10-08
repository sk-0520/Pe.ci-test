using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Library.Base;


namespace ContentTypeTextNet.Pe.Core.Models.Serialization
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
        public Encoding Encoding { get; init; } = new UTF8Encoding(false);
        /// <summary>
        /// <see cref="Stream"/>で使用されるバッファサイズ。
        /// </summary>
        public int BufferSize { get; init; } = 4 * 1024;
        /// <summary>
        /// <see cref="Stream"/> 生成機。
        /// </summary>
        public Func<Stream>? InnerStreamFactory { get; init; }

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
        /// <see cref="Stream"/>から<see cref="TextReader"/>を生成。
        /// </summary>
        /// <remarks>
        /// <para><see cref="Encoding"/>, <see cref="BufferSize"/>が使用される。</para>
        /// </remarks>
        /// <param name="stream"></param>
        /// <returns><paramref name="stream"/>は閉じられない。</returns>
        protected TextReader CreateReader(Stream stream) => new StreamReader(stream, Encoding, true, BufferSize, true);
        /// <summary>
        /// <see cref="Stream"/>から<see cref="TextWriter"/>を生成。
        /// </summary>
        /// <remarks>
        /// <para><see cref="Encoding"/>, <see cref="BufferSize"/>が使用される。</para>
        /// </remarks>
        /// <param name="stream"></param>
        /// <returns><paramref name="stream"/>は閉じられない。</returns>
        protected TextWriter CreateWriter(Stream stream) => new StreamWriter(stream, Encoding, BufferSize, true);

        /// <summary>
        /// オブジェクトを複製する。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">複製したいオブジェクト。</param>
        /// <returns></returns>
        public T Clone<T>(T source)
            where T : notnull
        {
            using(var stream = CreateInnerStream()) {
                SaveImpl(source, stream);
                stream.Position = 0;
                return LoadImpl<T>(stream);
            }
        }

        /// <inheritdoc cref="Load{TResult}(Stream)"/>
        protected abstract TResult LoadImpl<TResult>(Stream stream);

        /// <summary>
        /// 復元処理。
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="stream">復元対象ストリーム。</param>
        /// <returns></returns>
        /// <exception cref="SerializationException"></exception>
        public TResult Load<TResult>(Stream stream)
        {
            try {
                return LoadImpl<TResult>(stream);
            } catch(Exception ex) when(ex is not SerializationException) {
                throw new SerializationException(ex.Message, ex);
            }
        }

        /// <inheritdoc cref="Save{TValue}(TValue, Stream)"/>
        protected abstract void SaveImpl<TValue>(TValue value, Stream stream)
            where TValue : notnull
        ;

        /// <summary>
        /// 保存処理。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="stream">保存先ストリーム。</param>
        /// <exception cref="SerializationException"></exception>
        public void Save<TValue>(TValue value, Stream stream)
            where TValue : notnull
        {
            try {
                SaveImpl(value, stream);
            } catch(Exception ex) when(ex is not SerializationException) {
                throw new SerializationException(ex.Message, ex);
            }
        }

        #endregion
    }
}

