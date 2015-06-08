namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.ViewModel;

	public static class AppUtility
	{
		public static MainWorkerViewModel CreateMainWorkerViewModel()
		{
			var constants = new Constants(new CommandLine());
			return new MainWorkerViewModel(constants);
		}
	}
}
