namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Drawing;
	using System.Xml;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Pe.Library.Skin;
	using ContentTypeTextNet.Pe.PeMain.Logic;

	/// <summary>
	/// ツールバーの設定。
	/// </summary>
	[Serializable]
	public class ToolbarItem: DisposableNameItem, IDisposable
	{
		public static bool CheckNameEqual(string a, string b)
		{
			return a == b;
		}

		#region Equals and GetHashCode implementation !![ operator == ]!!
		public override bool Equals(object obj)
		{
			var item = obj as LauncherItem;
			if(item == null) {
				return false;
			}
			return IsNameEqual(item.Name);
		}

		public override int GetHashCode()
		{
			//if(this.Name == null) {
			//	return default(int);
			//}
			return Name.GetHashCode();
		}
		#endregion

		public ToolbarItem()
		{
			FontSetting = new FontSetting();
			ToolbarPosition = ToolbarPosition.DesktopRight;
			IconScale = IconScale.Small;
			ShowText = false;
			Visible = true;
			Topmost = true;
			AutoHide = false;
			FloatSize = Literal.toolbarFloatSize;
			DesktopSize = Literal.toolbarDesktopSize;
			TextWidth = Literal.toolbarTextWidth.median;

			HiddenWaitTime = Literal.toolbarHiddenTime.median;
			HiddenAnimateTime = Literal.toolbarAnimateTime.median;

			DefaultGroup = string.Empty;
		}

		public override void CorrectionValue()
		{
			TextWidth = Literal.toolbarTextWidth.ToRounding(TextWidth);
			HiddenWaitTime = Literal.toolbarHiddenTime.ToRounding(HiddenWaitTime);
			HiddenAnimateTime = Literal.toolbarAnimateTime.ToRounding(HiddenAnimateTime);
		}

		/// <summary>
		/// 表示
		/// </summary>
		public bool Visible { get; set; }
		/// <summary>
		/// 最前面表示
		/// </summary>
		public bool Topmost { get; set; }
		/// <summary>
		/// ツールバー位置
		/// </summary>
		public ToolbarPosition ToolbarPosition { get; set; }
		/// <summary>
		/// 自動的に隠す
		/// </summary>
		public bool AutoHide { get; set; }
		/// <summary>
		/// フロート状態サイズ
		/// </summary>
		public Size FloatSize { get; set; }
		/// <summary>
		/// フロート状態位置
		/// </summary>
		public Point FloatLocation { get; set; }
		/// <summary>
		/// デスクトップでのサイズ
		/// </summary>
		public Size DesktopSize { get; set; }
		/// <summary>
		/// アイコンサイズ
		/// </summary>
		public IconScale IconScale { get; set; }
		/// <summary>
		/// テキスト表示
		/// </summary>
		public bool ShowText { get; set; }
		/// <summary>
		/// テキスト表示欄の長さ(px)
		/// </summary>
		public int TextWidth { get; set; }
		/// <summary>
		/// フォント
		/// </summary>
		public FontSetting FontSetting { get; set; }
		/// <summary>
		/// 非表示までの時間
		/// </summary>
		[XmlIgnore]
		public TimeSpan HiddenWaitTime { get; set; }
		[XmlElement("HiddenWaitTime", DataType = "duration")]
		public string _HiddenWaitTime
		{
			get { return PropertyUtility.MixinTimeSpanGetter(HiddenWaitTime); }
			set { HiddenWaitTime = PropertyUtility.MixinTimeSpanSetter(value); }
		}
		/// <summary>
		/// 非表示のアニメーション時間
		/// </summary>
		[XmlIgnore]
		public TimeSpan HiddenAnimateTime { get; set; }
		[XmlElement("HiddenAnimateTime", DataType = "duration")]
		public string _HiddenAnimateTime
		{
			get { return PropertyUtility.MixinTimeSpanGetter(HiddenAnimateTime); }
			set { HiddenAnimateTime = PropertyUtility.MixinTimeSpanSetter(value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public string DefaultGroup { get; set; }

		public bool IsNameEqual(string name)
		{
			return CheckNameEqual(Name, name);
		}

		protected override void Dispose(bool disposing)
		{
			FontSetting.Dispose();

			base.Dispose(disposing);
		}
	}
}
