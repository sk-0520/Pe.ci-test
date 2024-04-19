using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Theme
{
    /// <summary>
    /// テーマ対象となるプロパティに設定。
    /// </summary>
    /// <remarks>
    /// <para><see cref="ContentTypeTextNet.Pe.Bridge.Models.IPlatformTheme.Changed"/>の追従で手書きはしんどい。</para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class ThemePropertyAttribute: Attribute
    {
        public ThemePropertyAttribute()
        { }
    }

    /// <summary>
    /// <see cref="ThemePropertyAttribute"/>の設定されたプロパティを保持。
    /// </summary>
    public sealed class ThemeProperties
    {
        public ThemeProperties(object obj)
        {
            Properties = obj.GetType()
                .GetProperties()
                .Select(i => new { Property = i, Attribute = i.GetCustomAttribute<ThemePropertyAttribute>() })
                .Where(i => i.Attribute != null)
                .Select(i => i.Property)
                .ToList()
            ;
        }

        #region property

        public IReadOnlyList<PropertyInfo> Properties { get; }

        #endregion

        public IEnumerable<string> GetPropertyNames()
        {
            return Properties.Select(i => i.Name);
        }
    }
}
