using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Define.Database;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Database
{

	/// <summary>
	/// 条件式からコマンドの構築。
	/// 
	/// DBManagerに渡すコマンドに対してさらに実装側で条件分岐を行う。
	/// </summary>
	public class CommandExpression
	{
		/// <summary>
		/// 条件式のデフォルト値生成。
		/// </summary>
		public CommandExpression()
		{
			Condition = false;
			TrueCommand = string.Empty;
			QueryCondition = QueryCondition.Command;
			FalseCommand = string.Empty;
			FalseExpression = null;
		}

		/// <summary>
		/// 条件を真とし、真の文字列コマンドを設定する。
		/// </summary>
		/// <param name="trueCommand"></param>
		public CommandExpression(string trueCommand)
			: this()
		{
			Condition = true;
			TrueCommand = trueCommand;
		}

		/// <summary>
		/// 条件式を指定値で生成。
		/// 
		/// 偽の場合は空文字列となる。
		/// </summary>
		/// <param name="condition">条件</param>
		/// <param name="trueCommand">真の場合のコマンド。</param>
		public CommandExpression(bool condition, string trueCommand)
			: this()
		{
			Condition = condition;
			TrueCommand = trueCommand;
			QueryCondition = QueryCondition.Command;
		}

		/// <summary>
		/// 条件式を指定値で生成。
		/// </summary>
		/// <param name="condition">条件</param>
		/// <param name="trueCommand">真の場合のコマンド</param>
		/// <param name="falseCommand">偽の場合のコマンド</param>
		public CommandExpression(bool condition, string trueCommand, string falseCommand)
			: this()
		{
			Condition = condition;
			TrueCommand = trueCommand;
			QueryCondition = QueryCondition.Command;
			FalseCommand = falseCommand;
		}

		/// <summary>
		/// 条件
		/// </summary>
		public bool Condition { get; private set; }
		/// <summary>
		/// 条件が真の場合のコマンド
		/// </summary>
		public string TrueCommand { get; private set; }
		/// <summary>
		/// 条件が偽の場合にコマンドと式のどちらを使用するか
		/// </summary>
		public QueryCondition QueryCondition { get; private set; }
		/// <summary>
		/// 条件が偽の場合のコマンド
		/// </summary>
		public string FalseCommand { get; private set; }
		/// <summary>
		/// 条件が偽の場合の式
		/// </summary>
		public CommandExpression FalseExpression { get; private set; }

		/// <summary>
		/// 条件をコマンドに落とし込む。
		/// </summary>
		/// <returns></returns>
		public string ToCode()
		{
			if(Condition) {
				return TrueCommand;
			}

			if(QueryCondition == QueryCondition.Command) {
				// 文字列
				return FalseCommand;
			} else {
				Debug.Assert(QueryCondition == QueryCondition.Expression);
				// 式
				return FalseExpression.ToCode();
			}
		}
	}
}
