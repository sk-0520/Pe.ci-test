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
        public ConfigurationAttribute(string memberName = "", string rootConvertMethodName = "", string nestConvertMethodName = "")
        {
            MemberName = memberName;
            RootConvertMethodName = rootConvertMethodName;
            NestConvertMethodName = nestConvertMethodName;
        }

        #region proeprty

        /// <summary>
        /// アプリケーション構成ファイルでのメンバ名。
        /// </summary>
        public string MemberName { get; }
        /// <summary>
        /// 変換処理に呼び出すメソッド名。
        /// <para><see cref="ConfigurationSetting"/>から呼び出せるメソッドであること。</para>
        /// <para>通常の内部規約としてIFは<c>プロパティTに対して</c><c>T MethodName(IConfigurationSection section, string key)</c>を実行する。</para>
        /// <para>ジェネリック内部規約としてIFは<c>プロパティTResult&lt;T...&gt;に対して</c><c>TResult&lt;T...&gt; MethodName&lt;T...&gt;(IConfigurationSection section, string key)</c>を実行する。</para>
        /// </summary>
        public string RootConvertMethodName { get; }
        /// <summary>
        /// 配列の中身に適用する処理的な。
        /// <inheritdoc cref="RootConvertMethodName"/>
        /// <para><see cref="RootConvertMethodName"/>が優先される。</para>
        /// </summary>
        public string NestConvertMethodName { get; }

        #endregion
    }

}
