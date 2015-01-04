
namespace ContentTypeTextNet.Pe.PeMain.IF
{
	public interface IItem { }
	
	/// <summary>
	/// データ補正関係処理
	/// </summary>
	public interface ICorrectionItem
	{
		void CorrectionValue();
	}

	public interface INameItem: IItem
	{
		/// <summary>
		/// 名前
		/// </summary>
		string Name { get; set; }
	}

	public interface IRangeItem<T>: IItem
	{
		T Start { get; set; }
		T End { get; set; }
	}
}
