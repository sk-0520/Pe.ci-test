namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Drawing;
	using System.Linq;
	using System.Windows.Forms;
	using System.Xml;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Logic;

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
			StackListWidth = Literal.stackListWidthLimit.median;
			TemplateListWidth = Literal.templateListWidthLimit.median;
			TextFont = new FontSetting(SystemFonts.DialogFont);

			Enabled = true;
			EnabledTypes = ClipboardType.Text | ClipboardType.Rtf | ClipboardType.Html | ClipboardType.Image | ClipboardType.File;

			SaveHistory = false;
			SaveTypes = ClipboardType.Text | ClipboardType.Rtf;
#if DEBUG
			SaveHistory = true;
			SaveTypes = ClipboardType.Text | ClipboardType.Rtf | ClipboardType.Html | ClipboardType.Image | ClipboardType.File;
#endif

			//SleepTime = Literal.clipboardSleepTime.median;
			WaitTime = Literal.clipboardWaitTime.median;

			ToggleHotKeySetting = new HotKeySetting();

			ClipboardListType = ClipboardListType.History;

			TemplateItems = new EventList<TemplateItem>();

			ClipboardRepeated = Literal.clipboardRepeated.median;
		}

		/// <summary>
		/// クリップボード履歴を取り込むか。
		/// </summary>
		public bool Enabled { get; set; }
		/// <summary>
		/// クリップボード取り込み対象。
		/// </summary>
		public ClipboardType EnabledTypes { get; set; }
		/// <summary>
		/// クリップボード履歴を保存するか。
		/// </summary>
		public bool SaveHistory { get; set; }
		/// <summary>
		/// クリップボード履歴保存対象。
		/// </summary>
		public ClipboardType SaveTypes { get; set; }

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
		/// クリップボード・テンプレートリストの横幅。
		/// </summary>
		public int StackListWidth { get; set; }
		/// <summary>
		/// テキストテンプレートリストの横幅。
		/// </summary>
		public int TemplateListWidth { get; set; }
		/// <summary>
		/// 最前面表示。
		/// </summary>
		public bool TopMost { get; set; }
		/// <summary>
		/// テキストデータのフォント。
		/// </summary>
		public FontSetting TextFont { get; set; }
		/// <summary>
		/// クリップボードデータ。
		/// </summary>
		[XmlIgnore]
		public FixedSizedList<ClipboardItem> HistoryItems { get; set; }

		/// <summary>
		/// テンプレートデータ。
		/// </summary>
		[XmlIgnore]
		public EventList<TemplateItem> TemplateItems { get; set; }
		/// <summary>
		/// コピーを検知を無視する。
		/// </summary>
		[XmlIgnore]
		[Obsolete("#225")]
		public bool DisabledCopy { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[XmlIgnore]
		public TimeSpan WaitTime { get; set; }
		[XmlElement("WaitTime", DataType = "duration")]
		public string _WaitTime
		{
			get { return PropertyUtility.MixinTimeSpanGetter(WaitTime); }
			set { WaitTime =  PropertyUtility.MixinTimeSpanSetter(value); }
		}
		/// <summary>
		/// 
		/// </summary>
		[XmlIgnore]
		[Obsolete("#225")]
		public TimeSpan SleepTime { get; set; }
		[XmlElement("SleepTime", DataType = "duration")]
		[Obsolete("#225")]
		public string _SleepTime
		{
			get; //{ return PropertyUtility.MixinTimeSpanGetter(SleepTime); }
			set; //{ SleepTime = PropertyUtility.MixinTimeSpanSetter(value); }
		}
		/// <summary>
		/// クリップボードリストのタイプ。
		/// </summary>
		public ClipboardListType ClipboardListType { get; set; }

		/// <summary>
		/// 重複判定範囲。
		/// 
		/// -1: 全て
		/// 0: 無し
		/// +N: Nを含む範囲
		/// </summary>
		public int ClipboardRepeated { get; set; }

		public bool DoubleClickToOutput { get; set; }
		public bool OutputUsingClipboard { get; set; }

		public HotKeySetting ToggleHotKeySetting { get; set; }

		public override void CorrectionValue()
		{
			base.CorrectionValue();

			Limit = Literal.clipboardLimit.ToRounding(Limit);
			HistoryItems = new FixedSizedList<ClipboardItem>(Limit);

			//SleepTime = Literal.clipboardSleepTime.ToRounding(SleepTime);
			WaitTime = Literal.clipboardWaitTime.ToRounding(WaitTime);
			ClipboardRepeated = Literal.clipboardRepeated.ToRounding(ClipboardRepeated);

			StackListWidth = Literal.stackListWidthLimit.ToRounding(StackListWidth);
			TemplateListWidth = Literal.templateListWidthLimit.ToRounding(TemplateListWidth);

		}

		#region DisposableItem

		protected override void Dispose(bool disposing)
		{
			TextFont.ToDispose();
			foreach(var item in HistoryItems.Cast<DisposableItem>().Concat(TemplateItems)) {
				item.ToDispose();
			}

			base.Dispose(disposing);
		}

		#endregion
	}
}
