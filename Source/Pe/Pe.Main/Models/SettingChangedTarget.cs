using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ContentTypeTextNet.Pe.Main.Models
{
    /// <summary>
    /// 設定変更を受け付けるメンバに対して付与する属性。
    /// </summary>
    /// <remarks>
    /// <para><see cref="EventHandler{NotifyEventArgs}"/>で使用する前提。</para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class SettingChangedTargetAttribute: Attribute
    { }

    public static class SettingChangedTargetHelper
    {
        #region function

        /// <summary>
        /// <see cref="SettingChangedTargetAttribute"/>の付与されたメンバーを取得する。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<MemberInfo> GetMembers(Type type)
        {
            var flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.GetProperty;
            var members = type.GetMembers(flags)
                .Where(i => i.GetCustomAttribute<SettingChangedTargetAttribute>() is not null)
            ;

            return members;
        }

        #endregion
    }
}
