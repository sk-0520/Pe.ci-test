namespace ContentTypeTextNet.Pe.PeMain.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.PeMain.Data;

	public interface IAppNonProcess: INonProcess
	{
		VariableConstants VariableConstants { get; }
		LauncherIconCaching LauncherIconCaching { get; }
	}
}
