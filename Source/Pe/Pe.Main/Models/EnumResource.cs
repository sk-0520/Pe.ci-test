using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace ContentTypeTextNet.Pe.Main.Models
{
    [System.AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    public sealed class EnumResourceAttribute: Attribute
    {
        public EnumResourceAttribute()
        {
            ResourceBaseName = string.Empty;
        }

        public EnumResourceAttribute(string resourceBaseName)
        {
            ResourceBaseName = resourceBaseName;
        }
        public EnumResourceAttribute(Type enumType, string memberName)
        {
            ResourceBaseName = enumType.FullName + "." + memberName;
        }

        public string ResourceBaseName { get; set; }
    }

    public readonly struct EnumResource
    {
        // uintとか long とか来ないことを信じて...
        public EnumResource(int rawMember, string resourceName)
        {
            RawMember = rawMember;
            ResourceName = resourceName;
        }

        #region property

        public int RawMember { get; }
        public string ResourceName { get; }

        #endregion
    }

    public class EnumResourceMapping
    {
        public EnumResourceMapping(Type enumType, IReadOnlyList<EnumResource> enumResources)
        {
            EnumType = enumType;
            Items = enumResources;
        }

        #region property

        public Type EnumType { get; }
        public IReadOnlyList<EnumResource> Items { get; }

        #endregion
    }

    public enum ResourceNameKind
    {
        Normal,
        AccessKey,
    }

    /// <summary>
    /// Enumのリソース名を管理。
    /// </summary>
    /// <remarks>
    /// <para>なにをどうしても <see cref="NameHeader"/> + <see cref="Separator"/> が先頭にくっつくことに注意。</para>
    /// </remarks>
    public class EnumResourceManager
    {
        #region property

        private IDictionary<Type, EnumResourceMapping> Map { get; } = new Dictionary<Type, EnumResourceMapping>();

        public string NameHeader { get; } = "String:Enum";
        public string Separator { get; } = "-";

        #endregion

        #region function

        /// <summary>
        /// 名前空間.Enum.メンバ名で登録
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        public EnumResourceManager Register<TEnum>()
            where TEnum : Enum
        {
            var type = typeof(TEnum);
            var memberValues = Enum.GetValues(type);
            var enumResources = new List<EnumResource>(memberValues.Length);
            foreach(var memberValue in memberValues) {
                var fieldInfo = type.GetField(memberValue!.ToString()!)!;
                //var attribute = fieldInfo.GetCustomAttribute<EnumResourceAttribute>();
                enumResources.Add(new EnumResource((int)memberValue, type.FullName + "." + fieldInfo.Name));
            }

            Map.Add(type, new EnumResourceMapping(type, enumResources));

            return this;
        }

        /// <summary>
        /// 独自の名前で登録。
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumResources"></param>
        public EnumResourceManager Register<TEnum>(IReadOnlyList<EnumResource> enumResources)
            where TEnum : Enum
        {
            var type = typeof(TEnum);
            Map.Add(type, new EnumResourceMapping(type, enumResources));

            return this;
        }

        private string GetResourceName(EnumResourceMapping mapping, Type enumType, object enumValue)
        {
            var item = mapping.Items.FirstOrDefault(i => i.RawMember == (int)enumValue);
            if(item.ResourceName == string.Empty) {
                return string.Empty;
            }
            return item.ResourceName;
        }

        private string GetResourceName(Type enumType, object enumValue)
        {
            var fieldInfo = enumType.GetField(enumValue.ToString()!)!;
            var attribute = fieldInfo.GetCustomAttribute<EnumResourceAttribute>();
            if(attribute == null) {
                throw new ArgumentException(null, nameof(enumType));
            }

            //TODO: resourceNameKind
            var baseName = string.IsNullOrWhiteSpace(attribute.ResourceBaseName)
                ? enumType.FullName + "." + fieldInfo.Name
                : attribute.ResourceBaseName
            ;
            return baseName;
        }

        public string GetString(object enumValue) => GetString(enumValue, ResourceNameKind.Normal, false);

        public string GetString(object enumValue, ResourceNameKind resourceNameKind, bool undefinedIsRaw)
        {
            var type = enumValue.GetType();
            if(type == typeof(string)) {
                return string.Empty;
            }

            var resourceBaseName = Map.TryGetValue(type, out var val)
                ? GetResourceName(val, type, enumValue)
                : GetResourceName(type, enumValue)
            ;
            if(resourceBaseName == string.Empty) {
                if(undefinedIsRaw) {
                    return enumValue.ToString() ?? string.Empty;
                }
                return string.Empty;
            }

            var resourceName = resourceNameKind switch {
                ResourceNameKind.AccessKey => resourceBaseName + "_A",
                ResourceNameKind.Normal => resourceBaseName,
                _ => throw new NotImplementedException()
            };

            var result = Properties.Resources.ResourceManager.GetString(NameHeader + Separator + resourceName, CultureInfo.InvariantCulture);
            if(result != null) {
                return result;
            }
            if(undefinedIsRaw) {
                return enumValue.ToString() ?? string.Empty;
            }

            return string.Empty;
        }

        #endregion
    }
}
