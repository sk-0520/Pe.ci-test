using System;
using System.Collections.Generic;
using System.Text;

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
}
