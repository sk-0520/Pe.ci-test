using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Bridge.Models
{
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class DateTimeKindAttribute: Attribute
    {
        public DateTimeKindAttribute(DateTimeKind kind)
        {
            DateTimeKind = kind;
        }

        #region property

        public DateTimeKind DateTimeKind { get; }

        #endregion
    }
}
