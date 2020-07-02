using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    /// <summary>
    /// UI スレッドであれやこれや頑張る。
    /// </summary>
    public sealed class ApplicationDispatcherWrapper : DispatcherWrapper
    {
        public ApplicationDispatcherWrapper(TimeSpan waitTime)
            : base(Application.Current.Dispatcher, waitTime)
        { }

    }
}
