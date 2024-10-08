using System;

namespace ContentTypeTextNet.Pe.Library.DependencyInjection
{
    /// <summary>
    /// DI処理でわっけ分からんことになったら投げられる例外。
    /// </summary>
    /// <remarks>
    /// <para>内部的に <see cref="ArgumentException"/> 等を投げる場合はわざわざラップしないのでこの例外だけ受ければ良いという話ではない。</para>
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class DiException: Exception
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

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class DiFunctionMethodNotFoundException: DiException
    {
        public DiFunctionMethodNotFoundException()
        { }

        public DiFunctionMethodNotFoundException(string? message)
            : base(message)
        { }

        public DiFunctionMethodNotFoundException(string? message, Exception? innerException)
            : base(message, innerException)
        { }
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class DiFunctionResultException: DiException
    {
        public DiFunctionResultException()
        { }

        public DiFunctionResultException(string? message)
            : base(message)
        { }

        public DiFunctionResultException(string? message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}
