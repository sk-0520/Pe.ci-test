/**
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
namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;
    using ContentTypeTextNet.Library.SharedLibrary.IF;

    public static class ReflectionUtility
    {
        public static IEnumerable<PropertyInfo> FilterSharedLibrary(IEnumerable<PropertyInfo> propertyInfos)
        {
            var filter = new[] {
                "PropertyInfos",
                "DisplayText",
            };
            return propertyInfos
                .Where(pi => !filter.Any(s => s == pi.Name))
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
        /// <param name="getMembers"></param>
        /// <returns></returns>
        public static string GetObjectString(IGetMembers getMembers)
        {
            var name = getMembers.GetType().Name;
            var nameValueStrings = getMembers.GetNameValueList();
            var joinString = ReflectionUtility.JoinNameValueStrings(nameValueStrings);

            return string.Format("{0}=>{1}", name, joinString);
        }

        public static IEnumerable<MemberInfo> GetSerializeMembers(object obj)
        {
            var type = obj.GetType();
            var members = type
                .GetMembers(BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.GetField | BindingFlags.SetField)
                .Where(m => m.CustomAttributes.Any(c => {
                    return c.AttributeType.GetCustomAttributes(typeof(DataContractAttribute), true).Any();
                })
                )
            ;

            return members;
        }
    }
}
