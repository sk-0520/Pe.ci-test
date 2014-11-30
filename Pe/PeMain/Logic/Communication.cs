using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeMain.Data;
using PeMain.IF;
using PeUtility;
using System.ServiceModel;

namespace PeMain.Logic
{
	class AppHost: ServiceHost
	{

	}

	class Communication: ISetCommonData, IDisposable
	{
		protected ServiceHost _host;

		public Communication(string pipeName)
		{
			Name = pipeName;
			var u = new Uri(string.Format("net.pipe://localhost/{0}", Name));
			this._host = new ServiceHost(this, u);
		}

		public string Name { get; private set; }
		CommonData CommonData { get; set; }

		public void SetCommonData(CommonData commonData)
		{
			CommonData = commonData;
		}

		protected virtual void Dispose(bool disposing)
		{
			this._host.ToDispose();
		}

		public void Dispose()
		{
			Dispose(true);
		}

		public void Initialize()
		{
			Debug.Assert(CommonData != null);
		}
	}
}
