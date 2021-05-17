using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// 破棄時に内部参照を破棄するストリーム。
    /// </summary>
    public class ReferenceReleaseStream: Stream
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream">内部参照とするストリーム。</param>
        public ReferenceReleaseStream(Stream stream)
        {
            BaseStream = stream;
        }

        #region property

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

        #region function

        public byte[] GetBuffer() => ((MemoryStream)BaseStream).GetBuffer();
        #endregion

    }
}
