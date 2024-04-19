using System;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    /// <summary>
    /// アプリケーション構成データをプロパティに対して適用。
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class ConfigurationAttribute: Attribute
    {
        public ConfigurationAttribute(string memberName = "", string rootConvertMethodName = "", string nestConvertMethodName = "")
        {
            MemberName = memberName;
            RootConvertMethodName = rootConvertMethodName;
            NestConvertMethodName = nestConvertMethodName;
        }

        #region property

        /// <summary>
        /// アプリケーション構成データでのメンバ名。
        /// </summary>
        public string MemberName { get; }
        /// <summary>
        /// 変換処理に呼び出すメソッド名。
        /// </summary>
        /// <remarks>
        /// <para><see cref="ConfigurationSetting"/>から呼び出せるメソッドであること。</para>
        /// <para>通常の内部規約としてIFは<c>プロパティT</c>に対して<c>T MethodName(IConfigurationSection section, string key)</c>を実行する。</para>
        /// <para>ジェネリック内部規約としてIFは<c>プロパティTResult&lt;T...&gt;</c>に対して<c>TResult&lt;T...&gt; MethodName&lt;T...&gt;(IConfigurationSection section, string key)</c>を実行する。</para>
        /// </remarks>
        public string RootConvertMethodName { get; }
        /// <summary>
        /// 配列の中身に適用する処理的な。
        /// <inheritdoc cref="RootConvertMethodName"/>
        /// </summary>
        /// <remarks>
        /// <para><see cref="RootConvertMethodName"/>が優先される。</para>
        /// </remarks>
        public string NestConvertMethodName { get; }

        #endregion
    }
}
