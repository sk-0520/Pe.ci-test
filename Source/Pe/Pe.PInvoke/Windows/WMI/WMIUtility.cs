using System.Management;

namespace ContentTypeTextNet.Pe.PInvoke.Windows.root.CIMV2
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
