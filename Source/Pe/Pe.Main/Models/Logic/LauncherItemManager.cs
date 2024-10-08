using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Base;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public class LauncherItemManager
    {
        #region function

        public string CreateNewName(LauncherItemKind kind, IReadOnlyCollection<string> names)
        {
            var baseName = kind switch {
                LauncherItemKind.File => Properties.Resources.String_LauncherItem_NewItem_FileName,
                LauncherItemKind.Addon => Properties.Resources.String_LauncherItem_NewItem_AddonName,
                LauncherItemKind.Separator => Properties.Resources.String_LauncherItem_NewItem_SeparatorName,
                _ => throw new NotImplementedException()
            };

            return TextUtility.ToUnique(baseName, names, StringComparison.OrdinalIgnoreCase, (s, n) => $"{s}({n})");
        }

        #endregion
    }
}
