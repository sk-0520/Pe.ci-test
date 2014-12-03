/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/10/13
 * 時刻: 11:33
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */

namespace ContentTypeTextNet.Pe.Application.IF
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
}
