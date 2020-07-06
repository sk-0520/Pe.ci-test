using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Startup
{
    public class ProgramElement: ElementBase
    {
        public ProgramElement(FileInfo fileInfo, IReadOnlyList<Regex> autoImportUntargetRegexItems, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            FileInfo = fileInfo;
            AutoImportUntargetRegexItems = autoImportUntargetRegexItems;
            IconImageLoader = new IconImageLoader(new Data.IconData() { Path = FileInfo.FullName }, Bridge.Models.Data.IconBox.Small, dispatcherWrapper, LoggerFactory);
        }

        #region property

        public FileInfo FileInfo { get; }
        IReadOnlyList<Regex> AutoImportUntargetRegexItems { get; }
        public bool IsImport { get; set; }
        public IconImageLoader IconImageLoader { get; }

        #endregion

        #region ContextElementBase

        protected override void InitializeImpl()
        {
            IsImport = !AutoImportUntargetRegexItems.Any(i => i.IsMatch(FileInfo.Name));
        }

        #endregion
    }
}
