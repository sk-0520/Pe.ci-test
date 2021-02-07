using System;

namespace ContentTypeTextNet.Pe.Bridge.Models
{
    [Flags]
    public enum UnuseKinds
    {
        Unknown,
        /// <summary>
        /// WPF で TwoWay しないと動かないのでしゃあなしセッターを公開しているやつ。
        /// <para>この属性がついている場合、publicからのアクセスは多分実装ミスとなる。</para>
        /// </summary>
        TwoWayBinding,
    }

    [System.AttributeUsage(System.AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class UnuseAttribute: Attribute
    {
        public UnuseAttribute(UnuseKinds unuseKinds)
        {
            Kinds = unuseKinds;
        }

        #region property

        public UnuseKinds Kinds { get; }

        #endregion
    }
}
