namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.PeMain.ViewModel;

	public static class AppUtility
	{
		public static MainWorkerViewModel CreateMainWorkerViewModel()
		{
			return new MainWorkerViewModel();
		}
	}
}
