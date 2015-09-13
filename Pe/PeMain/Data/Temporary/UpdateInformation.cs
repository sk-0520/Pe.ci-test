namespace ContentTypeTextNet.Pe.PeMain.Data.Temporary
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class UpdateInformation
	{
		IEnumerable<string> _log;

		public UpdateInformation(IEnumerable<string> log)
		{
			this._log = log;
		}

		public string Version { get; set; }
		public bool IsUpdate { get; set; }
		public bool IsRcVersion { get; set; }
		public bool IsError { get; set; }
		public int ErrorCode { get; set; }

		public string Log
		{
			get
			{
				return string.Join(Environment.NewLine, this._log);
			}
		}
	}
}
