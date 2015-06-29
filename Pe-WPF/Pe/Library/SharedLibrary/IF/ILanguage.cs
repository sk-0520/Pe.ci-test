namespace ContentTypeTextNet.Library.SharedLibrary.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public interface ILanguage
	{
		string this[string key, IReadOnlyDictionary<string, string> map = null] { get; }
	}
}
