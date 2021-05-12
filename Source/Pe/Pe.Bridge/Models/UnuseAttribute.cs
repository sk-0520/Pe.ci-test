using System;

namespace ContentTypeTextNet.Pe.Bridge.Models
{
    /// <summary>
    /// ソース上で使用していない理由の種別。
    /// </summary>
    [Flags]
    public enum UnuseKinds
    {
        /// <summary>
        /// 知らん。
        /// </summary>
        Unknown,
        /// <summary>
        /// WPF で TwoWay しないと動かないのでしゃあなしセッターを公開しているやつ。
        /// <para>この属性がついている場合、publicからのアクセスは多分実装ミスとなる。</para>
        /// </summary>
        TwoWayBinding,
    }

    /// <summary>
    /// ソース上で使用していないことを明示。
    /// <para>諸々の都合により書かざるを得ないが使用していないものに対する説明書き。</para>
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class UnuseAttribute: Attribute
    {
        public UnuseAttribute(UnuseKinds unuseKinds)
        {
            Kinds = unuseKinds;
        }

        #region property

        /// <summary>
        /// ソース上で使用していない理由。
        /// </summary>
        public UnuseKinds Kinds { get; }

        #endregion
    }
}
