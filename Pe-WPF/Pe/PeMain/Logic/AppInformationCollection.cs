namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Data;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;

	public class AppInformationCollection : InformationCollection
	{
		public override FileVersionInfo GetVersionInfo
		{
			get { return FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location); }
		}

		public override InformationGroup GetApplication()
		{
			var result = base.GetApplication();

			result.Items.Add("BuildType", Constants.BuildType);
			
			return result;
		}
	}
}

