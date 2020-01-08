using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.Startup;
using ContentTypeTextNet.Pe.Main.Models.UsageStatistics;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Startup
{
    public class ProgramViewModel : ElementViewModelBase<ProgramElement>
    {
        public ProgramViewModel(ProgramElement model, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        { }

        #region property

        public string? FileName => Path.GetFileNameWithoutExtension(Model.FileInfo.Name);
        public bool IsImport
        {
            get => Model.IsImport;
            set => SetModelValue(value);
        }
        #endregion
    }
}
