using System.Diagnostics;
using ContentTypeTextNet.Pe.Library.Utility;

namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	/// <summary>
	/// Description of Information.
	/// </summary>
	public class AppInformation:InformationCollection
	{
		public override FileVersionInfo GetVersionInfo
		{
			get { return FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location); }
		}
	}
}
