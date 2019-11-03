using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.StandardInputOutput
{
    public class StandardInputOutputElement : ElementBase, IViewShowStarter, IViewCloseReceiver
    {
        public StandardInputOutputElement(string id, IOrderManager orderManager, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Id = id;
            OrderManager = orderManager;
        }

        #region property

        public string Id { get; }

        IOrderManager OrderManager { get; }

        bool ViewCreated { get; set; }

        #endregion

        #region function
        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IViewShowStarter

        public bool CanStartShowView => throw new NotImplementedException();

        #endregion

        #region IViewShowStarter

        public void StartView()
        {
            var windowItem = OrderManager.CreateStandardInputOutputWindow(this);

            ViewCreated = true;
        }

        public bool ReceiveViewUserClosing()
        {
            return true;
        }

        public bool ReceiveViewClosing()
        {
            return true;
        }

        public void ReceiveViewClosed()
        {
            ViewCreated = false;
        }


        #endregion

    }
}
