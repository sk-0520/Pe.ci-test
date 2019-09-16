using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models.Database
{
    /// <summary>
    /// enum の属性設定にて <see cref="EnumTransfer{TEnum}"/> を制御する。
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EnumTransferAttribute : Attribute
    {
        public EnumTransferAttribute(string value)
        {
            if(string.IsNullOrWhiteSpace(value)) {
                throw new ArgumentException(nameof(value));
            }

            Value = value;
        }

        #region property

        public string Value { get; }

        #endregion
    }

    /// <summary>
    /// <see cref="Dapper"/> で enum (の文字列)を扱えるように変換する。
    /// <para>キャッシュとかは気が向けば。。。</para>
    /// </summary>
    public class EnumTransfer<TEnum>
        where TEnum : struct /*, Enum*/
    {
        public EnumTransfer()
        {
            Debug.Assert(EnumType.IsEnum);
        }

        #region property

        Type EnumType { get; } = typeof(TEnum);

        #endregion

        #region function

        /// <summary>
        /// enum 値をそれっぽい文字列に変換。
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public string ToString(TEnum member)
        {
            //TODO: nullの可能性
            var fieldInfo = EnumType.GetField(member.ToString()!)!;

            var attribute = fieldInfo.GetCustomAttribute<EnumTransferAttribute>();
            if(attribute != null) {
                return attribute.Value;
            }

            var name = fieldInfo.Name;
            var builder = new StringBuilder((int)(name.Length * 1.5));
            var lastUpperIndex = -1;
            for(var i = 0; i < name.Length; i++) {
                var c = name[i];
                if(char.IsUpper(c)) {
                    if(c != 0 && lastUpperIndex != i - 1) {
                        builder.Append('-');
                    }
                    builder.Append(char.ToLower(c));
                    lastUpperIndex = i;
                } else {
                    builder.Append(char.ToLower(c));
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// DB の値を enum 値に変換。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public TEnum ToEnum(string value)
        {
            if(string.IsNullOrWhiteSpace(value)) {
                return default(TEnum);
            }

            var fieldItem = EnumType.GetFields()
                .Select(f => new { Field = f, Attribute = f.GetCustomAttribute<EnumTransferAttribute>() })
                .Where(i => i.Attribute != null)
                .FirstOrDefault(i => i.Attribute!.Value == value)
            ;
            if(fieldItem != null) {
                foreach(var enumMember in Enum.GetValues(EnumType)) {
                    //TODO: null の可能性
                    if(enumMember!.ToString() == fieldItem.Field.Name) {
                        return (TEnum)enumMember;
                    }
                }
            }


            var memberValue = value
                .Replace("-", "")
                .ToLower()
            ;

            if(Enum.TryParse<TEnum>(memberValue, true, out var result)) {
                return result;
            }

            return default(TEnum);
        }

        #endregion
    }
}
