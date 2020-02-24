/*
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.IF;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
    public static class ReflectionUtility
    {
        public static IEnumerable<PropertyInfo> FilterSharedLibrary(IEnumerable<PropertyInfo> propertyInfos)
        {
            var filter = new[] {
                nameof(IDisplayText.DisplayText),
            };
            return propertyInfos
                .Where(pi => !filter.Any(s => s == pi.Name))
                .Where(pi => {
                    var attr = pi.GetCustomAttributes(typeof(IgnoreDisplayTextAttribute), true);
                    return attr == null || attr.Length == 0;
                })
            ;
        }

        /// <summary>
        /// メンバ名とその値を取得する。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyInfos"></param>
        /// <returns></returns>
        public static IDictionary<string, object> GetMembers(object obj, IEnumerable<PropertyInfo> propertyInfos)
        {
            var result = new Dictionary<string, object>(propertyInfos.Count());

            foreach(var propertyInfo in propertyInfos) {
                var value = propertyInfo.GetValue(obj, null);
                result.Add(propertyInfo.Name, value);
            }

            return result;
        }

        /// <summary>
        /// メンバ名と値を結合してそのリストを取得する。
        /// </summary>
        /// <param name="nameValues"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetNameValueStrings(IDictionary<string, object> nameValues)
        {
            return nameValues
                .OrderBy(p => p.Key)
                .Select(pair => string.Format("{0}=[{1}]", pair.Key, pair.Value ?? "null"))
            ;
        }

        /// <summary>
        /// メンバ名と値を結合したリストを一つの文字列にする。
        /// </summary>
        /// <param name="nameValues"></param>
        /// <returns></returns>
        public static string JoinNameValueStrings(IEnumerable<string> nameValues)
        {
            return string.Join(",", nameValues);
        }

        /// <summary>
        /// メンバ名と値を保持するオブジェクトを文字列にする。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetObjectString(object obj)
        {
            var name = obj.GetType().Name;
            var propertyInfos = FilterSharedLibrary(obj.GetType().GetProperties());
            var nameValueMap = GetMembers(obj, propertyInfos);
            var nameValueStrings = GetNameValueStrings(nameValueMap);
            var joinString = JoinNameValueStrings(nameValueStrings);

            return string.Format("{0}=>{1}", name, joinString);
        }

        public static object GetMemberValue(FieldInfo fieldInfo, object obj)
        {
            return fieldInfo.GetValue(obj);
        }
        public static object GetMemberValue(PropertyInfo propertyInfo, object obj)
        {
            return propertyInfo.GetValue(obj);
        }
        public static object GetMemberValue(MemberInfo memberInfo, object obj)
        {
            if(memberInfo.MemberType.HasFlag(MemberTypes.Property)) {
                return GetMemberValue((PropertyInfo)memberInfo, obj);
            } else if(memberInfo.MemberType.HasFlag(MemberTypes.Field)) {
                return GetMemberValue((FieldInfo)memberInfo, obj);
            } else {
                throw new NotImplementedException();
            }
        }

        public static void SetMemberValue<T>(FieldInfo fieldInfo, ref T obj, object value)
        {
            if(obj.GetType().IsValueType) {
                fieldInfo.SetValueDirect(__makeref(obj), value);
            } else {
                fieldInfo.SetValue(obj, value);
            }
        }
        public static void SetMemberValue<T>(PropertyInfo propertyInfo, ref T obj, object value)
        {
            if(obj.GetType().IsValueType) {
                var box = RuntimeHelpers.GetObjectValue(obj);
                propertyInfo.SetValue(box, value);
                obj = (T)box;
            } else {
                propertyInfo.SetValue(obj, value);
            }
        }
        public static void SetMemberValue<T>(MemberInfo memberInfo, ref T obj, object value)
        {
            if(memberInfo.MemberType.HasFlag(MemberTypes.Property)) {
                SetMemberValue((PropertyInfo)memberInfo, ref obj, value);
            } else if(memberInfo.MemberType.HasFlag(MemberTypes.Field)) {
                SetMemberValue((FieldInfo)memberInfo, ref obj, value);
            } else {
                throw new NotImplementedException();
            }
        }
    }
}
