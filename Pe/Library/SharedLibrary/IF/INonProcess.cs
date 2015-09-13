namespace ContentTypeTextNet.Library.SharedLibrary.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;

	/// <summary>
	/// 各種操作なんかで持ち運びたい処理しない(ように見える)データ群。
	/// </summary>
	public interface INonProcess
	{
		ILogger Logger { get; }
		ILanguage Language { get; }
	}
}
