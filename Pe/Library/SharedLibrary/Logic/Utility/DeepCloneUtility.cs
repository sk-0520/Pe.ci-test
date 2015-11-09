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
                .Where(m => m.GetCustomAttributes(typeof(IsDeepCloneAttribute)).Any())
                .Where(m => m.MemberType.HasFlag(MemberTypes.Field) || m.MemberType.HasFlag(MemberTypes.Property))
            ;
        }

        public static TDeepClone Copy<TDeepClone>(TDeepClone src)
            where TDeepClone:  IDeepClone, new()
        {
            var dst = new TDeepClone();

            var memberInfos = GetMembers(src);

            foreach(var memberInfo in memberInfos) {
                var srcValue = ReflectionUtility.GetMemberValue(memberInfo, src);
                var srcClone = srcValue as IDeepClone;
                if(srcClone != null) {
                    //var dstClone = (IDeepClone)property.GetValue(dst);
                    var dstClone = srcClone.DeepClone();
                    ReflectionUtility.SetMemberValue(memberInfo, ref dst, dstClone);
                } else {
                    ReflectionUtility.SetMemberValue(memberInfo, ref dst, srcValue);
                }
            }

            return dst;
        }

    }
}
