using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models.DependencyInjection
{
    public sealed class DiDirtyMember
    {
        public DiDirtyMember(Type baseType, MemberInfo memberInfo, Type objectType)
        {
            BaseType = baseType;
            MemberInfo = memberInfo;
            ObjectType = objectType;
        }

        #region property

        public Type BaseType { get; }
        public MemberInfo MemberInfo { get; }
        public Type ObjectType { get; }

        #endregion
    }
}
