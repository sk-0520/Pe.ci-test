namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Pe.PeMain.Logic;

	[Serializable]
	public class TemplateItem: DisposableNameItem
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

		[XmlIgnore]
		public TemplateProcessor Processor { get; set; }
	}
}
