namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.PeMain.IF;

	public class SystemLogger : Logger
	{
		#region variable

		ILogCollector _logCollector;

		#endregion

		public SystemLogger()
			: base()
		{
			LoggerConfig.EnabledAll = true;
			LoggerConfig.PutsDebug = true;
		}

		#region property

		public ILogCollector LogCollector
		{
			get { return this._logCollector; }
			set
			{
				if (this._logCollector != null) {
					// イベントとか切る用
				}

				this._logCollector = value;
				
				if (this._logCollector == null) {
					LoggerConfig.PutsCustom = false;
				} else {
					LoggerConfig.PutsCustom = true;
				}
			}
		}

		#endregion

		#region Logger

		protected override void Dispose(bool disposing)
		{
			if (!IsDisposed) {
				Information("exit!");
			}
			base.Dispose(disposing);
		}

		protected override void PutsCustom(LogItemModel item)
		{
			if (LogCollector != null) {
				LogCollector.Puts(item);
			}
		}

		#endregion
	}
}
