using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public class UniqueKeyPool
    {
        public UniqueKeyPool()
        { }

        #region property

        ConcurrentDictionary<string, object> Pool { get; } = new ConcurrentDictionary<string, object>();

        #endregion

        #region function

        public object Get([CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            var sb = new StringBuilder(callerMemberName.Length + 1 + callerLineNumber);
            sb.Append(callerMemberName);
            sb.Append('.');
            sb.Append(callerLineNumber);

            var result = Pool.GetOrAdd(sb.ToString(), k => new object());
            return result;
        }

        #endregion
    }
}
