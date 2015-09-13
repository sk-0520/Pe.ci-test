namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Drawing;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.IF;

	[Serializable]
	public class StreamSetting: DisposableItem, IDeepClone
	{
		public StreamSetting()
		{
			FontSetting = new FontSetting();

			GeneralColor = new ColorPairItem();
			InputColor = new ColorPairItem();
			ErrorColor = new ColorPairItem();

			GeneralColor.CorrectionColor(Literal.streamGeneralForeground, Literal.streamGeneralBackground);
			InputColor.CorrectionColor(Literal.streamInputForeground, Literal.streamInputBackground);
			ErrorColor.CorrectionColor(Literal.streamErrorForeground, Literal.streamErrorBackground);
		}

		/// <summary>
		/// 標準出力フォント。
		/// </summary>
		public FontSetting FontSetting { get; set; }

		/// <summary>
		/// 通常。
		/// </summary>
		public ColorPairItem GeneralColor { get; set; }
		/// <summary>
		/// 入力時。
		/// </summary>
		public ColorPairItem InputColor { get; set; }
		/// <summary>
		/// エラー。
		/// </summary>
		public ColorPairItem ErrorColor { get; set; }

		#region DisposableItem

		protected override void Dispose(bool disposing)
		{
			FontSetting.ToDispose();
			base.Dispose(disposing);
		}

		#endregion

		#region IDeepClone

		public IDeepClone DeepClone()
		{
			return new StreamSetting() {
				FontSetting = (FontSetting)this.FontSetting.Clone(),
				GeneralColor = (ColorPairItem)this.GeneralColor.DeepClone(),
				InputColor = (ColorPairItem)this.InputColor.DeepClone(),
				ErrorColor = (ColorPairItem)this.ErrorColor.DeepClone(),
			};

		}
		#endregion
	}
}
