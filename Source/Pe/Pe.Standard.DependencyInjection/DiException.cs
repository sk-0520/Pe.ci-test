using System;

namespace ContentTypeTextNet.Pe.Standard.DependencyInjection
{
    /// <summary>
    /// DI処理でわっけ分からんことになったら投げられる例外。
    /// <para>内部的に <see cref="ArgumentException"/> 等を投げる場合はわざわざラップしないのでこの例外だけ受ければ良いという話ではない。</para>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly")]
    public sealed class DiException: ApplicationException
    {
        public DiException()
        { }

        public DiException(string? message)
            : base(message)
        { }

        public DiException(string? message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}
