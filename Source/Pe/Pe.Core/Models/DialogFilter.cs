using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// ダイアログで使用するフィルタのアイテム。
    /// </summary>
    public class DialogFilterItem
    {
        /// <summary>
        /// 初期化。
        /// </summary>
        /// <param name="display">表示文字列。</param>
        /// <param name="wildcard">ワイルドカード一覧。</param>
        public DialogFilterItem(string display, IEnumerable<string> wildcard)
        {
            Display = display;
            Wildcard = new List<string>(wildcard);
        }

        /// <summary>
        /// 初期化。
        /// </summary>
        /// <param name="display">表示文字列。</param>
        /// <param name="wildcard">ワイルドカード一覧。</param>
        public DialogFilterItem(string display, params string[] wildcard)
            : this(display, (IEnumerable<string>)wildcard)
        { }

        #region property

        /// <summary>
        /// フィルタリングに使用するワイルドカード一覧。
        /// </summary>
        public IReadOnlyList<string> Wildcard { get; }

        #endregion

        #region IDisplayText

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
            return string.Format("{0}|{1}", Display, string.Join(";", Wildcard));
        }

        #endregion
    }

    /// <summary>
    /// ダイアログで使用するフィルタのアイテム。
    /// <para>値を保持する。</para>
    /// </summary>
    public class DialogFilterItem<TValue> : DialogFilterItem
    {
        public DialogFilterItem(TValue value, string display, IEnumerable<string> wildcard)
            : base(display, wildcard)
        {
            Value = value;
        }

        public DialogFilterItem(TValue value, string display, params string[] wildcard)
            : base(display, wildcard)
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

    public class DialogFilterList : Collection<DialogFilterItem>
    {
        #region

        public override string ToString()
        {
            return string.Join("|", this.Select(i => i.ToString()));
        }

        #endregion
    }

}
