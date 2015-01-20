namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Threading;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Kind;

	/// <summary>
	/// パラメータの種類。
	/// </summary>
	public enum ApplicationParameterType
	{
		/// <summary>
		/// 名前のみ。
		/// </summary>
		KeyOnly,
		/// <summary>
		/// 真偽値。
		/// </summary>
		Boolean,
		/// <summary>
		/// 文字列。
		/// </summary>
		Text,
		/// <summary>
		/// 数値。
		/// </summary>
		Number,
		/// <summary>
		/// 16進数。
		/// </summary>
		//Hex,
	}

	/// <summary>
	/// アプリケーションの通信方法。
	/// </summary>
	public enum ApplicationCommunication
	{
		/// <summary>
		/// イベントによる終了通知。
		/// </summary>
		Event,
		/// <summary>
		/// C/Sにてデータの相互通信。
		/// </summary>
		ClientServer,
	}

	/// <summary>
	/// アプリケーションのファイル設定。
	/// </summary>
	[Serializable]
	public class ApplicationFile: NameItem
	{
		/// <summary>
		/// ディレクトリ名。
		/// </summary>
		[XmlAttribute]
		public string Directory { get; set; }
		/// <summary>
		/// ヘルプ。
		/// </summary>
		[XmlAttribute]
		public string Help { get; set; }
		/// <summary>
		/// 設定を行うか。
		/// </summary>
		[XmlAttribute]
		public bool Setting { get; set; }
		/// <summary>
		/// ログ出力を行うか。
		/// </summary>
		[XmlAttribute]
		public bool Log { get; set; }
	}

	/// <summary>
	/// パラメータ。
	/// </summary>
	[Serializable]
	public class ApplicationParameter: NameItem
	{
		/// <summary>
		/// パラメータの種類。
		/// </summary>
		[XmlAttribute]
		public ApplicationParameterType Type { get; set; }
		/// <summary>
		/// 必要項目か。
		/// </summary>
		[XmlAttribute]
		public bool Necessary { get; set; }

		/// <summary>
		/// 値。
		/// </summary>
		[XmlAttribute]
		public string Value { get; set; }
	}

	/// <summary>
	/// アプリケーション。
	/// </summary>
	[Serializable]
	public class ApplicationItem: NameItem
	{
		public ApplicationItem()
		{
			Parameters = new List<ApplicationParameter>();
		}

		/// <summary>
		/// アプリケーションの共通IFとしての言語キー。
		/// </summary>
		[XmlAttribute]
		public string LanguageKey { get; set; }

		/// <summary>
		/// ファイル設定。
		/// </summary>
		public ApplicationFile File { get; set; }
		/// <summary>
		/// 種類。
		/// </summary>
		public ApplicationType Type { get; set; }
		/// <summary>
		/// 標準入出力。
		/// </summary>
		public ApplicationStream Stream { get; set; }
		/// <summary>
		/// パラメータ一覧。
		/// </summary>
		[XmlArrayItem("Arg")]
		public List<ApplicationParameter> Parameters { get; set; }
		/// <summary>
		/// 通信方法。
		/// </summary>
		public ApplicationCommunication Communication { get; set; }
		/// <summary>
		/// 管理者権限で実行。
		/// </summary>
		public bool Administrator { get; set; }
		/// <summary>
		/// ディレクトリパス。
		/// </summary>
		public string DirectoryPath
		{
			get
			{
				return Path.Combine(Literal.ApplicationBinDirectoryPath, File.Directory);
			}
		}
		/// <summary>
		/// アプリケーション自体のパス。
		/// </summary>
		public string FilePath
		{
			get
			{
				return Path.Combine(DirectoryPath, File.Name);
			}
		}
		/// <summary>
		/// ヘルプのパス。
		/// 
		/// ファイル・URIを問わない。
		/// </summary>
		public string HelpPath
		{
			get
			{
				//TODO: ファイル限定
				return Path.Combine(DirectoryPath, File.Help);
			}
		}
		/// <summary>
		/// 設定ファイルを配置するディレクトリパス。
		/// </summary>
		public string SettingDirectoryPath
		{
			get
			{
				return Path.Combine(Literal.ApplicationSettingBaseDirectoryPath, File.Directory);
			}
		}
		/// <summary>
		/// ログファイルを配置するディレクトリパス。
		/// </summary>
		public string LogDirectoryPath
		{
			get
			{
				return Path.Combine(Literal.ApplicationLogBaseDirectoryPath, File.Directory);
			}
		}

		/// <summary>
		/// アプリケーションに対してPeから渡される環境変数のデータを作成する。
		/// </summary>
		/// <returns></returns>
		public IDictionary<string, string> CreateExecuterEV()
		{
			var result = new Dictionary<string, string>() {
				{ EVLiteral.systemExecuteFilePath, Literal.ApplicationExecutablePath },
				{ EVLiteral.systemDirectoryPath, Literal.ApplicationRootDirectoryPath },
				{ EVLiteral.systemSettingDirectoryPath, Literal.UserSettingDirectoryPath },
				{ EVLiteral.systemLogDirectoryPath, Literal.LogFileDirPath },
			};

			if(File.Setting) {
				result[EVLiteral.applicationSettingDirectoryPath] = SettingDirectoryPath;
			}
			if(File.Log) {
				result[EVLiteral.applicationLogDirectoryPath] = LogDirectoryPath;
			}

			var communication = new Dictionary<ApplicationCommunication, TPair<string, string>>() {
				{ ApplicationCommunication.Event, TPair<string,string>.Create(EVLiteral.communicationEventName, string.Format("e-{0}", Name)) },
				{ ApplicationCommunication.ClientServer, TPair<string,string>.Create(EVLiteral.communicationServerName, string.Format("s-{0}", Name)) },
			}[Communication];
			result[communication.First] = communication.Second;

			return result;
		}
	}

	/// <summary>
	/// 実行アプリケーションのデータ。
	/// </summary>
	public class ApplicationExecuteItem: DisposableNameItem
	{
		public ApplicationExecuteItem(ApplicationItem item)
		{
			ApplicationItem = item;
		}

		/// <summary>
		/// 実行中アプリケーション。
		/// </summary>
		public ApplicationItem ApplicationItem { get; private set; }
		/// <summary>
		/// 実行中プロセス。
		/// </summary>
		public Process Process { get; set; }
		/// <summary>
		/// 通信用イベント。
		/// </summary>
		public EventWaitHandle Event { get; set; }

		#region INameItem

		/// <summary>
		/// 実行中アプリケーション名。
		/// </summary>
		public new string Name
		{
			get { return ApplicationItem.Name;  }
			//set { throw new NotImplementedException(); }
		}

		#endregion

		#region IDisposable

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			switch(ApplicationItem.Communication) {
				case ApplicationCommunication.Event:
					Event.ToDispose();
					break;

				case ApplicationCommunication.ClientServer:
				default:
					throw new NotImplementedException();
			}

			if(Process.HasExited) {
				Process.ToDispose();
			}
		}

		#endregion
	}

	/// <summary>
	/// アプリケーション設定。
	/// </summary>
	[Serializable]
	public class ApplicationSetting: DisposableItem
	{
		public ApplicationSetting()
		{
			Items = new List<ApplicationItem>();
			ExecutingItems = new List<ApplicationExecuteItem>();
		}

		public List<ApplicationItem> Items { get; set; }

		[XmlIgnore]
		public List<ApplicationExecuteItem> ExecutingItems { get; set; }

		#region IDisposable
		
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		#endregion

		public bool IsExecutingItem(string name)
		{
			return ExecutingItems.Any(i => i.Name == name);
		}

		public ApplicationItem GetApplicationItem(LauncherItem item)
		{
			Debug.Assert(item.LauncherType == LauncherType.Embedded);
			return Items.Single(i => i.Name == item.Command);
		}

		public void KillApplicationItem(ApplicationExecuteItem applicationExecuteItem)
		{
			switch(applicationExecuteItem.ApplicationItem.Communication) {
				case ApplicationCommunication.Event:
					applicationExecuteItem.Event.Set();
					break;

				case ApplicationCommunication.ClientServer:
				default:
					throw new NotImplementedException();
			}
		}
		public void KillApplicationItem(string name)
		{
			var applicationExecuteItem = ExecutingItems.Single(i => i.Name == name);
			KillApplicationItem(applicationExecuteItem);
		}
		public void KillApplicationItem(LauncherItem item)
		{
			Debug.Assert(item.LauncherType == LauncherType.Embedded);
			KillApplicationItem(item.Command);
		}
		public void KillAllApplication()
		{
			foreach(var item in ExecutingItems.ToArray()) {
				KillApplicationItem(item);
			}
		}
	}

}
