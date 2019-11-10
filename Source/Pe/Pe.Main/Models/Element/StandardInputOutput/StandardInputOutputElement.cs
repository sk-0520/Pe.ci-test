using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.StandardInputOutput
{
    public class StandardInputOutputElement : ElementBase, IViewShowStarter, IViewCloseReceiver
    {
        #region variable

        bool _isVisible;

        #endregion

        public StandardInputOutputElement(string captionName, Process process, Screen screen, IOrderManager orderManager, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            CaptionName = captionName;
            Process = process;
            Screen = screen;
            OrderManager = orderManager;
        }

        #region property

        public string CaptionName { get; }
        Process Process { get; }
        ProcessStartInfo StartInfo => Process.StartInfo;
        Screen Screen { get; }
        IOrderManager OrderManager { get; }

        bool ViewCreated { get; set; }

        public bool IsVisible
        {
            get => this._isVisible;
            private set => SetProperty(ref this._isVisible, value);
        }

        #endregion

        #region function

        public void BeginProcess()
        {
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
        }

        #endregion

        #region IViewShowStarter

        public bool CanStartShowView
        {
            get
            {
                if(ViewCreated) {
                    return false;
                }

                return IsVisible;
            }
        }

        #endregion

        #region IViewShowStarter

        public void StartView()
        {
            var windowItem = OrderManager.CreateStandardInputOutputWindow(this);
            windowItem.Window.Show();
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
