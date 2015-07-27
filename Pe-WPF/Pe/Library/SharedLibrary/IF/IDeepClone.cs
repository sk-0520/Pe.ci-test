namespace ContentTypeTextNet.Library.SharedLibrary.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// ディープコピー。
	/// <para>ICloneableはシャローなのかディープなのかようわからん。</para>
	/// </summary>
	public interface IDeepClone
	{
		/// <summary>
		/// 全データを完全複製。
		/// <para>ほぼほぼDeepCloneToを呼び出すためだけに存在している。</para>
		/// </summary>
		/// <returns></returns>
		IDeepClone DeepClone();

		/// <summary>
		/// 全データを完全複製。
		/// <para>状態までは面倒見ない。DBへのコネクションとかね。</para>
		/// <para>おおよそDeepCloneの内部実装。</para>
		/// </summary>
		/// <param name="target"></param>
		void DeepCloneTo(IDeepClone target);
	}
}
