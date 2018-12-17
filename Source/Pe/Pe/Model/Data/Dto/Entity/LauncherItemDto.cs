using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Model.Data.Dto.Entity
{
    public interface IReadOnlyLauncherItemRowDto : IReadOnlyRowDtoBase
    {
        #region property

        Guid LauncherItemId { get; }

        string Code { get; }
        string Name { get; }
        string Kind { get; }
        string Command { get; }
        string Option { get; }
        string WorkDirectory { get; }
        string IconPath { get; }
        long IconIndex { get; }
        bool IsEnabledCommandLauncher { get; }
        bool IsEnabledCustomEnvVar { get; }
        bool IsEnabledStandardOutput { get; }
        bool IsEnabledStandardInput { get; }
        string Permission { get; }
        Guid CredentId { get; }
        long ExecuteCount { get; }
        DateTime LastExecuteTimestamp { get; }
        string Note { get; }

        #endregion
    }

    public class LauncherItemRowDto : RowDtoBase, IReadOnlyLauncherItemRowDto
    {
        #region IReadOnlyLauncherItemRowDto

        public Guid LauncherItemId { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public string Kind { get; set; }
        public string Command { get; set; }
        public string Option { get; set; }
        public string WorkDirectory { get; set; }
        public string IconPath { get; set; }
        public long IconIndex { get; set; }
        public bool IsEnabledCommandLauncher { get; set; }
        public bool IsEnabledCustomEnvVar { get; set; }
        public bool IsEnabledStandardOutput { get; set; }
        public bool IsEnabledStandardInput { get; set; }
        public string Permission { get; set; }
        public Guid CredentId { get; set; }
        public long ExecuteCount { get; set; }
        public DateTime LastExecuteTimestamp { get; set; }
        public string Note { get; set; }

        #endregion
    }
}
