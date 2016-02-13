/*
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.Logic;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
using ContentTypeTextNet.Pe.PeMain.ViewModel;
using Hardcodet.Wpf.TaskbarNotification;

namespace ContentTypeTextNet.Pe.PeMain
{
    /// <summary>
    /// Simple application. Check the XAML for comments.
    /// </summary>
    public partial class App: Application
    {
        #region variable

        TaskbarIcon _notifyIcon;
        MainWorkerViewModel _mainWorker;
        Mutex _instanceMutex;

        #endregion

        #region Application

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

#if DEBUG
            DebugProcess();
#endif

            var commandLine = new CommandLine();
            var constants = new VariableConstants(commandLine);
            var systemLogger = AppUtility.CreateSystemLogger(constants.FileLogging, constants.LogDirectoryPath);
            systemLogger.IsStock = true;
            systemLogger.Information("application: start", commandLine);
            systemLogger.Information("environment information", new AppInformationCollection().ToString());
            systemLogger.Debug("mutex name: " + constants.MutexName);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            bool isFirstInstance;
            this._instanceMutex = new Mutex(true, constants.MutexName, out isFirstInstance);
            if(!isFirstInstance) {
                systemLogger.Error("duplicate boot", constants.MutexName);
                Application.Current.Shutdown();
            } else {
                this._mainWorker = new MainWorkerViewModel(constants, systemLogger);
                var startupNotifiyData = this._mainWorker.Initialize();
                if(startupNotifiyData.QuickExecute) {
                    systemLogger.Information("application: quick exec");
                    Application.Current.Shutdown();
                }
                if(startupNotifiyData.AcceptRunning) {
                    //LanguageUtility.RecursiveSetLanguage(this._notifyIcon, this._mainWorker.Language);
                    this._notifyIcon = (TaskbarIcon)FindResource("root");
                    this._notifyIcon.DataContext = this._mainWorker;
                    this._mainWorker.SetView(this._notifyIcon);
                    if(!startupNotifiyData.ExistsSetting) {
                        // 設定データがなければ完全に初回とする
                        systemLogger.Information("application: first");
                        Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                            this._mainWorker.ShowHomeDialog();
                            this._mainWorker.ResetToolbarWindow();
                            this._mainWorker.CheckUpdateProcessAsync();
                            this._mainWorker.SendUserInformation();
                        }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                    } else {
                        this._mainWorker.CheckUpdateProcessAsync();
                        this._mainWorker.SendUserInformation();
                    }
                } else {
                    // 終了
                    systemLogger.Information("application: cancel exec");
                    Application.Current.Shutdown();
                }
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if(this._instanceMutex != null) {
                this._instanceMutex.Dispose();
                this._instanceMutex = null;
            }
            if(this._notifyIcon != null) {
                this._notifyIcon.Dispose();
            }
            if(this._mainWorker != null) {
                this._mainWorker.Dispose();
            }

            AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;

            base.OnExit(e);
        }

        #endregion

        #region function

        void CatchUnhandleException(Exception ex, bool callerUiThread)
        {
            if(this._mainWorker != null && this._mainWorker.CommonData != null && this._mainWorker.CommonData.Logger != null) {
                this._mainWorker.CommonData.Logger.Fatal(ex);
            } else {
                Debug.WriteLine(ex);
            }
        }

        #endregion

        /// <summary>
        /// UIスレッド
        /// </summary>
        void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            CatchUnhandleException(e.Exception, true);
            //e.Handled = true;
        }

        /// <summary>
        /// 非UIスレッド
        /// </summary>
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            CatchUnhandleException((Exception)e.ExceptionObject, false);

            Shutdown();
        }

    }
}
