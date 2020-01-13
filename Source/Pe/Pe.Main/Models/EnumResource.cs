using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Main.Models
{
    [System.AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    public sealed class EnumResourceAttribute : Attribute
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
        //TODO: uintとかlongとか来ないことを信じて...
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

    public class EnumResourceManager
    {
        #region property

        IDictionary<Type, EnumResourceMapping> Map { get; } = new Dictionary<Type, EnumResourceMapping>();

        string NameHeader { get; } = "String_Enum";
        string Separator { get; } = ":";

        #endregion

        #region function

        public void Add<TEnum>(IReadOnlyList<EnumResource> enumResources)
            where TEnum: Enum
        {
            var type = typeof(TEnum);
            Map.Add(type, new EnumResourceMapping(type, enumResources));
        }

        public string GetString(object enumValue) => GetString(enumValue, ResourceNameKind.Normal);

        public string GetString(object enumValue, ResourceNameKind resourceNameKind)
        {
            var type = enumValue.GetType();
            if(Map.TryGetValue(type, out var val)) {
            }

            var fieldInfo = type.GetField(enumValue.ToString()!)!;
            var attribute = fieldInfo.GetCustomAttribute<EnumResourceAttribute>();
            if(attribute == null) {
                throw new ArgumentException(nameof(type));
            }

            //TODO: resourceNameKind
            var baseName = string.IsNullOrWhiteSpace(attribute.ResourceBaseName)
                ? type.FullName + "." + fieldInfo.Name
                : attribute.ResourceBaseName
            ;
            var name = NameHeader + Separator + baseName;

            var result = Properties.Resources.ResourceManager.GetString(name);
            return result ?? string.Empty;
        }

        #endregion
    }
}
