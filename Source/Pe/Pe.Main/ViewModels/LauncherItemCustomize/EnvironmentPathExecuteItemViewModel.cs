using System.IO;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize
{
    public class EnvironmentPathExecuteItemViewModel: ViewModelBase
    {
        public EnvironmentPathExecuteItemViewModel(EnvironmentPathExecuteItem item, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Item = item;
        }

        #region property

        EnvironmentPathExecuteItem Item { get; }

        public string Name => Path.GetFileNameWithoutExtension(Item.File.Name);
        public string DirectoryPath
        {
            get
            {
                if(Item.Directory.FullName.EndsWith(Path.DirectorySeparatorChar) || Item.Directory.FullName.EndsWith(Path.AltDirectorySeparatorChar)) {
                    return Item.Directory.FullName;
                }
                return Item.Directory.FullName + Path.DirectorySeparatorChar;
            }
        }
        public string FileName => Item.File.Name;
        public string FullPath => Item.File.FullName;

        #endregion

        #region object

        public override string ToString() => Name;

        #endregion

    }
}
