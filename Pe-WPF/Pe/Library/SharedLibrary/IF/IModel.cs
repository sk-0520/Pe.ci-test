namespace ContentTypeTextNet.Library.SharedLibrary.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public interface IModel: IGetMembers
	{
		/// <summary>
		/// 補正。
		/// </summary>
		void Correction();
	}
}
