using System;
using System.Reflection;

namespace ContentTypeTextNet.Pe.Library.DependencyInjection
{
    /// <summary>
    /// 対象オブジェクトのプロパティに値を設定。
    /// </summary>
    public sealed class DiInjectionMember
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="baseType">対象オブジェクト。</param>
        /// <param name="memberInfo"><paramref name="baseType"/>の対象プロパティ。</param>
        /// <param name="objectType">設定する値。</param>
        /// <param name="name">名前。</param>
        public DiInjectionMember(Type baseType, MemberInfo memberInfo, Type objectType, string name)
        {
            BaseType = baseType;
            MemberInfo = memberInfo;
            ObjectType = objectType;
            Name = name;
        }

        #region property

        /// <summary>
        /// 対象オブジェクト。
        /// </summary>
        public Type BaseType { get; }
        /// <summary>
        /// <see cref="BaseType"/>の対象プロパティ。
        /// </summary>
        public MemberInfo MemberInfo { get; }
        /// <summary>
        /// 設定する値。
        /// </summary>
        public Type ObjectType { get; }

        /// <summary>
        /// 名前。
        /// </summary>
        public string Name { get; }

        #endregion
    }
}
