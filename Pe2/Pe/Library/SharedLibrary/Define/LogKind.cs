namespace ContentTypeTextNet.Library.SharedLibrary.Define
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// ログ種別。
	/// </summary>
	public enum LogKind
	{
		/// <summary>
		/// デバッグ情報。
		/// </summary>
		Debug,
		/// <summary>
		/// トレース情報。
		/// </summary>
		Trace,
		/// <summary>
		/// 操作情報。
		/// </summary>
		Information,
		/// <summary>
		/// 注意。
		/// </summary>
		Warning,
		/// <summary>
		/// エラー。
		/// </summary>
		Error,
		/// <summary>
		/// 異常。
		/// </summary>
		Fatal
	}
}
