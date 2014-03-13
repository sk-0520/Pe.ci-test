/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/02/11
 * 時刻: 20:32
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;

namespace PeUtility
{
	[Serializable]
	public struct TPair<TFIRST, TSECOND>
	{
		public TFIRST First { get; set; }
		public TSECOND Second { get; set; }
	}
	/// <summary>
	/// 評価
	/// </summary>
	public class Evaluation
	{
		public bool Eval { get; set; }
		public string Info { get; set; }
		public object Tag { get; set; }
	}
	/// <summary>
	/// 評価とその結果
	/// </summary>
	public class EvaluationResult<T>: Evaluation
	{
		public T Result { get; set; }
	}
}
