using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Bridge.Models.Data
{
    public static class ContactKinds
    {
        public static string WebSite { get; } = "web-site";
        public static string Repository { get; } = "repository";
        public static string EMail { get; } = "e-mail";
    }

    /// <summary>
    /// 連絡先。
    /// </summary>
    public interface IContact
    {
        #region property

        /// <summary>
        /// 連絡先方法。
        /// <para>ガッチガチに固めない文字列指定。</para>
        /// <para><see cref="ContactKinds"/>で指定するのがよろし。</para>
        /// </summary>
        string ContactKind { get; }
        /// <summary>
        /// 連絡先。
        /// <para><see cref="ContactKind"/>によりプログラムでの挙動や表示方法が変わる。</para>
        /// <para><see cref="ContactKinds"/>にあるものはサポートしている。</para>
        /// </summary>
        string ContactValue { get; }
        #endregion
    }
}
