using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
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
		#region Define

		public const string CommunicationEvent = "ev";
		public const string CommunicationServerClient = "sc";

		#endregion ////////////////////////

		/// <summary>
		/// ディレクトリ名
		/// </summary>
		[XmlAttribute]
		public string Directory { get; set; }
		/// <summary>
		/// 通信種別。
		/// </summary>
		[XmlAttribute]
		ApplicationCommunication Communication { get; set; }
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
	}

	[Serializable]
	public class ApplicationSetting
	{
		public ApplicationSetting()
		{
			Items = new List<ApplicationItem>();
		}

		public List<ApplicationItem> Items { get; set; }
	}

	public class ApplicationExecuteItem: INameItem
	{
		public ApplicationExecuteItem(ApplicationItem item)
		{
			ApplicationItem = item;
		}

		public ApplicationItem ApplicationItem { get; private set; }
		public Process Process { get; set; }
		public AutoResetEvent Event { get; set; }

		#region INameItem

		public string Name
		{
			get { return ApplicationItem.Name;  }
			set { throw new NotImplementedException(); }
		}

		#endregion
	}
}
