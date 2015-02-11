using System;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using ContentTypeTextNet.Pe.Library.Utility;
using ContentTypeTextNet.Pe.PeMain.Logic;

namespace ContentTypeTextNet.Pe.PeMain.Data
{
	/// <summary>
	/// 設定統括
	/// </summary>
	[Serializable]
	public class MainSetting: DisposableItem
	{
		public MainSetting()
		{
			RunningInfo = new RunningInfo();
			
			Log = new LogSetting();
			SystemEnv = new SystemEnvSetting();
			Launcher = new LauncherSetting();
			Command = new CommandSetting();
			Toolbar = new ToolbarSetting();
			Note = new NoteSetting();

			Clipboard = new ClipboardSetting();

			Skin = new SkinSetting();
			
			WindowSaveTime = Literal.windowSaveTime.median;
			WindowSaveCount = Literal.windowSaveCount.median;
		}
		
		public override void CorrectionValue()
		{
			WindowSaveTime = Literal.windowSaveTime.ToRounding(WindowSaveTime);
			WindowSaveCount = Literal.windowSaveCount.ToRounding(WindowSaveCount);

			Launcher.CorrectionValue();
			Log.CorrectionValue();
			SystemEnv.CorrectionValue();
			Command.CorrectionValue();
			Toolbar.CorrectionValue();
			Note.CorrectionValue();
			Clipboard.CorrectionValue();
		}
		
		public RunningInfo RunningInfo { get; set; }
		
		/// <summary>
		/// 使用言語。
		/// </summary>
		public string LanguageName { get; set; }
		/// <summary>
		/// ランチャアイテム統括。
		/// </summary>
		public LauncherSetting Launcher { get; set; }
		/// <summary>
		/// ログ設定
		/// </summary>
		public LogSetting Log { get; set; }
		/// <summary>
		/// システム環境セッティング
		/// </summary>
		public SystemEnvSetting SystemEnv { get; set; }
		/// <summary>
		/// コマンドランチャ設定。
		/// </summary>
		public CommandSetting Command { get; set; }
		/// <summary>
		/// ツールバー
		/// </summary>
		public ToolbarSetting Toolbar { get; set; }
		/// <summary>
		/// ノード
		/// </summary>
		public NoteSetting Note { get; set; }
		
		/// <summary>
		/// ウィンドウ一覧保持数。
		/// </summary>
		public int WindowSaveCount { get; set; }
		/// <summary>
		/// ウィンドウ一覧取得時間。
		/// </summary>
		[XmlIgnore]
		public TimeSpan WindowSaveTime { get; set; }
		[XmlElement("WindowSaveTime", DataType = "duration")]
		public string _WindowSaveTime
		{
			get { return PropertyUtility.MixinTimeSpanGetter(WindowSaveTime); }
			set { WindowSaveTime = PropertyUtility.MixinTimeSpanSetter(value); }
		}

		public ClipboardSetting Clipboard { get; set; }

		/// <summary>
		/// 使用するスキン。
		/// </summary>
		public SkinSetting Skin { get; set; }

		#region DisposableItem

		protected override void Dispose(bool disposing)
		{
			Command.ToDispose();
			Clipboard.ToDispose();

			base.Dispose(disposing);
		}

		#endregion
	}
}
