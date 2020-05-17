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
using ContentTypeTextNet.Pe.Core.Models.DependencyInjection;
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

        Guid KeyboardNotifyLogId { get; set; }

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

            MouseHooker.MouseMove += MouseHooker_MouseMove;
            MouseHooker.MouseDown += MouseHooker_MouseDown;
            MouseHooker.MouseUp += MouseHooker_MouseUp;

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

            MouseHooker.MouseMove -= MouseHooker_MouseMove;
            MouseHooker.MouseDown -= MouseHooker_MouseDown;
            MouseHooker.MouseUp -= MouseHooker_MouseUp;

            KeyboradHooker.Dispose();
            MouseHooker.Dispose();
        }

        void StartHook()
        {
            var hookConfiguration = ApplicationDiContainer.Build<HookConfiguration>();

            var hooked = false;
            if(hookConfiguration.Keyboard) {
                if(KeyActionChecker.HasJob) {
                    KeyboradHooker.Register();
                    hooked = true;
                }
            } else {
                Logger.LogInformation("キーボードフックは無効");
            }

            if(hookConfiguration.Mouse) {
                MouseHooker.Register();
                hooked = true;
            } else {
                Logger.LogInformation("マウスフックは無効");
            }

            IsEnabledHook = hooked;
        }

        void StopHook()
        {
            if(KeyboradHooker.IsEnabled) {
                KeyboradHooker.Unregister();
            }
            if(MouseHooker.IsEnabled) {
                MouseHooker.Unregister();
            }

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
            void PutNotifyLog(string message)
            {
                if(KeyboardNotifyLogId != Guid.Empty) {
                    NotifyManager.ClearLog(KeyboardNotifyLogId);
                    KeyboardNotifyLogId = Guid.Empty;
                }
                NotifyManager.AppendLog(new NotifyMessage(NotifyLogKind.Normal, Properties.Resources.String_Hook_Keyboard_Header, new NotifyLogContent(message)));
            }

            switch(job) {
                case KeyActionCommandJob commandJob: {
                        Logger.LogInformation("キーからの起動: コマンドランチャー");
                        PutNotifyLog(Properties.Resources.String_Hook_Keyboard_Execute_Command_Show);
                        ApplicationDiContainer.Get<IDispatcherWrapper>().Begin(() => {
                            ShowCommandView();
                        }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                    }
                    break;

                case KeyActionLauncherItemJob launcherItemJob: {
                        Logger.LogInformation("キーからの起動: アイテム = {0}, キー = {1}", launcherItemJob.PressedData.LauncherItemId, launcherItemJob.CommonData.KeyActionId);

                        NativeMethods.GetCursorPos(out var podPoint);
                        var deviceCursorLocation = PodStructUtility.Convert(podPoint);
                        var screen = Screen.FromDevicePoint(deviceCursorLocation);
                        var element = GetOrCreateLauncherItemElement(launcherItemJob.PressedData.LauncherItemId);
                        var map = new Dictionary<string, string>() {
                            ["ITEM"] = element.Name,
                        };
                        switch(launcherItemJob.PressedData.LauncherItemKind) {
                            case KeyActionContentLauncherItem.Execute:
                                PutNotifyLog(TextUtility.ReplaceFromDictionary(Properties.Resources.String_Hook_Keyboard_Execute_LauncherItem_Normal_Format, map));
                                element.Execute(screen);
                                break;
                            case KeyActionContentLauncherItem.ExtendsExecute:
                                PutNotifyLog(TextUtility.ReplaceFromDictionary(Properties.Resources.String_Hook_Keyboard_Execute_LauncherItem_Extends_Format, map));
                                element.OpenExtendsExecuteView(screen);
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                    }
                    break;

                case KeyActionLauncherToolbarJob launcherToolbarJob: {
                        PutNotifyLog(Properties.Resources.String_Hook_Keyboard_Execute_Toolbar_Hidden);

                        ApplicationDiContainer.Get<IDispatcherWrapper>().Begin(() => {
                            var windowItems = WindowManager.GetWindowItems(WindowKind.LauncherToolbar);
                            foreach(var windowItem in windowItems) {
                                var viewModel = (ViewModels.LauncherToolbar.LauncherToolbarViewModel)windowItem.ViewModel;
                                viewModel.HideAndShowWaiting();
                            }
                        }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                    }
                    break;

                case KeyActionNoteJob noteJob: {
                        switch(noteJob.PressedData.NoteKind) {
                            case KeyActionContentNote.Create:
                                PutNotifyLog(Properties.Resources.String_Hook_Keyboard_Execute_Note_Create);

                                var deviceCursorPos = MouseUtility.GetDevicePosition();
                                var screen = Screen.FromDevicePoint(deviceCursorPos);
                                var noteElement = CreateNote(screen, NoteStartupPosition.CursorPosition);

                                ApplicationDiContainer.Get<IDispatcherWrapper>().Begin(() => {
                                    noteElement.StartView();
                                }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                                break;

                            case KeyActionContentNote.ZOrderTop:
                                PutNotifyLog(Properties.Resources.String_Hook_Keyboard_Execute_Note_Z_Top);

                                ApplicationDiContainer.Get<IDispatcherWrapper>().Begin(() => {
                                    MoveZOrderAllNotes(true);
                                }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                                break;

                            case KeyActionContentNote.ZOrderBottom:
                                PutNotifyLog(Properties.Resources.String_Hook_Keyboard_Execute_Note_Z_Bottom);

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
                KeyActionPressedJobBase? firstWaitingPressedJob = null;

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
                                    firstWaitingPressedJob = pressedJob;
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

                if(firstWaitingPressedJob != null) {
                    var factory = new KeyMappingFactory();
                    var cultureService = ApplicationDiContainer.Get<CultureService>();
                    var keyMessages = firstWaitingPressedJob.GetCurrentMappings().Select(i => factory.ToString(CultureService.Instance, i, Properties.Resources.String_Hook_Keyboard_Join));
                    var keyMessage = string.Join(Properties.Resources.String_Hook_Keyboard_Separator, keyMessages);

                    if(KeyboardNotifyLogId == Guid.Empty) {
                        var logContent = new NotifyLogContent(keyMessage);
                        KeyboardNotifyLogId = NotifyManager.AppendLog(new NotifyMessage(NotifyLogKind.Topmost, Properties.Resources.String_Hook_Keyboard_Header, logContent));
                    } else {
                        NotifyManager.ReplaceLog(KeyboardNotifyLogId, keyMessage);
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

                    DelayResetScreenViewElements();

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

                        DelayResetScreenViewElements();
                    });
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
            bool flag;
            if(HeartBeatSender != null) {
                Logger.LogInformation("ロック抑制終了");
                StopDisableSystemIdle();
                flag = false;
            } else {
                Logger.LogInformation("ロック抑制開始");
                StartDisableSystemIdle();
                flag = true;
            }

            var lazyWriter = ApplicationDiContainer.Build<IMainDatabaseLazyWriter>();
            lazyWriter.Stock(c => {
                var appPlatformSettingEntityDao = ApplicationDiContainer.Build<AppPlatformSettingEntityDao>(c, c.Implementation);
                appPlatformSettingEntityDao.UpdateSuppressSystemIdle(flag, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        private void StartSupportExplorer()
        {
            var customConfiguration = ApplicationDiContainer.Build<CustomConfiguration>();
            var platform = customConfiguration.Platform;

            ExplorerSupporter = ApplicationDiContainer.Build<ExplorerSupporter>(platform.ExplorerSupporterRefreshTime, platform.ExplorerSupporterCacheSize);
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
            bool flag;
            if(ExplorerSupporter != null) {
                Logger.LogInformation("Explorer 補正終了");
                StopSupportExplorer();
                flag = false;
            } else {
                Logger.LogInformation("Explorer 補正開始");
                StartSupportExplorer();
                flag = true;
            }

            var lazyWriter = ApplicationDiContainer.Build<IMainDatabaseLazyWriter>();
            lazyWriter.Stock(c => {
                var appPlatformSettingEntityDao = ApplicationDiContainer.Build<AppPlatformSettingEntityDao>(c, c.Implementation);
                appPlatformSettingEntityDao.UpdateSupportExplorer(flag, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        #endregion

        private void KeyboradHooker_KeyDown(object? sender, KeyboardHookEventArgs e)
        {
            var jobs = KeyActionChecker.Find(e.IsDown, e.Key, e.modifierKeyStatus, e.kbdll);
            if(0 < jobs.Count) {
                e.Handled = !IsThroughSystem(jobs);
                ExecuteKeyDownJobsAsync(jobs, e.modifierKeyStatus).ConfigureAwait(false);
            } else {
                if(KeyboardNotifyLogId != Guid.Empty) {
                    if(!e.Key.IsModifierKey()) {
                        Logger.LogTrace("キー入力該当なし");
                        NotifyManager.ClearLog(KeyboardNotifyLogId);
                        KeyboardNotifyLogId = Guid.Empty;
                        NotifyManager.AppendLog(new NotifyMessage(NotifyLogKind.Normal, Properties.Resources.String_Hook_Keyboard_Header, new NotifyLogContent(Properties.Resources.String_Hook_Keyboard_NotFound)));
                    }
                }
            }
        }

        private void KeyboradHooker_KeyUp(object? sender, KeyboardHookEventArgs e)
        {
            var jobs = KeyActionChecker.Find(e.IsDown, e.Key, new ModifierKeyStatus(), e.kbdll);
            ExecuteKeyUpJobsAsync(jobs, e.Key, e.modifierKeyStatus).ConfigureAwait(false);
        }

        private void MouseHooker_MouseMove(object? sender, MouseHookEventArgs e)
        {
            if(NotifyLogElement.NowShowing && NotifyLogElement.Position == NotifyLogPosition.Cursor) {
                // #636: 通知ログがカーソル位置指定で通知ログウィンドウにクリック可能なアイテムがある場合は常時追従してはいけない
                var hasCommandLog = NotifyManager.StreamNotifyLogs
                    .Concat(NotifyManager.TopmostNotifyLogs)
                    .Any(i => i.Kind == NotifyLogKind.Command || i.Kind == NotifyLogKind.Undo)
                ;
                if(!hasCommandLog) {
                    NotifyLogElement.StartView();
                }
            }
        }

        private void MouseHooker_MouseDown(object? sender, MouseHookEventArgs e)
        {
            Logger.LogTrace("キー入力待ちリセット");
            if(NotifyManager.ExistsLog(KeyboardNotifyLogId)) {
                NotifyManager.ClearLog(KeyboardNotifyLogId);
                KeyboardNotifyLogId = Guid.Empty;
            }
            KeyActionChecker.Reset();
        }

        private void MouseHooker_MouseUp(object? sender, MouseHookEventArgs e)
        { }


        void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {
            Logger.LogInformation("セッション終了検知: Reason = {0}, Cancel = {1}", e.Reason, e.Cancel);

            CloseViews();
            DisposeElements();
            BackupSettingsDefault(ApplicationDiContainer);
        }


        void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            Logger.LogInformation("セッション変更検知: Reason = {0}", e.Reason);
            // そろそろ switch すべきちゃうんかと

            if(e.Reason == SessionSwitchReason.ConsoleConnect || e.Reason == SessionSwitchReason.SessionUnlock) {
                DelayResetScreenViewElements();
                if(e.Reason == SessionSwitchReason.SessionUnlock) {
                    // アップデート処理とかとか
                    DelayCheckUpdateAsync().ConfigureAwait(false);
                }
            } else if(e.Reason == SessionSwitchReason.ConsoleDisconnect) {
                BackupSettingsDefault(ApplicationDiContainer);
            } else if(e.Reason == SessionSwitchReason.SessionLock) {
                // アップデート処理実施
                if(ApplicationUpdateInfo.IsReady) {
                    StartUpdate(UpdateTarget.Application, UpdateProcess.Update);
                }
            }
        }

        void SystemEvents_DisplaySettingsChanging(object? sender, EventArgs e)
        {
            Logger.LogInformation("ディスプレイ変更検知");

            DelayResetScreenViewElements();
        }


    }
}
