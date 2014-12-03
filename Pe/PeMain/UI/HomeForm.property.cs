using System;
using System.Collections.Generic;
using ContentTypeTextNet.Pe.PeMain.Data;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	partial class HomeForm
	{
		CommonData CommonData { get; set; }
		
		public bool ItemFound { get; private set; }
		public IReadOnlyList<LogItem> LogList { get { return this._logList; } }
	}
}
