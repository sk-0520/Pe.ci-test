using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ICSharpCode.AvalonEdit.Document;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.StandardInputOutput
{
    public class StandardInputOutputElement : ElementBase, IViewShowStarter, IViewCloseReceiver
    {
        #region variable

        bool _isVisible;
        bool _preparatedReceive;

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
        Screen Screen { get; }
        IOrderManager OrderManager { get; }

        public StreamReceiver? InputStreamReceiver { get; private set; }

        bool ViewCreated { get; set; }

        public bool IsVisible
        {
            get => this._isVisible;
            private set => SetProperty(ref this._isVisible, value);
        }

        public bool PreparatedReceive
        {
            get => this._preparatedReceive;
            private set => SetProperty(ref this._preparatedReceive, value);
        }

        #endregion

        #region function

        public void PreparateReceiver()
        {
            if(Process.HasExited) {
                Logger.LogWarning("既に終了したプロセス: id = {0}, name = {1}, exit coe = {2}, exit time = {3}", Process.Id, Process.ProcessName, Process.ExitCode, Process.ExitTime);
                return;
            }

            InputStreamReceiver = new StreamReceiver(Process.StandardOutput, LoggerFactory);

            PreparatedReceive = true;
        }

        public void RunReceiver()
        {
            Debug.Assert(PreparatedReceive);

            InputStreamReceiver!.StartReceive();
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
