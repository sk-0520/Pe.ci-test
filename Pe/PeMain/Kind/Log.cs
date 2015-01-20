namespace ContentTypeTextNet.Pe.PeMain.Kind
{
	using System;

	/// <summary>
	/// ログ種別。
	/// </summary>
	[Flags]
	public enum LogType
	{
		/// <summary>
		/// 使用しない。
		/// </summary>
		None = 0x00,
		/// <summary>
		/// 情報。
		/// </summary>
		Information = 0x01,
		/// <summary>
		/// 警告。
		/// </summary>
		Warning = 0x02,
		/// <summary>
		/// 異常。
		/// </summary>
		Error = 0x04,
		/// <summary>
		/// デバッグ。
		/// </summary>
		Debug
	}
}
