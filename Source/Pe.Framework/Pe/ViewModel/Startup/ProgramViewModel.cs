using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Element.Startup;

namespace ContentTypeTextNet.Pe.Main.ViewModel.Startup
{
    public class ProgramViewModel : SingleModelViewModelBase<ProgramElement>
    {
        public ProgramViewModel(ProgramElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property

        public string FileName => Path.GetFileNameWithoutExtension(Model.FileInfo.Name);
        public bool IsImport
        {
            get => Model.IsImport;
            set => SetModelValue(value);
        }
        #endregion
    }
}
