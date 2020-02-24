using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models
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
