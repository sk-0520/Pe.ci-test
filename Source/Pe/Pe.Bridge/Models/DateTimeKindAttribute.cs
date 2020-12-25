using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Bridge.Models
{
    /// <summary>
    /// 対象時刻の種別を示す。
    /// <para>これをつけたからと何がどうなるわけでもないがソースに埋め込むことになるので流し見するときに楽な気がする。</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class DateTimeKindAttribute: Attribute
    {
        public DateTimeKindAttribute(DateTimeKind kind)
        {
            DateTimeKind = kind;
        }

        #region property

        /// <inheritdoc cref="System.DateTimeKind"/>
        public DateTimeKind DateTimeKind { get; }

        #endregion
    }
}
