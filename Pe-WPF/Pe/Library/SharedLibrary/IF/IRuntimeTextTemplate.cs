namespace ContentTypeTextNet.Library.SharedLibrary.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using Microsoft.VisualStudio.TextTemplating;

	/// <summary>
	/// 実行時テンプレートのインターフェイス
	/// </summary>
	public interface IRuntimeTextTemplate : IDisposable
	{
		ITextTemplatingEngineHost Host { get; set; }

		/// <summary>
		/// テンプレート変換を実施する.
		/// </summary>
		/// <returns></returns>
		string TransformText();
	}
}
