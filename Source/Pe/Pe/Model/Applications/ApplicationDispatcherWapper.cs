using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Applications
{
    /// <summary>
    /// UI スレッドであれやこれや頑張る。
    /// </summary>
    public class ApplicationDispatcherWapper : DispatcherWapper
    {
        public ApplicationDispatcherWapper(ILogger logger)
            : base(Application.Current.Dispatcher)
        { }

    }
}
