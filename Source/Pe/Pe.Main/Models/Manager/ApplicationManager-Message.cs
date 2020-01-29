using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
    partial class ApplicationManager
    {
        #region property

        public bool IsEnabledHook { get; private set; }

        HeartBeatSender? HeartBeatSender { get; set; }
        ExplorerSupporter? ExplorerSupporter { get; set; }

        public bool IsDisabledSystemIdle => HeartBeatSender != null;
        public bool IsSupportedExplorer => ExplorerSupporter != null;

        #endregion

        #region function

        private void MakeMessageWindow()
        {
            /*
            var thread = new Thread(() => {
                MessageWindowDispatcherWapper = new CurrentDispatcherWapper();
                MessageWindowHandleSource = new HwndSource(new HwndSourceParameters(nameof(MessageWindowHandleSource)) {
                    Width = 0,
                    Height = 0,
                    WindowStyle = (int)WindowStyle.None,
                    //ParentWindow = WindowsUtility.ToIntPtr(HWND.HWND_MESSAGE),
                    HwndSourceHook = MessageWindowProc,
                });
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();
            */
            MessageWindowHandleSource = new HwndSource(new HwndSourceParameters(nameof(MessageWindowHandleSource)) {
                Width = 0,
                Height = 0,
                WindowStyle = (int)WindowStyle.None,
                //ParentWindow = WindowsUtility.ToIntPtr(HWND.HWND_MESSAGE),
                HwndSourceHook = MessageWindowProc,
            });
        }

        private IntPtr MessageWindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            Logger.LogTrace("[MSG WND] hwnd = {0}, msg = {1}({2}), wParam = {3}, lParam = {4}", hwnd, msg, (WM)msg, wParam, lParam);

            switch(msg) {
                case (int)WM.WM_DEVICECHANGE: {
                        var deviceChangedData = new DeviceChangedData(hwnd, msg, wParam, lParam);
                        CatchDeviceChanged(deviceChangedData);
                    }
                    break;

                case (int)WM.WM_SETTINGCHANGE:
                    PlatformThemeLoader.WndProc_WM_SETTINGCHANGE(hwnd, msg, wParam, lParam, ref handled);
                    break;

                case (int)WM.WM_DWMCOLORIZATIONCOLORCHANGED:
                    PlatformThemeLoader.WndProc_WM_DWMCOLORIZATIONCOLORCHANGED(hwnd, msg, wParam, lParam, ref handled);
                    break;
            }

            return IntPtr.Zero;
        }

        private void InitializeSystem()
        {
            SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
            SystemEvents.SessionEnding += SystemEvents_SessionEnding;
            SystemEvents.DisplaySettingsChanging += SystemEvents_DisplaySettingsChanging;
        }

        private void UninitializeSystem()
        {
            SystemEvents.SessionSwitch -= SystemEvents_SessionSwitch;
            SystemEvents.SessionEnding -= SystemEvents_SessionEnding;
            SystemEvents.DisplaySettingsChanging -= SystemEvents_DisplaySettingsChanging;
        }

        private void InitializeHook()
        {
            KeyActionAssistant.SelfJobInputId = KeyActionChecker.SelfJobInputId;

            var builder = ApplicationDiContainer.Build<KeyActionFactory>();

            KeyboradHooker.KeyDown += KeyboradHooker_KeyDown;
            KeyboradHooker.KeyUp += KeyboradHooker_KeyUp;

            RebuildHook();
        }

        private void RebuildHook()
        {
            var keyActionFactory = ApplicationDiContainer.Build<KeyActionFactory>();
            KeyActionChecker.ReplaceJobs.SetRange(keyActionFactory.CreateReplaceJobs());
            KeyActionChecker.DisableJobs.SetRange(keyActionFactory.CreateDisableJobs());
            KeyActionChecker.PressedJobs.SetRange(keyActionFactory.CreatePressedJobs());
        }

        private void DisposeHook()
        {
            KeyboradHooker.KeyDown -= KeyboradHooker_KeyDown;
            KeyboradHooker.KeyUp -= KeyboradHooker_KeyUp;

            KeyboradHooker.Dispose();
            MouseHooker.Dispose();
        }

        void StartHook()
        {
            var hooked = false;
            if(KeyActionChecker.HasJob) {
                KeyboradHooker.Register();
                hooked = true;
            }
            //MouseHooker.Register();

            IsEnabledHook = hooked;
        }

        void StopHook()
        {
            KeyboradHooker.Unregister();
            MouseHooker.Unregister();

            IsEnabledHook = false;
        }

        public void ToggleHook()
        {
            if(IsEnabledHook) {
                StopHook();
            } else {
                StartHook();
            }
        }


        void ExecuteKeyPressedJob(KeyActionPressedJobBase job)
        {
            switch(job) {
                case KeyActionCommandJob commandJob: {
                        Logger.LogInformation("キーからの起動: コマンドランチャー");
                        ApplicationDiContainer.Get<IDispatcherWrapper>().Begin(() => {
                            ShowCommandView();
                        });
                    }
                    break;

                case KeyActionLauncherItemJob launcherItemJob: {
                        Logger.LogInformation("キーからの起動: アイテム = {0}, キー = {1}", launcherItemJob.PressedData.LauncherItemId, launcherItemJob.CommonData.KeyActionId);

                        NativeMethods.GetCursorPos(out var podPoint);
                        var deviceCursorLocation = PodStructUtility.Convert(podPoint);
                        var screen = Screen.FromDevicePoint(deviceCursorLocation);
                        var element = GetOrCreateLauncherItemElement(launcherItemJob.PressedData.LauncherItemId);
                        switch(launcherItemJob.PressedData.LauncherItemKind) {
                            case KeyActionContentLauncherItem.Execute:
                                element.Execute(screen);
                                break;
                            case KeyActionContentLauncherItem.ExtendsExecute:
                                element.OpenExtendsExecuteView(screen);
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                    }
                    break;

                case KeyActionLauncherToolbarJob launcherToolbarJob: {
                        ApplicationDiContainer.Get<IDispatcherWrapper>().Begin(() => {
                            var windowItems = WindowManager.GetWindowItems(WindowKind.LauncherToolbar);
                            foreach(var windowItem in windowItems) {
                                var viewModel = (ViewModels.LauncherToolbar.LauncherToolbarViewModel)windowItem.ViewModel;
                                if(viewModel.IsVisible && viewModel.IsAutoHide) {
                                    viewModel.AppDesktopToolbarExtend!.HideView(true);
                                }
                            }
                        });
                    }
                    break;

                case KeyActionNoteJob noteJob: {
                        switch(noteJob.PressedData.NoteKind) {
                            case KeyActionContentNote.Create:
                                var deviceCursorPos = MouseUtility.GetDevicePosition();
                                var screen = Screen.FromDevicePoint(deviceCursorPos);
                                var noteElement = CreateNote(screen, NoteStartupPosition.CursorPosition);

                                ApplicationDiContainer.Get<IDispatcherWrapper>().Begin(() => {
                                    noteElement.StartView();
                                }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                                break;

                            case KeyActionContentNote.ZOrderTop:
                                ApplicationDiContainer.Get<IDispatcherWrapper>().Begin(() => {
                                    MoveZOrderAllNotes(true);
                                }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                                break;

                            case KeyActionContentNote.ZOrderBottom:
                                ApplicationDiContainer.Get<IDispatcherWrapper>().Begin(() => {
                                    MoveZOrderAllNotes(false);
                                }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                                break;

                            default:
                                throw new NotImplementedException();
                        }

                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        Task ExecuteKeyDownJobsAsync(IReadOnlyCollection<KeyActionJobBase> jobs, in ModifierKeyStatus modifierKeyStatus)
        {
            var localModifierKeyStatus = modifierKeyStatus;
            return Task.Run(() => {
                foreach(var job in jobs) {
                    switch(job.CommonData.KeyActionKind) {
                        case KeyActionKind.Replace: {
                                var replaceJob = (KeyActionReplaceJob)job;
                                KeyActionAssistant.ExecuteReplaceJob(replaceJob, localModifierKeyStatus);
                            }
                            break;

                        case KeyActionKind.Disable:
                            // なんもしないです。。。
                            break;

                        default:
                            if(job is KeyActionPressedJobBase pressedJob) {
                                if(!pressedJob.IsAllHit) {
                                    Logger.LogTrace("待機中: {0}", job.CommonData.KeyActionId);
                                    break;
                                }
                                pressedJob.Reset();
                                ExecuteKeyPressedJob(pressedJob);
                                break;
                            } else {
                                throw new NotImplementedException();
                            }
                    }
                }
            });
        }

        Task ExecuteKeyUpJobsAsync(IReadOnlyCollection<KeyActionJobBase> jobs, Key key, in ModifierKeyStatus modifierKeyStatus)
        {
            var localModifierKeyStatus = modifierKeyStatus;
            return Task.Run(() => {
                KeyActionAssistant.CleanupReplaceJob(key, localModifierKeyStatus);

                foreach(var job in jobs) {
                    // 何もやることはないハズ
                }
            });
        }

        /// <summary>
        /// システムへ通知する必要があるか。
        /// </summary>
        /// <param name="jobs"></param>
        /// <returns></returns>
        bool IsThroughSystem(IReadOnlyCollection<KeyActionJobBase> jobs)
        {
            Debug.Assert(0 < jobs.Count);

            if(jobs.Any(i => i.CommonData.KeyActionKind == KeyActionKind.Disable)) {
                return false;
            }

            return jobs.OfType<KeyActionPressedJobBase>().Any(i => i.ThroughSystem);
        }

        void CatchDeviceChanged(DeviceChangedData deviceChangedData)
        {
            Logger.LogInformation("デバイス状態検知: {0}", deviceChangedData.DBT);

            // デバイス状態が変更されたか
            if(deviceChangedData.DBT == DBT.DBT_DEVNODES_CHANGED /*&& Initialized && !IsPause*/) {
                // デバイス変更前のスクリーン数が異なっていればディスプレイの抜き差しが行われたと判定する

                // 変更通知から現在数をAPIでまともに取得する
                var rawScreenCount = NativeMethods.GetSystemMetrics(SM.SM_CMONITORS);
                if(LauncherToolbarElements.Count != rawScreenCount) {
                    // 数が変わってりゃ待機
                    Logger.LogInformation("ディスプレイ数変更検知: WindowsAPI = {0}, Toolbar = {1}", rawScreenCount, LauncherToolbarElements.Count);
                    var environmentParameters = ApplicationDiContainer.Get<EnvironmentParameters>();

                    CloseViews();
                    DisposeElements();

                    Task.Run(() => {
                        // Forms で取得するディスプレイ数の合計値は少し遅れる
                        int waitMax = environmentParameters.Configuration.Display.ChangedRetryCount;
                        int waitCount = 0;

                        var managedScreenCount = Screen.AllScreens.Length;
                        while(rawScreenCount != managedScreenCount) {
                            if(waitMax < ++waitCount) {
                                // タイムアウト
                                Logger.LogWarning("ディスプレイ数変更検知: タイムアウト");
                                break;
                            }
                            Thread.Sleep(environmentParameters.Configuration.Display.ChangedRetryWaitTime);
                            managedScreenCount = Screen.AllScreens.Length;
                        }

                    }).ContinueWith(t => {
                        ExecuteElements();
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
        }

        private void StartPlatform()
        {
            var mainDatabaseBarrier = ApplicationDiContainer.Build<IMainDatabaseBarrier>();
            SettingAppPlatformSettingData setting;
            using(var commander = mainDatabaseBarrier.WaitRead()) {
                var appPlatformSettingEntityDao = ApplicationDiContainer.Build<AppPlatformSettingEntityDao>(commander, commander.Implementation);
                setting = appPlatformSettingEntityDao.SelectSettingPlatformSetting();
            }
            if(setting.SuppressSystemIdle) {
                StartDisableSystemIdle();
            }
            if(setting.SupportExplorer) {
                StartSupportExplorer();
            }
        }

        private void StopPlatform()
        {
            if(HeartBeatSender != null) {
                StopDisableSystemIdle();
            }
            if(ExplorerSupporter != null) {
                StopSupportExplorer();
            }
        }

        private void StartDisableSystemIdle()
        {
            HeartBeatSender = new HeartBeatSender(TimeSpan.FromSeconds(40), LoggerFactory);
            HeartBeatSender.Start();
        }
        private void StopDisableSystemIdle()
        {
            Debug.Assert(HeartBeatSender != null);

            HeartBeatSender.Dispose();
            HeartBeatSender = null;
        }

        public void ToggleDisableSystemIdle()
        {
            if(HeartBeatSender != null) {
                Logger.LogInformation("ロック抑制終了");
                StopDisableSystemIdle();
            } else {
                Logger.LogInformation("ロック抑制開始");
                StartDisableSystemIdle();
            }
        }

        private void StartSupportExplorer()
        {
            ExplorerSupporter = new ExplorerSupporter(TimeSpan.FromMilliseconds(800), LoggerFactory);
            ExplorerSupporter.Refresh();
            ExplorerSupporter.Start();
        }

        private void StopSupportExplorer()
        {
            Debug.Assert(ExplorerSupporter != null);

            ExplorerSupporter.Dispose();
            ExplorerSupporter = null;
        }

        public void ToggleSupportExplorer()
        {
            if(ExplorerSupporter != null) {
                Logger.LogInformation("Explorer 補正終了");
                StopSupportExplorer();
            } else {
                Logger.LogInformation("Explorer 補正開始");
                StartSupportExplorer();
            }
        }

        #endregion

        private void KeyboradHooker_KeyDown(object? sender, KeyboardHookEventArgs e)
        {
            var jobs = KeyActionChecker.Find(e.IsDown, e.Key, e.modifierKeyStatus, e.kbdll);
            if(0 < jobs.Count) {
                e.Handled = !IsThroughSystem(jobs);
                ExecuteKeyDownJobsAsync(jobs, e.modifierKeyStatus).ConfigureAwait(false);
            }
        }

        private void KeyboradHooker_KeyUp(object? sender, KeyboardHookEventArgs e)
        {
            var jobs = KeyActionChecker.Find(e.IsDown, e.Key, new ModifierKeyStatus(), e.kbdll);
            ExecuteKeyUpJobsAsync(jobs, e.Key, e.modifierKeyStatus).ConfigureAwait(false);
        }

        void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {
            Logger.LogInformation("セッション終了検知: Reason = {0}, Cancel = {1}", e.Reason, e.Cancel);

            CloseViews();
            DisposeElements();
            BackupSettingsDefault();
        }


        void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            Logger.LogInformation("セッション変更検知: Reason = {0}", e.Reason);

            if(e.Reason == SessionSwitchReason.ConsoleConnect || e.Reason == SessionSwitchReason.SessionUnlock) {
                ResetScreenViewElements();
                if(e.Reason == SessionSwitchReason.SessionUnlock) {
                    // アップデート処理とかとか
                }
            } else if(e.Reason == SessionSwitchReason.ConsoleDisconnect) {
                BackupSettingsDefault();
            }
        }

        void SystemEvents_DisplaySettingsChanging(object? sender, EventArgs e)
        {
            Logger.LogInformation("ディスプレイ変更検知");

            ResetScreenViewElements();
        }


    }
}
