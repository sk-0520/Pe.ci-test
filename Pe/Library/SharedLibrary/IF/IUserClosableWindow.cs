namespace ContentTypeTextNet.Library.SharedLibrary.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// http://stackoverflow.com/questions/1866537/how-to-know-whether-window-was-closed-by-x-button?answertab=votes#tab-top
	/// </summary>
	public interface IUserClosableWindow
	{
		void UserClose();
	}
}
