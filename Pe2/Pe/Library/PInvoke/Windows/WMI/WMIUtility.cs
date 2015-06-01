using System.Management;

namespace ContentTypeTextNet.Library.PInvoke.Windows.root.CIMV2
{
	public interface IImportWMI
	{
		void Import(ManagementBaseObject obj);
	}
	/*
	public static class WMIUtility
	{
	}
	*/
}
