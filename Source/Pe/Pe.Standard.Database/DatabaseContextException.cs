using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Standard.Database
{
    [Serializable]
    public class DatabaseContextException: Exception
    {
        /// <inheritdoc cref="Exception()"/>
        public DatabaseContextException() { }
        /// <inheritdoc cref="Exception(string)"/>
        public DatabaseContextException(string message) : base(message) { }
        /// <inheritdoc cref="Exception(string, Exception)"/>
        public DatabaseContextException(string message, Exception inner) : base(message, inner) { }
        protected DatabaseContextException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        { }
    }
}
