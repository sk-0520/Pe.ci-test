using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// ダイアログで使用するフィルタのアイテム。
    /// </summary>
    public record DialogFilterItem
    {
        /// <summary>
        /// 初期化。
        /// </summary>
        /// <param name="display">表示文字列。</param>
        /// <param name="wildcards">ワイルドカード一覧。</param>
        public DialogFilterItem(string display, string defaultExtension, IEnumerable<string> wildcards)
        {
            Display = display;
            Wildcards = new List<string>(wildcards);
            DefaultExtension = defaultExtension;
        }


        /// <summary>
        /// 初期化。
        /// </summary>
        /// <param name="display">表示文字列。</param>
        /// <param name="wildcard">ワイルドカード一覧。</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "HAA0504:Implicit new array creation allocation")]
        public DialogFilterItem(string display, string defaultExtension, string wildcard, params string[] wildcards)
            : this(display, defaultExtension, new[] { wildcard }.Concat(wildcards))
        { }

        #region property

        /// <summary>
        /// フィルタリングに使用するワイルドカード一覧。
        /// </summary>
        public IReadOnlyList<string> Wildcards { get; }

        /// <summary>
        /// デフォルト拡張子。
        /// </summary>
        public string DefaultExtension { get; }

        /// <summary>
        /// 表示名
        /// </summary>
        public string Display { get; private set; }

        #endregion

        #region object

        /// <summary>
        /// ダイアログフィルタとして使用する生値。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}|{1}", Display, string.Join(";", Wildcards));
        }

        #endregion
    }

    /// <summary>
    /// ダイアログで使用するフィルタのアイテム。
    /// <para>値を保持する。</para>
    /// </summary>
    public record DialogFilterItem<TValue>: DialogFilterItem
    {
        public DialogFilterItem(TValue value, string display, string defaultExtension, IEnumerable<string> wildcard)
            : base(display, defaultExtension, wildcard)
        {
            Value = value;
        }

        public DialogFilterItem(TValue value, string display, string defaultExtension, params string[] wildcard)
            : base(display, defaultExtension, wildcard)
        {
            Value = value;
        }

        #region property

        /// <summary>
        /// 保持する値。
        /// </summary>
        public TValue Value { get; }

        #endregion

    }

    public class DialogFilterList: Collection<DialogFilterItem>
    {
        #region

        public override string ToString()
        {
            return string.Join("|", this.Select(i => i.ToString()));
        }

        #endregion
    }
}
