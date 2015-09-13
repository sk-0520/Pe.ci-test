namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Drawing;
	using ContentTypeTextNet.Pe.Library.Utility;
using ContentTypeTextNet.Pe.PeMain.IF;

	/// <summary>
	/// ノートの設定。
	/// 
	/// ノート一つ一つではなくノートという機能に対する設定。
	/// </summary>
	[Serializable]
	public class NoteSetting: DisposableItem, IDisposable, IDeepClone
	{
		public NoteSetting()
		{
			CreateHotKey = new HotKeySetting();
			HiddenHotKey = new HotKeySetting();
			CompactHotKey = new HotKeySetting();
			ShowFrontHotKey = new HotKeySetting();
			
			CaptionFontSetting = new FontSetting(SystemFonts.CaptionFont);
		}
		
		/// <summary>
		/// 新規作成時のホットキー
		/// </summary>
		public HotKeySetting CreateHotKey { get; set; }
		public HotKeySetting HiddenHotKey { get; set; }
		public HotKeySetting CompactHotKey { get; set; }
		public HotKeySetting ShowFrontHotKey { get; set; }
		
		public FontSetting CaptionFontSetting { get; set; }

		#region DisposableItem

		protected override void Dispose(bool disposing)
		{
			CaptionFontSetting.ToDispose();

			base.Dispose(disposing);
		}

		#endregion

		#region IDeepClone

		public IDeepClone DeepClone()
		{
			return new NoteSetting() {
				CreateHotKey = (HotKeySetting)CreateHotKey.Clone(),
				HiddenHotKey = (HotKeySetting)HiddenHotKey.Clone(),
				CompactHotKey = (HotKeySetting)CompactHotKey.Clone(),
				ShowFrontHotKey = (HotKeySetting)ShowFrontHotKey.Clone(),
				CaptionFontSetting = (FontSetting)CaptionFontSetting.Clone(),
			};
		}

		#endregion
	}
}
