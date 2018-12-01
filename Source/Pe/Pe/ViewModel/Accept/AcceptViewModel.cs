using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.ViewElement.Accept;

namespace ContentTypeTextNet.Pe.Main.ViewModel.Accept
{
    public class AcceptViewModel : SingleModelViewModelBase<AcceptViewElement>
    {
        public AcceptViewModel(AcceptViewElement model, ILogger logger)
            : base(model, logger)
        { }
    }
}
