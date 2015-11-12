namespace ContentTypeTextNet.Pe.PeMain.IF
{
	/// <summary>
	/// アイテム基底。
	/// </summary>
	public interface IItem
	{ }

	/// <summary>
	/// データ補正関係処理。
	/// </summary>
	public interface ICorrectionItem
	{
		/// <summary>
		/// データ補正処理。
		/// </summary>
		void CorrectionValue();
	}

	/// <summary>
	/// 名前付きアイテム。
	/// </summary>
	public interface INameItem: IItem
	{
		/// <summary>
		/// 名前
		/// </summary>
		string Name { get; set; }
	}

	/// <summary>
	/// 範囲持ちアイテム。
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IRangeItem<T>: IItem
	{
		/// <summary>
		/// 範囲の開始点。
		/// </summary>
		T Start { get; set; }
		/// <summary>
		/// 範囲の終了点。
		/// </summary>
		T End { get; set; }
	}
}
