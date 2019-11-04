using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize
{
    public class EnvironmentPathExecuteItemViewModel: ViewModelBase
    {
        public EnvironmentPathExecuteItemViewModel(EnvironmentPathExecuteItem item, ILoggerFactory loggerFactory)
            :base(loggerFactory)
        {
            Item = item;
        }

        #region property

        EnvironmentPathExecuteItem Item { get; }

        public string Name => Path.GetFileNameWithoutExtension(Item.File.Name);
        public string DirectoryPath => Item.Directory.FullName;
        public string FileName => Item.File.Name;

        #endregion

        #region object

        public override string ToString() => Name;

        #endregion

    }
}
