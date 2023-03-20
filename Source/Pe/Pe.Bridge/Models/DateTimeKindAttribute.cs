using System;

namespace ContentTypeTextNet.Pe.Bridge.Models
{
    /// <summary>
    /// 対象時刻の種別を示す。
    /// <para>これをつけたからと何がどうなるわけでもないがソースに埋め込むことになるので流し見するときに楽な気がする。</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public sealed class DateTimeKindAttribute: Attribute
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="kind">割り当てる種別。</param>
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
