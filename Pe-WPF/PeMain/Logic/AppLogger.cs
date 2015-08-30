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

	public class AppLogger : Logger
	{
		#region variable

		ILogAppender _logCollector;
		bool _isStock;

		#endregion

		public AppLogger()
			: base()
		{
			LoggerConfig.EnabledAll = true;
			LoggerConfig.PutsDebug = true;
		}

		#region property

		public ILogAppender LogCollector
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

		public List<LogItemModel> StockItems { get; private set; }

		public bool IsStock
		{
			get { return this._isStock; }
			set
			{
				this._isStock = value;
				if (this._isStock) {
					if (StockItems == null) {
						StockItems = new List<LogItemModel>();
					}
				} else {
					if (StockItems != null) {
						StockItems.Clear();
						StockItems = null;
					}
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
				LogCollector.AddLog(item);
			}
		}

		protected override void Puts(LogItemModel item)
		{
			if (IsStock) {
				StockItems.Add(item);
			}
			base.Puts(item);
		}

		#endregion
	}
}
