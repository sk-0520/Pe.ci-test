namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Reflection;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Define;

	/// <summary>
	/// ログとして出力するデータ。
	/// </summary>
	[Serializable]
	public class LogItemModel: ModelBase
	{
		/// <summary>
		/// 発生日。
		/// </summary>
		[DataMember]
		public DateTime DateTime { get; set; }
		/// <summary>
		/// ログ種別。
		/// </summary>
		[DataMember]
		public LogKind LogKind { get; set; }
		/// <summary>
		/// ログメッセージ。
		/// </summary>
		[DataMember]
		public string Message { get; set; }
		/// <summary>
		/// 詳細。
		/// <para>nullで何もなし。</para>
		/// <para>判定にはHasDetailを使用。</para>
		/// </summary>
		[DataMember]
		public object Detail { get; set; }
		/// <summary>
		/// 詳細はあるか。
		/// </summary>
		public bool HasDetail { get { return Detail == null; } }
		/// <summary>
		/// スタックトレース情報。
		/// </summary>
		[DataMember]
		public StackTrace StackTrace { get; set; }
		/// <summary>
		/// 呼び出しファイルパス。
		/// </summary>
		[DataMember]
		public string CallerFile { get; set; }
		/// <summary>
		/// 呼び出し行番号。
		/// </summary>
		[DataMember]
		public int CallerLine { get; set; }
		/// <summary>
		/// 呼び出しメンバ。
		/// </summary>
		[DataMember]
		public string CallerMember { get; set; }
		/// <summary>
		/// 呼び出しアセンブリ。
		/// </summary>
		[DataMember]
		public Assembly CallerAssembly { get; set; }
		/// <summary>
		/// 呼び出しスレッド。
		/// </summary>
		[DataMember]
		public Thread CallerThread { get; set; }
	}
}
