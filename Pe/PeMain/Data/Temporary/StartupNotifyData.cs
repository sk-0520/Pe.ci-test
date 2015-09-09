namespace ContentTypeTextNet.Pe.PeMain.Data.Temporary
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class StartupNotifyData : NotifyDataBase
	{
		#region property

		public bool ExistsSetting { get; set; }
		public bool ExistsFormsSetting { get; set; }

		public bool AcceptRunning { get; set; }

		#endregion
	}
}
