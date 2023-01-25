using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public class WeakEvent<TEventSource, TEventArgs>: WeakEventManager
        where TEventSource : class
        where TEventArgs : EventArgs
    {
        public WeakEvent()
            : base()
        {
        }

        #region property


        #endregion

        #region function

        public void Invoke(TEventSource sender, TEventArgs e)
        {
            DeliverEvent(sender, e);
        }

        public void Register(TEventSource sender)
        {
        }

        #endregion

        #region WeakEventManager

        protected override void StartListening(object source)
        {
            throw new NotImplementedException();
        }

        protected override void StopListening(object source)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class WeakEvent2<TEventSource, TEventArgs>
        where TEventSource : class
        where TEventArgs : EventArgs
    {
        public WeakEvent2()
        {
        }

        #region property

        // 辞書の中に弱い参照ぶちこんどきゃいいんじゃないかなぁ。

        #endregion

        #region function

        #endregion
    }

}
