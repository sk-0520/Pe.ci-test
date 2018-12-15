using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.ViewElement.Startup;

namespace ContentTypeTextNet.Pe.Main.ViewModel.Startup
{
    public class ImportProgramsViewModel : SingleModelViewModelBase<ImportProgramsViewElement>
    {
        public ImportProgramsViewModel(ImportProgramsViewElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }
    }
}
