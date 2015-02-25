namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	[Serializable]
	public class TemplateItem: NameItem
	{
		/// <summary>
		/// 置き換えモードを使用するか。
		/// </summary>
		public bool ReplaceMode { get; set; }
		/// <summary>
		/// プログラム的置き換えを使用するか。
		/// </summary>
		public bool Program { get; set; }
		/// <summary>
		/// 対象文字列。
		/// </summary>
		public string Source { get; set; }
	}
}
