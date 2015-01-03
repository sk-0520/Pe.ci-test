using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Pe.Applications;
using ContentTypeTextNet.Pe.Library.Utility;
using ContentTypeTextNet.Pe.PeMain.IF;

namespace ContentTypeTextNet.Pe.PeMain.Data
{
	/// <summary>
	/// プログラムの種類。
	/// </summary>
	public enum ApplicationType
	{
		/// <summary>
		/// コンソール。
		/// </summary>
		Console,
		/// <summary>
		/// GUI。
		/// </summary>
		Window,
	}

	/// <summary>
	/// 標準入出力に関するフラグ。
	/// </summary>
	[Flags]
	public enum ApplicationStream
	{
		/// <summary>
		/// なし
		/// </summary>
		None  = 0x00,
		/// <summary>
		/// 標準入力。
		/// </summary>
		In    = 0x01,
		/// <summary>
		/// 標準出力。
		/// </summary>
		Out   = 0x02,
		/// <summary>
		/// 標準エラー。
		/// </summary>
		Error = 0x04,
		/// <summary>
		/// Peで完全処理(In, Out, Errorは無視される)。
		/// </summary>
		Custom = 0x08,
	}

	/// <summary>
	/// パラメータの種類。
	/// </summary>
	public enum ApplicationParameterType
	{
		/// <summary>
		/// 名前のみ。
		/// </summary>
		NameOnly,
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

	public enum ApplicationCommunication
	{
		Event,
		ClientServer,
	}

	/// <summary>
	/// アプリケーションのファイル設定。
	/// </summary>
	[Serializable]
	public class ApplicationFile: NameItem
	{
		/// <summary>
		/// ディレクトリ名
		/// </summary>
		[XmlAttribute]
		public string Directory { get; set; }
		/// <summary>
		/// ヘルプ。
		/// </summary>
		[XmlAttribute]
		public string Help { get; set; }
		/// <summary>
		/// ヘルプ。
		/// </summary>
		[XmlAttribute]
		public bool Setting { get; set; }
		/// <summary>
		/// ヘルプ。
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

		public string DirectoryPath
		{
			get
			{
				return Path.Combine(Literal.ApplicationBinDirPath, File.Directory);
			}
		}
		public string FilePath
		{
			get
			{
				return Path.Combine(DirectoryPath, File.Name);
			}
		}
		public string SettingDirectoryPath
		{
			get
			{
				return Path.Combine(Literal.ApplicationSettingBaseDirectoryPath, File.Directory);
			}
		}
		public string LogDirectoryPath
		{
			get
			{
				return Path.Combine(Literal.ApplicationLogBaseDirectoryPath, File.Directory);
			}
		}


		public IDictionary<string, string> CreateExecuterEV()
		{
			var result = new Dictionary<string, string>() {
				{ EVLiteral.systemExecuteFilePath, Literal.ApplicationExecutablePath },
				{ EVLiteral.systemDirectoryPath, Literal.ApplicationRootDirPath },
				{ EVLiteral.systemSettingDirectoryPath, Literal.UserSettingDirPath },
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

	public class ApplicationExecuteItem: INameItem
	{
		public ApplicationExecuteItem(ApplicationItem item)
		{
			ApplicationItem = item;
		}

		public ApplicationItem ApplicationItem { get; private set; }
		public Process Process { get; set; }
		public EventWaitHandle Event { get; set; }

		#region INameItem

		public string Name
		{
			get { return ApplicationItem.Name;  }
			set { throw new NotImplementedException(); }
		}

		#endregion
	}

	[Serializable]
	public class ApplicationSetting: IDisposable
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
		
		protected virtual void Dispose(bool disposing)
		{ }

		public void Dispose()
		{
			Dispose(true);
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
