namespace ContentTypeTextNet.Pe.Bridge.Models.Data
{
    /// <summary>
    /// 標準的な連絡先の一覧。
    /// </summary>
    public static class ContactKinds
    {
        /// <summary>
        /// Webサイト。
        /// </summary>
        public static string Website { get; } = "website";
        /// <summary>
        /// プロジェクトサイト。
        /// </summary>
        public static string ProjectSite { get; } = "project-site";
        /// <summary>
        /// 電子メール。
        /// </summary>
        public static string Email { get; } = "email";
    }

    /// <summary>
    /// 連絡先。
    /// </summary>
    public interface IContact
    {
        #region property

        /// <summary>
        /// 連絡先方法。
        /// </summary>
        /// <remarks>
        /// <para>ガッチガチに固めない文字列指定。</para>
        /// <para><see cref="ContactKinds"/>で指定するのがよろし。</para>
        /// </remarks>
        string ContactKind { get; }
        /// <summary>
        /// 連絡先。
        /// </summary>
        /// <remarks>
        /// <para><see cref="ContactKind"/>によりプログラムでの挙動や表示方法が変わる。</para>
        /// <para><see cref="ContactKinds"/>にあるものはサポートしている。</para>
        /// </remarks>
        string ContactValue { get; }
        #endregion
    }

    /// <inheritdoc cref="IContact" />
    public class Contact: IContact
    {
        public Contact(string kind, string value)
        {
            ContactKind = kind;
            ContactValue = value;
        }

        #region IContact

        /// <inheritdoc cref="IContact.ContactKind"/>
        public string ContactKind { get; }

        /// <inheritdoc cref="IContact.ContactValue"/>
        public string ContactValue { get; }

        #endregion
    }

}
