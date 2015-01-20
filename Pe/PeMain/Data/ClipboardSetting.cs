using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using ContentTypeTextNet.Pe.Library.Utility;

namespace ContentTypeTextNet.Pe.PeMain.Data
{
	/// <summary>
	/// 認識可能とするクリップボード形式。
	/// </summary>
	[Flags]
	public enum ClipboardType
	{
		/// <summary>
		/// 無効。
		/// </summary>
		None  = 0,
		/// <summary>
		/// プレーンテキスト。
		/// </summary>
		Text  = 0x01,
		/// <summary>
		/// RTF。
		/// </summary>
		Rtf = 0x02,
		/// <summary>
		/// HTML。
		/// </summary>
		Html = 0x04,
		/// <summary>
		/// 画像。
		/// </summary>
		Image = 0x08,
		/// <summary>
		/// ファイル。
		/// </summary>
		File = 0x10,
		/// <summary>
		/// 全部
		/// </summary>
		All = Text | Rtf | Html | Image | File
	}

	[Serializable]
	public class ClipboardSetting: DisposableItem
	{
		public ClipboardSetting()
		{
			Visible = false;
			Limit = Literal.clipboardLimit.median;
			//Items = new FixedSizedList<ClipboardItem>(Limit);
			EnabledApplicationCopy = false;
			Size = new Size(
				Screen.PrimaryScreen.Bounds.Width / 3,
				Screen.PrimaryScreen.Bounds.Height / 3
			);
			var screenArea = Screen.PrimaryScreen.WorkingArea;
			Location = new Point(screenArea.X, screenArea.Height - Size.Height);
			TextFont = new FontSetting(SystemFonts.DialogFont);

			Enabled = true;
			EnabledTypes = ClipboardType.Text | ClipboardType.Rtf | ClipboardType.Html | ClipboardType.Image | ClipboardType.File;

			SleepTime = Literal.clipboardSleepTime.median;
			WaitTime = Literal.clipboardWaitTime.median;

			ToggleHotKeySetting = new HotKeySetting();
		}

		/// <summary>
		/// クリップボードユーティリティを使用するか。
		/// </summary>
		public bool Enabled { get; set; }
		/// <summary>
		/// クリップボード通知対象。
		/// </summary>
		public ClipboardType EnabledTypes { get; set; }
		/// <summary>
		/// サイズ。
		/// </summary>
		public int Limit { get; set; }
		/// <summary>
		/// 本体でのコピー操作でもコピー検知に含めるか。
		/// </summary>
		public bool EnabledApplicationCopy { get; set; }
		/// <summary>
		/// 表示状態。
		/// </summary>
		public bool Visible { get; set; }
		/// <summary>
		/// サイズ。
		/// </summary>
		public Size Size { get; set; }
		/// <summary>
		/// 位置。
		/// </summary>
		public Point Location { get; set; }
		/// <summary>
		/// 最前面表示。
		/// </summary>
		public bool TopMost { get; set; }
		/// <summary>
		/// テキストデータのフォント
		/// </summary>
		public FontSetting TextFont { get; set; }
		/// <summary>
		/// クリップボードデータ
		/// </summary>
		[XmlIgnore]
		public FixedSizedList<ClipboardItem> Items { get; set; }
		/// <summary>
		/// コピーを検知を無視する
		/// </summary>
		[XmlIgnore]
		public bool DisabledCopy { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[XmlIgnore]
		public TimeSpan WaitTime { get; set; }
		[XmlElement("WaitTime", DataType = "duration")]
		public string _WaitTime
		{
			get { return XmlConvert.ToString(WaitTime); }
			set
			{
				if(!string.IsNullOrWhiteSpace(value)) {
					WaitTime = XmlConvert.ToTimeSpan(value);
				}
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[XmlIgnore]
		public TimeSpan SleepTime { get; set; }
		[XmlElement("SleepTime", DataType = "duration")]
		public string _SleepTime
		{
			get { return XmlConvert.ToString(SleepTime); }
			set
			{
				if(!string.IsNullOrWhiteSpace(value)) {
					SleepTime = XmlConvert.ToTimeSpan(value);
				}
			}
		}

		public override void CorrectionValue()
		{
			base.CorrectionValue();

			Limit = Literal.clipboardLimit.ToRounding(Limit);
			Items = new FixedSizedList<ClipboardItem>(Limit);

			SleepTime = Literal.clipboardSleepTime.ToRounding(SleepTime);
			WaitTime = Literal.clipboardWaitTime.ToRounding(WaitTime);
		}

		public HotKeySetting ToggleHotKeySetting { get; set; }

		#region DisposableItem

		protected override void Dispose(bool disposing)
		{
			TextFont.ToDispose();

			base.Dispose(disposing);
		}

		#endregion
	}
}
