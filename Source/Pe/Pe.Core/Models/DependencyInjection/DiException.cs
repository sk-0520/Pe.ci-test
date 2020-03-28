using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models.DependencyInjection
{
    /// <summary>
    /// DI処理でわっけ分からんことになったら投げられる例外。
    /// <para><see cref="ArgumentException"/>等の分かっているのはその例外を投げるのでこの例外だけ受ければ良いという話ではない。</para>
    /// </summary>
    public class DiException : ApplicationException
    {
        public DiException()
        { }

        public DiException(string? message)
            : base(message)
        { }

        public DiException(string? message, Exception? innerException)
            : base(message, innerException)
        { }

        protected DiException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
