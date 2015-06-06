using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.PeMain.IF
{
	/// <summary>
	/// 内部オブジェクトを含めて複製を行うIF。
	/// <para>不変オブジェクトの場合はこの限りではない</para>
	/// </summary>
	public interface IDeepClone
	{
		/// <summary>
		/// コピー処理。
		/// </summary>
		/// <returns></returns>
		IDeepClone DeepClone();
	}
}
