using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherIcon;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class LauncherItemCustomizeSettingElement : LauncherItemCustomizeElementBase
    {
        public LauncherItemCustomizeSettingElement(Guid launcherItemId, LauncherIconElement launcherIconElement, IClipboardManager clipboardManager, ILoggerFactory loggerFactory)
            : base(launcherItemId, launcherIconElement, clipboardManager, loggerFactory)
        { }

        #region LauncherItemCustomizeElementBase
        public override IReadOnlyCollection<LauncherEnvironmentVariableData> LoadEnvironmentVariableItems()
        {
            throw new NotImplementedException();
        }

        public override LauncherFileData LoadFileData()
        {
            throw new NotImplementedException();
        }

        public override IReadOnlyCollection<string> LoadTags()
        {
            throw new NotImplementedException();
        }

        public override void SaveFile(LauncherItemData launcherItemData, LauncherFileData launcherFileData, IEnumerable<LauncherEnvironmentVariableData> environmentVariableItems, IEnumerable<string> tags)
        {
            throw new NotImplementedException();
        }

        protected override void InitializeImpl()
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
