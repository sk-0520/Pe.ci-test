namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Reflection;
	using System.Windows.Forms;
	using System.Xml;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic;

	/// <summary>
	/// 設定統括
	/// </summary>
	[Serializable]
	public class MainSetting: DisposableItem, IDeepClone
	{
		public MainSetting()
		{
			Running = new RunningSetting();
			
			Log = new LogSetting();
			SystemEnvironment = new SystemEnvironmentSetting();
			Launcher = new LauncherSetting();
			Stream = new StreamSetting();
			Command = new CommandSetting();
			Toolbar = new ToolbarSetting();
			Note = new NoteSetting();

			Clipboard = new ClipboardSetting();

			Skin = new SkinSetting();
			
			WindowSaveTime = Literal.windowSaveTime.median;
			WindowSaveCount = Literal.windowSaveCount.median;
		}

		/// <summary>
		/// 実行許可とかそんな感じ。
		/// </summary>
		[XmlElement("RunningInfo")]
		public RunningSetting Running { get; set; }
		
		/// <summary>
		/// 使用言語。
		/// </summary>
		public string LanguageName { get; set; }
		/// <summary>
		/// ランチャアイテム統括。
		/// </summary>
		public LauncherSetting Launcher { get; set; }
		public StreamSetting Stream { get; set; }
		/// <summary>
		/// ログ設定
		/// </summary>
		public LogSetting Log { get; set; }
		/// <summary>
		/// システム環境セッティング
		/// </summary>
		[XmlElement("SystemEnv")]
		public SystemEnvironmentSetting SystemEnvironment { get; set; }
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

		#region override

		public override void CorrectionValue()
		{
			WindowSaveTime = Literal.windowSaveTime.ToRounding(WindowSaveTime);
			WindowSaveCount = Literal.windowSaveCount.ToRounding(WindowSaveCount);

			Launcher.CorrectionValue();
			Stream.CorrectionValue();
			Log.CorrectionValue();
			SystemEnvironment.CorrectionValue();
			Command.CorrectionValue();
			Toolbar.CorrectionValue();
			Note.CorrectionValue();
			Clipboard.CorrectionValue();
		}

		#endregion

		#region DisposableItem

		protected override void Dispose(bool disposing)
		{
			Command.ToDispose();
			Clipboard.ToDispose();

			base.Dispose(disposing);
		}

		#endregion

		#region IDeepClone

		public IDeepClone DeepClone()
		{
			var result = new MainSetting() {
				WindowSaveTime = this.WindowSaveTime,
				WindowSaveCount = this.WindowSaveCount,
			};

			result.Running = (RunningSetting)Running.Clone();
			result.Log = (LogSetting)Log.Clone();
			result.SystemEnvironment = (SystemEnvironmentSetting)SystemEnvironment.DeepClone();
			result.Launcher = (LauncherSetting)Launcher.DeepClone();
			result.Stream = (StreamSetting)Stream.DeepClone();
			result.Command = (CommandSetting)Command.DeepClone();
			result.Toolbar = (ToolbarSetting)Toolbar.DeepClone();
			result.Note = (NoteSetting)Note.DeepClone();
			result.Clipboard = (ClipboardSetting)Clipboard.DeepClone();
			result.Skin = (SkinSetting)Skin.DeepClone();

			return result;
		}

		#endregion
	}
}
