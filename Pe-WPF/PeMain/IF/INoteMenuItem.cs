namespace ContentTypeTextNet.Pe.PeMain.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Input;

	public interface INoteMenuItem : IMenuItem
	{
		ICommand NoteMenuSelectedCommand { get; }
	}
}
