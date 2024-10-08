using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Base
{
    /// <summary>
    /// <see cref="Stream"/>をラップするストリーム。
    /// </summary>
    /// <remarks>
    /// <para>解放処理は継承先で対応すること。</para>
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1710:識別子は、正しいサフィックスを含んでいなければなりません")]
    public abstract class WrapStreamBase: Stream
    {
        protected WrapStreamBase(Stream stream)
        {
            BaseStream = stream;
        }

        #region property

        /// <summary>
        /// ラップしているストリーム。
        /// </summary>
        protected Stream BaseStream { get; set; }

        #endregion

        #region Stream

        public override bool CanRead => BaseStream.CanRead;

        public override bool CanSeek => BaseStream.CanSeek;

        public override bool CanWrite => BaseStream.CanWrite;

        public override long Length => BaseStream.Length;

        public override long Position
        {
            get { return BaseStream.Position; }
            set { BaseStream.Position = value; }
        }

        public override void Flush() => BaseStream.Flush();

        public override int Read(byte[] buffer, int offset, int count) => BaseStream.Read(buffer, offset, count);

        public override long Seek(long offset, SeekOrigin origin) => BaseStream.Seek(offset, origin);

        public override void SetLength(long value) => BaseStream.SetLength(value);

        public override void Write(byte[] buffer, int offset, int count) => BaseStream.Write(buffer, offset, count);

        #endregion
    }

    /// <summary>
    /// 渡されたストリームを閉じないストリーム。
    /// </summary>
    /// <remarks>
    /// <para>他のストリーム使用処理へ渡した後 閉じられると困る場合にこいつをかませて閉じないようにすることが目的。</para>
    /// <para>用途が用途なので <see cref="Dispose"/> しても <see cref="WrapStreamBase.BaseStream"/> は何もケアされない、つまりはひらきっぱなことに注意。</para>
    /// </remarks>
    public sealed class KeepStream: WrapStreamBase
    {
        /// <summary>
        /// <inheritdoc cref="KeepStream(Stream, bool)"/>
        /// <para>位置も戻る。</para>
        /// </summary>
        /// <param name="stream"><inheritdoc cref="KeepStream(Stream, bool)"/></param>
        public KeepStream(Stream stream)
            : this(stream, true)
        { }

        /// <summary>
        /// ストリームを閉じないストリーム。
        /// </summary>
        /// <param name="stream">閉じたくないストリーム。</param>
        /// <param name="keepPosition"><see cref="Stream.Dispose"/> 時に <see cref="Stream.Position"/> を初期状態に戻すか。</param>
        public KeepStream(Stream stream, bool keepPosition)
            : base(stream)
        {
            KeepPosition = keepPosition;
            if(KeepPosition) {
                RestorePosition = BaseStream.Position;
            }
        }

        #region property

        /// <summary>
        /// <see cref="Stream.Dispose"/> 時に <see cref="Stream.Position"/> を初期状態に戻すか。
        /// </summary>
        private bool KeepPosition { get; }
        /// <summary>
        /// <see cref="Stream.Dispose"/> 時に移動させるストリーム位置。
        /// </summary>
        /// <remarks>
        /// <para><see cref="KeepPosition"/>が真の場合に有効値が設定される。</para>
        /// </remarks>
        private long RestorePosition { get; }

        #endregion

        #region WrapStreamBase

        protected override void Dispose(bool disposing)
        {
            if(KeepPosition) {
                if(BaseStream != null) {
                    BaseStream.Position = RestorePosition;
                }
            }
            BaseStream = null!;

            base.Dispose(disposing);
        }

        #endregion
    }

    /// <summary>
    /// 破棄時に内部参照を破棄するストリーム。
    /// </summary>
    public class ReferenceReleaseStream: WrapStreamBase
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="stream">内部参照とするストリーム。</param>
        public ReferenceReleaseStream(Stream stream)
            : base(stream)
        { }

        #region WrapStreamBase

        protected override void Dispose(bool disposing)
        {
            if(disposing && BaseStream is not null) {
                BaseStream.Dispose();
                BaseStream = null!;
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    /// <summary>
    /// 内部で<see cref="MemoryStream"/>を使用する<see cref="ReferenceReleaseStream"/>。
    /// </summary>
    public sealed class MemoryReleaseStream: ReferenceReleaseStream
    {
        public MemoryReleaseStream()
            : base(new MemoryStream())
        { }

        public MemoryReleaseStream(byte[] buffer)
            : base(new MemoryStream(buffer))
        { }

        public MemoryReleaseStream(int capacity)
            : base(new MemoryStream(capacity))
        { }

        public MemoryReleaseStream(byte[] buffer, int index, int count)
            : base(new MemoryStream(buffer, index, count))
        { }

        #region function

        public byte[] GetBuffer() => ((MemoryStream)BaseStream).GetBuffer();
        public byte[] ToArray() => ((MemoryStream)BaseStream).ToArray();

        #endregion
    }
}
