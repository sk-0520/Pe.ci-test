using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
{
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class TimestampAttribute : Attribute
    {
        public TimestampAttribute(DateTimeKind kind)
        {
            DateTimeKind = kind;
        }

        #region property

        public DateTimeKind DateTimeKind { get; }

        #endregion
    }
}
