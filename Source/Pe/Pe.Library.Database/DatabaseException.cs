using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Database
{
    [Serializable]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class DatabaseException: Exception
    {
        /// <inheritdoc cref="Exception()"/>
        public DatabaseException() { }
        /// <inheritdoc cref="Exception(string)"/>
        public DatabaseException(string message) : base(message) { }
        /// <inheritdoc cref="Exception(string, Exception)"/>
        public DatabaseException(string message, Exception inner) : base(message, inner) { }
        [Obsolete]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Info Code Smell", "S1133:Deprecated code should be removed")]
        protected DatabaseException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        { }
    }

    [Serializable]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class DatabaseStatementException: DatabaseException
    {
        /// <inheritdoc cref="DatabaseException()"/>
        public DatabaseStatementException()
        :base(){ }
        /// <inheritdoc cref="DatabaseException(string)"/>
        public DatabaseStatementException(string message) : base(message) { }
        /// <inheritdoc cref="DatabaseException(string, Exception)"/>
        public DatabaseStatementException(string message, Exception inner) : base(message, inner) { }
        [Obsolete]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Info Code Smell", "S1133:Deprecated code should be removed")]
        protected DatabaseStatementException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        { }
    }

    [Serializable]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class DatabaseManipulationException: DatabaseException
    {
        /// <inheritdoc cref="DatabaseException()"/>
        public DatabaseManipulationException() { }
        /// <inheritdoc cref="DatabaseException(string)"/>
        public DatabaseManipulationException(string message) : base(message) { }
        /// <inheritdoc cref="DatabaseException(string, Exception)"/>
        public DatabaseManipulationException(string message, Exception inner) : base(message, inner) { }
        [Obsolete]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Info Code Smell", "S1133:Deprecated code should be removed")]
        protected DatabaseManipulationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        { }
    }
}
