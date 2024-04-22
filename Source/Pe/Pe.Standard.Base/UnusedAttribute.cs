using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Standard.Base
{
    /// <summary>
    /// ソース上で使用していない理由の種別。
    /// </summary>
    [Flags]
    public enum UnusedKinds
    {
        /// <summary>
        /// 知らん。
        /// </summary>
        Unknown = 0b_0000,
        /// <summary>
        /// <see cref="IDisposable.Dispose"/> 時に null 設定する目的でセッターを公開。
        /// </summary>
        /// <remarks>
        /// <para>破棄処理(と初期化)以外で<c>set</c>が呼ばれている場合はバグってる。</para>
        /// </remarks>
        Dispose = 0b_0001,
        /// <summary>
        /// WPF で TwoWay しないと動かないのでしゃあなしセッターを公開しているやつ。
        /// </summary>
        /// <remarks>
        /// <para>この属性がついている場合、publicからのアクセスは多分実装ミスとなる。</para>
        /// </remarks>
        TwoWayBinding = 0b_0010,
    }

    /// <summary>
    /// ソース上で使用していないことを明示。
    /// </summary>
    /// <remarks>
    /// <para>諸々の都合により書かざるを得ないが使用していないものに対する説明書き。</para>
    /// </remarks>
    [System.AttributeUsage(System.AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public sealed class UnusedAttribute: Attribute
    {
        public UnusedAttribute(UnusedKinds unuseKinds)
        {
            Kinds = unuseKinds;
        }

        #region property

        /// <summary>
        /// ソース上で使用していない理由。
        /// </summary>
        public UnusedKinds Kinds { get; }

        #endregion
    }
}
