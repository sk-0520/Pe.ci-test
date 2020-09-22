using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class ConfigurationAttribute: Attribute
    {
        public ConfigurationAttribute()
            : this(string.Empty)
        { }

        public ConfigurationAttribute(string memberName)
        {
            MemberName = memberName;
        }

        #region proeprty

        /// <summary>
        /// アプリケーション構成ファイルでのメンバ名。
        /// </summary>
        public string MemberName { get; }

        #endregion
    }

    public class ListConfigurationAttribute: ConfigurationAttribute
    {
        public ListConfigurationAttribute()
            : base()
        { }

        public ListConfigurationAttribute(string memberName, Type type)
            : base(memberName)
        { }

        #region proeprty

        //public Type Type { get; }

        #endregion
    }
}
