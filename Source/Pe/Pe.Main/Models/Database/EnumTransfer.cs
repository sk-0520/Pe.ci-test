using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.Base;

namespace ContentTypeTextNet.Pe.Main.Models.Database
{
    /// <summary>
    /// <see cref="Dapper"/> で <see langword="enum" /> (の文字列)を扱えるように変換する。
    /// </summary>
    /// <remarks>
    /// <para>キャッシュとかは気が向けば。。。</para>
    /// </remarks>
    public class EnumTransfer<TEnum>
        where TEnum : struct, Enum
    {
        public EnumTransfer()
        {
            Debug.Assert(EnumType.IsEnum);
        }

        #region property

        private Type EnumType { get; } = typeof(TEnum);

        #endregion

        #region function

        /// <summary>
        /// <see langword="enum" /> 値をそれっぽい文字列に変換。
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public string ToString(TEnum member)
        {
            var fieldInfo = EnumType.GetField(member.ToString())!;

            var attribute = fieldInfo.GetCustomAttribute<EnumTransferAttribute>();
            if(attribute != null) {
                return attribute.Value;
            }

            var name = fieldInfo.Name;
            var nameConverter = new NameConverter();
            return nameConverter.PascalToKebab(name);
        }

        /// <summary>
        /// DB の値を <see langword="enum" /> 値に変換。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public TEnum ToEnum(string? value)
        {
            if(string.IsNullOrWhiteSpace(value)) {
                return default;
            }

            var fieldItem = EnumType.GetFields()
                .Select(f => new { Field = f, Attribute = f.GetCustomAttribute<EnumTransferAttribute>() })
                .Where(i => i.Attribute != null)
                .FirstOrDefault(i => i.Attribute!.Value == value)
            ;
            if(fieldItem != null) {
                foreach(var enumMember in Enum.GetValues(EnumType)) {
                    if(enumMember?.ToString() == fieldItem.Field.Name) {
                        return (TEnum)enumMember;
                    }
                }
            }


            var memberValue = value
                .Replace("-", "")
                .ToLowerInvariant()
            ;

            if(Enum.TryParse<TEnum>(memberValue, true, out var result)) {
                return result;
            }

            return default(TEnum);
        }

        #endregion
    }
}
