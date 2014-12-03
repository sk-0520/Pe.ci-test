/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/18
 * 時刻: 11:45
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;
using ContentTypeTextNet.Pe.Library.Skin;
using ContentTypeTextNet.Pe.Library.Utility;

namespace ContentTypeTextNet.Pe.PeMain.Data
{

	
	/// <summary>
	/// 
	/// </summary>
	public static class ToolbarPositionUtility
	{
		public static ToolbarPosition ToToolbarPosition(DesktopDockType value)
		{
			return new Dictionary<DesktopDockType, ToolbarPosition>() {
				{ DesktopDockType.Left,   ToolbarPosition.DesktopLeft },
				{ DesktopDockType.Top,    ToolbarPosition.DesktopTop },
				{ DesktopDockType.Right,  ToolbarPosition.DesktopRight },
				{ DesktopDockType.Bottom, ToolbarPosition.DesktopBottom },
			}[value];
		}
		public static DesktopDockType ToDockType(ToolbarPosition value)
		{
			return new Dictionary<ToolbarPosition, DesktopDockType>() {
				{ToolbarPosition.DesktopLeft,   DesktopDockType.Left },
				{ToolbarPosition.DesktopTop,    DesktopDockType.Top },
				{ToolbarPosition.DesktopRight,  DesktopDockType.Right },
				{ToolbarPosition.DesktopBottom, DesktopDockType.Bottom },
			}[value];
		}
		public static bool IsDockingMode(ToolbarPosition value)
		{
			return value.IsIn(ToolbarPosition.DesktopLeft, ToolbarPosition.DesktopTop, ToolbarPosition.DesktopRight, ToolbarPosition.DesktopBottom);
		}
		public static bool IsHorizonMode(ToolbarPosition pos)
		{
			return pos.IsIn(
				ToolbarPosition.DesktopFloat,
				ToolbarPosition.DesktopTop,
				ToolbarPosition.DesktopBottom,
				ToolbarPosition.WindowTop,
				ToolbarPosition.WindowBottom
			);
		}
		
	}
	
	/// <summary>
	/// ツールバーグループとして名前を管理。
	/// </summary>
	[Serializable]
	public class ToolbarGroupItem: NameItem
	{
		public ToolbarGroupItem()
		{
			ItemNames = new List<string>();
		}
		
		public List<string> ItemNames { get; set; }
	}

	/// <summary>
	/// ツールバーの各グループを統括。
	/// </summary>
	[Serializable]
	public class ToolbarGroup: Item
	{
		public ToolbarGroup()
		{
			Groups = new List<ToolbarGroupItem>();
		}
		
		public List<ToolbarGroupItem> Groups { get; set; }
	}
	
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
			get { return XmlConvert.ToString(HiddenWaitTime); }
			set
			{
				if(!string.IsNullOrWhiteSpace(value)) {
					HiddenWaitTime = XmlConvert.ToTimeSpan(value);
				}
			}
		}
		/// <summary>
		/// 非表示のアニメーション時間
		/// </summary>
		[XmlIgnore]
		public TimeSpan HiddenAnimateTime { get; set; }
		[XmlElement("HiddenAnimateTime", DataType = "duration")]
		public string _HiddenAnimateTime
		{
			get { return XmlConvert.ToString(HiddenAnimateTime); }
			set
			{
				if(!string.IsNullOrWhiteSpace(value)) {
					HiddenAnimateTime = XmlConvert.ToTimeSpan(value);
				}
			}
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
	
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class ToolbarSetting: Item
	{
		public ToolbarSetting()
		{
			ToolbarGroup = new ToolbarGroup();
			
			Items = new HashSet<ToolbarItem>();
		}
		
		public ToolbarGroup ToolbarGroup { get; set; }
		public HashSet<ToolbarItem> Items { get; set; }
	}
}
