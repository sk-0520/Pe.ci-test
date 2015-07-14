namespace ContentTypeTextNet.Pe.PeMain.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using System.Windows.Media;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	public interface IMenuItem: IDisplayText
	{
		ImageSource MenuImage { get; }
		ICommand MenuSelectedCommand { get; }
	}
}
