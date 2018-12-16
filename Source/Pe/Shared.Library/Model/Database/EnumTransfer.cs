using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database
{
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
    /// Dapper で enum (の文字列)を扱えるように変換する。
    /// </summary>
    public class EnumTransfer<TEnumMember>
        where TEnumMember : struct /*, Enum*/
    {
        #region property

        #endregion

        #region function

        public string To(TEnumMember member)
        {
            var enumType = typeof(TEnumMember);
            Debug.Assert(enumType.IsEnum);

            var fieldInfo = enumType.GetField(member.ToString());

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

        #endregion
    }
}
