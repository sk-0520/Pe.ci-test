namespace ContentTypeTextNet.Library.SharedLibrary.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// EqualsとかGetHashCodeとかめんどいんすよ、等価だけやりたいんすよ、ようわからんのですよ。
	/// </summary>
	public interface IIsEqual
	{
		bool IsEqual(IIsEqual target);
	}
}
