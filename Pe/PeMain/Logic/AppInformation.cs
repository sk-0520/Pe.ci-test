namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System.Diagnostics;
	using ContentTypeTextNet.Pe.Library.Utility;

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
