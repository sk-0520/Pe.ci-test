namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;

	public class SystemLogger : Logger
	{
		public SystemLogger()
			:base()
		{
			LoggerConfig.EnabledAll = true;
			LoggerConfig.PutsDebug = true;
		}

		protected override void Dispose(bool disposing)
		{
			if (!IsDisposed) {
				Information("exit!");
			}
			base.Dispose(disposing);
		}
	}
}
