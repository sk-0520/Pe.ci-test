namespace ContentTypeTextNet.Pe.Library.Utility
{
	using System.Linq;
	using System.Collections.Generic;
	using System.Windows.Forms;

	/// <summary>
	/// DialogFilter内部で使用するデータ。
	/// </summary>
	public class DialogFilterItem
	{
		public DialogFilterItem()
		{
			Wildcard = new List<string>();
		}

		/// <summary>
		/// フィルターアイテムの設定。
		/// </summary>
		/// <param name="display">表示文字列。</param>
		/// <param name="wildcard">対象ワイルドカード。</param>
		public DialogFilterItem(string display, params string[] wildcard)
		{
			Display = display;
			Wildcard = new List<string>(wildcard);
		}

		/// <summary>
		/// フィルターアイテムの設定。
		/// </summary>
		/// <param name="display">表示文字列。</param>
		/// <param name="wildcard">対象ワイルドカード。</param>
		public DialogFilterItem(string display, IEnumerable<string> wildcard)
		{
			Display = display;
			Wildcard = new List<string>(wildcard);
		}

		/// <summary>
		/// 表示名。
		/// </summary>
		public string Display { get; private set; }

		/// <summary>
		/// 対象ワイルドカード。
		/// </summary>
		public IReadOnlyList<string> Wildcard { get; private set; }

		public override string ToString()
		{
			return string.Format("{0}|{1}", Display, string.Join(";", Wildcard));
		}
	}


	/// <summary>
	/// DialogFilterItemで且つなにかデータを持つ。
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class DialogFilterValueItem<T>: DialogFilterItem
	{
		public DialogFilterValueItem(T value)
			: base()
		{
			Value = value;
		}

		public DialogFilterValueItem(T value, string display, params string[] wildcard)
			: base(display, wildcard)
		{
			Value = value;
		}

		public DialogFilterValueItem(T value, string display, IEnumerable<string> wildcard)
			: base(display, wildcard)
		{
			Value = value;
		}


		public T Value { get; set; }
	}

	/// <summary>
	/// ファイルダイアログのフィルタ。
	/// </summary>
	public class DialogFilter
	{
		public DialogFilter()
		{
			Items = new List<DialogFilterItem>();
		}

		/// <summary>
		/// 各アイテム。
		/// </summary>
		public List<DialogFilterItem> Items { get; private set; }

		public override string ToString()
		{
			return string.Join("|", Items.Select(i => i.ToString()));
		}
	}

	/// <summary>
	/// くっつける。
	/// </summary>
	public static class DialogFilterUtility
	{
		public static void Attachment(this FileDialog dialog, DialogFilter filter)
		{
			dialog.Filter = filter.ToString();
		}

	}
}
