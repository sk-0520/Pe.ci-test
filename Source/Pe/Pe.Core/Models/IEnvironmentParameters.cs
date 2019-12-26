using System.IO;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public interface IEnvironmentParameters
    {
        #region property

        DirectoryInfo ApplicationDirectory { get; }
        DirectoryInfo AssemblyDirectory { get; }
        DirectoryInfo DocumentDirectory { get; }
        DirectoryInfo EtcDirectory { get; }
        FileInfo FileFile { get; }
        DirectoryInfo MachineArchiveDirectory { get; }
        DirectoryInfo MachineDirectory { get; }
        DirectoryInfo MachineUpdateDirectory { get; }
        DirectoryInfo MainSqlDirectory { get; }
        DirectoryInfo RootDirectory { get; }
        FileInfo SettingFile { get; }
        DirectoryInfo SettingTemporaryDirectory { get; }
        DirectoryInfo SqlDirectory { get; }
        DirectoryInfo SystemApplicationDirectory { get; }
        DirectoryInfo TemporaryDirectory { get; }
        DirectoryInfo UserBackupDirectory { get; }
        DirectoryInfo UserRoamingDirectory { get; }
        DirectoryInfo UserSettingDirectory { get; }

        #endregion
    }

    public interface IConfiguration
    {
        #region property
        #endregion
    }


}
