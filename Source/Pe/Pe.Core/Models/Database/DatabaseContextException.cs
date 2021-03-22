using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Core.Models.Database
{
    [Serializable]
    public class DatabaseContextException: Exception
    {
        public DatabaseContextException() { }
        public DatabaseContextException(string message) : base(message) { }
        public DatabaseContextException(string message, Exception inner) : base(message, inner) { }
        protected DatabaseContextException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        { }
    }
}
