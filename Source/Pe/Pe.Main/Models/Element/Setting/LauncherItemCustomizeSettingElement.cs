using System;
using System.Collections.Generic;
using System.Linq;
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
        public LauncherItemCustomizeSettingElement(Guid launcherItemId, LauncherItemKind kind, IClipboardManager clipboardManager, LauncherIconElement launcherIconElement, ILoggerFactory loggerFactory)
            : base(launcherItemId, clipboardManager, launcherIconElement, loggerFactory)
        {
            Kind = kind;
        }

        #region LauncherItemCustomizeElementBase
        public override IReadOnlyCollection<LauncherEnvironmentVariableData> LoadEnvironmentVariableItems()
        {
            return new List<LauncherEnvironmentVariableData>();
        }

        public override LauncherFileData LoadFileData()
        {
            return new LauncherFileData();
        }

        public override IReadOnlyCollection<string> LoadTags()
        {
            return new List<string>();
        }

        public override void SaveFile(LauncherItemData launcherItemData, LauncherFileData launcherFileData, IEnumerable<LauncherEnvironmentVariableData> environmentVariableItems, IEnumerable<string> tags)
        {
            throw new NotSupportedException();
        }

        protected override void InitializeImpl()
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
