namespace ContentTypeTextNet.Pe.Library.Skin
{
	using System;

	/// <summary>
	/// スキン情報。
	/// </summary>
	public interface ISkinAbout
	{
		/// <summary>
		/// スキン名。
		/// </summary>
		string Name { get; }
		/// <summary>
		/// 作者。
		/// </summary>
		string Author { get; }
		/// <summary>
		/// スキン配布元。
		/// </summary>
		Uri Website { get; }
		/// <summary>
		/// セッティングを持つか。
		/// </summary>
		bool Setting { get; }
	}
}
