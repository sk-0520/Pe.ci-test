using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.PeMain.Kind
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
		None = 0x00,
		/// <summary>
		/// 標準入力。
		/// </summary>
		In = 0x01,
		/// <summary>
		/// 標準出力。
		/// </summary>
		Out = 0x02,
		/// <summary>
		/// 標準エラー。
		/// </summary>
		Error = 0x04,
		/// <summary>
		/// Peで完全処理(In, Out, Errorは無視される)。
		/// </summary>
		Custom = 0x08,
	}
}
