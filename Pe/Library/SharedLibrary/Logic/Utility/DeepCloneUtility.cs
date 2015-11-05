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
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using Attribute;
    using IF;

    public static class DeepCloneUtility
    {
        public static IEnumerable<MemberInfo> GetMembers(IDeepClone deepCone)
        {
            return deepCone.GetType().GetMembers(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(p => p.GetCustomAttributes(typeof(IsDeepCloneAttribute)).Any())
            ;
        }

        public static void DeepCopy(IDeepClone dst, IDeepClone src)
        {
            if(dst.GetType() != src.GetType()) {
                throw new ArgumentException(string.Format("dst[{0}] != src[{1}]", dst.GetType(), src.GetType()));
            }

            var properties = GetMembers(src);
            
            foreach(var property in properties.OfType<FieldInfo>()) {
                var srcValue = property.GetValue(src);
                var srcClone = srcValue as IDeepClone;
                if(srcClone != null) {
                    var dstClone = (IDeepClone)property.GetValue(dst);
                    srcClone.DeepCloneTo(dstClone);
                    if(dstClone.GetType().IsValueType) {
                        // 構造体は値の再設定が必要(多分)
                        property.SetValue(dst, dstClone);
                    }
                } else {
                    property.SetValue(dst, srcValue);
                }
            }
        }

    }
}
