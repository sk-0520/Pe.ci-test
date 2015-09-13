namespace ContentTypeTextNet.Library.SharedLibrary.Event
{
	using System;
	using System.CodeDom.Compiler;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// T4変換時のエラー出力イベント。
	/// </summary>
	public class TextTemplatingErrorEventArgs : EventArgs
	{
		public TextTemplatingErrorEventArgs(CompilerErrorCollection errors)
		{
			CompilerErrorCollection = errors;
		}

		/// <summary>
		/// エラー内容。
		/// </summary>
		public CompilerErrorCollection CompilerErrorCollection { get; private set; }
	}
}
