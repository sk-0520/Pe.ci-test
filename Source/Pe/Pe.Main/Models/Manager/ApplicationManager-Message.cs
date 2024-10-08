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
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Library.Base;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Addon;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using ContentTypeTextNet.Pe.Library.Base.Linq;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
    partial class ApplicationManager
    {
        #region property

        public bool IsEnabledHook { get; private set; }

        private HeartBeatSender? HeartBeatSender { get; set; }
        private ExplorerSupporter? ExplorerSupporter { get; set; }

        public bool IsDisabledSystemIdle => HeartBeatSender != null;
        public bool IsSupportedExplorer => ExplorerSupporter != null;

        private NotifyLogId KeyboardNotifyLogId { get; set; }
        private BackgroundAddonProxy? BackgroundAddon { get; set; }

        private CronScheduler CronScheduler { get; }
        /// <summary>
        /// 定周期処理。
        /// <see cref="CronScheduler"/>の間隔未満(1分より小さい)で優先度を特に考えなくていいスケジュール処理を実施。
        /// </summary>
        private System.Timers.Timer LowScheduler { get; }

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

        private IntPtr MessageWindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
#if DEBUG
            Logger.LogTrace("[MSG WND] hWnd = {0}, msg = {1}({2}), wParam = {3}, lParam = {4}", hWnd, msg, (WM)msg, wParam, lParam);
#endif

            switch(msg) {
                case (int)WM.WM_DEVICECHANGE: {
                        var deviceChangedData = new DeviceChangedData(hWnd, msg, wParam, lParam);
                        CatchDeviceChanged(deviceChangedData);
                    }
                    break;

                case (int)WM.WM_SETTINGCHANGE:
                    PlatformThemeLoader.WndProc_WM_SETTINGCHANGE(hWnd, msg, wParam, lParam, ref handled);
                    break;

                case (int)WM.WM_DWMCOLORIZATIONCOLORCHANGED:
                    PlatformThemeLoader.WndProc_WM_DWMCOLORIZATIONCOLORCHANGED(hWnd, msg, wParam, lParam, ref handled);
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

        private void FinalizeSystem()
        {
            SystemEvents.SessionSwitch -= SystemEvents_SessionSwitch;
            SystemEvents.SessionEnding -= SystemEvents_SessionEnding;
            SystemEvents.DisplaySettingsChanging -= SystemEvents_DisplaySettingsChanging;
        }

        private void InitializeHook()
        {
            KeyActionAssistant.SelfJobInputId = KeyActionChecker.SelfJobInputId;

            KeyboradHooker.KeyDown += KeyboardHooker_KeyDown;
            KeyboradHooker.KeyUp += KeyboardHooker_KeyUp;

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
            KeyboradHooker.KeyDown -= KeyboardHooker_KeyDown;
            KeyboradHooker.KeyUp -= KeyboardHooker_KeyUp;

            MouseHooker.MouseMove -= MouseHooker_MouseMove;
            MouseHooker.MouseDown -= MouseHooker_MouseDown;
            MouseHooker.MouseUp -= MouseHooker_MouseUp;

            KeyboradHooker.Dispose();
            MouseHooker.Dispose();
        }

        private void StartHook()
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

        private void StopHook()
        {
            if(KeyboradHooker.IsEnabled) {
                KeyboradHooker.Unregister();
            }
            if(MouseHooker.IsEnabled) {
                MouseHooker.Unregister();
            }

            IsEnabledHook = false;
        }

        private void StartBackground()
        {
            BackgroundAddon = PluginContainer.Addon.GetBackground();

            var backgroundAddonProxyRunStartupContext = new BackgroundAddonProxyRunStartupContext();
            BackgroundAddon.RunStartup(backgroundAddonProxyRunStartupContext);

            var hookConfiguration = ApplicationDiContainer.Build<HookConfiguration>();
            if(hookConfiguration.Keyboard) {
                if(BackgroundAddon.IsSupported(BackgroundKind.KeyboardHook)) {
                    if(!KeyboradHooker.IsEnabled) {
                        Logger.LogInformation("キーボード設定は存在しないがバックグラウンドアドオンでキーボード処理が存在するためキーフックを開始");
                        KeyboradHooker.Register();
                        IsEnabledHook = true;
                    }
                }
            }

            if(hookConfiguration.Mouse) {
                if(BackgroundAddon.IsSupported(BackgroundKind.MouseHook)) {
                    if(!MouseHooker.IsEnabled) {
                        Logger.LogInformation("バックグラウンドアドオンでマウス処理が存在するためマウスフックを開始");
                        MouseHooker.Register();
                        IsEnabledHook = true;
                    }
                }
            }
        }

        public void ToggleHook()
        {
            if(IsEnabledHook) {
                StopHook();
            } else {
                StartHook();
                StartBackground();
            }
        }


        private async Task ExecuteKeyPressedJobAsync(KeyActionPressedJobBase job, CancellationToken cancellationToken)
        {
            void PutNotifyLog(string message)
            {
                if(KeyboardNotifyLogId != NotifyLogId.Empty) {
                    NotifyManager.ClearLog(KeyboardNotifyLogId);
                    KeyboardNotifyLogId = NotifyLogId.Empty;
                }
                NotifyManager.AppendLogAsync(new NotifyMessage(NotifyLogKind.Normal, Properties.Resources.String_Hook_Keyboard_Header, new NotifyLogContent(message)), cancellationToken);
            }

            switch(job) {
                case KeyActionCommandJob commandJob: {
                        Logger.LogInformation("キーからの起動: コマンドランチャー");
                        PutNotifyLog(Properties.Resources.String_Hook_Keyboard_Execute_Command_Show);
                        await ApplicationDiContainer.Get<IDispatcherWrapper>().BeginAsync(async () => {
                            await ShowCommandViewAsync(cancellationToken);
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
                                await element.ExecuteAsync(screen, cancellationToken);
                                break;
                            case KeyActionContentLauncherItem.ExtendsExecute:
                                PutNotifyLog(TextUtility.ReplaceFromDictionary(Properties.Resources.String_Hook_Keyboard_Execute_LauncherItem_Extends_Format, map));
                                await element.OpenExtendsExecuteViewAsync(screen, cancellationToken);
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                    }
                    break;

                case KeyActionLauncherToolbarJob launcherToolbarJob: {
                        PutNotifyLog(Properties.Resources.String_Hook_Keyboard_Execute_Toolbar_Hidden);

                        await ApplicationDiContainer.Get<IDispatcherWrapper>().BeginAsync(() => {
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
                                var noteElement = await CreateNoteAsync(screen, NoteStartupPosition.CursorPosition, cancellationToken);

                                await ApplicationDiContainer.Get<IDispatcherWrapper>().BeginAsync(() => {
                                    noteElement.StartView();
                                }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                                break;

                            case KeyActionContentNote.ZOrderTop:
                                PutNotifyLog(Properties.Resources.String_Hook_Keyboard_Execute_Note_Z_Top);

                                await ApplicationDiContainer.Get<IDispatcherWrapper>().BeginAsync(() => {
                                    MoveZOrderAllNotes(true);
                                }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                                break;

                            case KeyActionContentNote.ZOrderBottom:
                                PutNotifyLog(Properties.Resources.String_Hook_Keyboard_Execute_Note_Z_Bottom);

                                await ApplicationDiContainer.Get<IDispatcherWrapper>().BeginAsync(() => {
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

        private Task ExecuteKeyDownJobsAsync(IReadOnlyCollection<KeyActionJobBase> jobs, in ModifierKeyStatus modifierKeyStatus, CancellationToken cancellationToken)
        {
            var localModifierKeyStatus = modifierKeyStatus;
            return Task.Run(async () => {
                void UpdateKeyExecuteCount(IKeyActionId keyActionId)
                {
                    var mainDatabaseDelayWriter = ApplicationDiContainer.Build<IMainDatabaseDelayWriter>();
                    mainDatabaseDelayWriter.Stock(c => {
                        var dao = ApplicationDiContainer.Build<KeyActionsEntityDao>(c, c.Implementation);
                        dao.UpdateUsageCountIncrement(keyActionId.KeyActionId, DatabaseCommonStatus.CreateCurrentAccount());
                    });
                }

                KeyActionPressedJobBase? firstWaitingPressedJob = null;

                foreach(var job in jobs) {
                    switch(job.CommonData.KeyActionKind) {
                        case KeyActionKind.Replace: {
                                var replaceJob = (KeyActionReplaceJob)job;
                                KeyActionAssistant.ExecuteReplaceJob(replaceJob, localModifierKeyStatus);
                                UpdateKeyExecuteCount(job.CommonData);
                            }
                            break;

                        case KeyActionKind.Disable:
                            // なんもしないです。。。
                            UpdateKeyExecuteCount(job.CommonData);
                            break;

                        default:
                            if(job is KeyActionPressedJobBase pressedJob) {
                                if(!pressedJob.IsAllHit) {
                                    Logger.LogTrace("待機中: {0}", job.CommonData.KeyActionId);
                                    firstWaitingPressedJob = pressedJob;
                                    break;
                                }

                                pressedJob.Reset();
                                await ExecuteKeyPressedJobAsync(pressedJob, cancellationToken);
                                UpdateKeyExecuteCount(job.CommonData);

                                break;
                            } else {
                                throw new NotImplementedException();
                            }
                    }
                }

                if(firstWaitingPressedJob != null) {
                    var factory = new KeyMappingFactory();
                    var cultureService = ApplicationDiContainer.Get<ICultureService>();
                    var keyMessages = firstWaitingPressedJob.GetCurrentMappings().Select(i => factory.ToString(cultureService, i, Properties.Resources.String_Hook_Keyboard_Join));
                    var keyMessage = string.Join(Properties.Resources.String_Hook_Keyboard_Separator, keyMessages);

                    if(KeyboardNotifyLogId == NotifyLogId.Empty) {
                        var logContent = new NotifyLogContent(keyMessage);
                        KeyboardNotifyLogId = await NotifyManager.AppendLogAsync(new NotifyMessage(NotifyLogKind.Topmost, Properties.Resources.String_Hook_Keyboard_Header, logContent), cancellationToken);
                    } else {
                        NotifyManager.ReplaceLog(KeyboardNotifyLogId, keyMessage);
                    }
                }
            }, cancellationToken);
        }

        private Task ExecuteKeyUpJobsAsync(IReadOnlyCollection<KeyActionJobBase> jobs, Key key, in ModifierKeyStatus modifierKeyStatus)
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
        private bool IsThroughSystem(IReadOnlyCollection<KeyActionJobBase> jobs)
        {
            Debug.Assert(0 < jobs.Count);

            if(jobs.Any(i => i.CommonData.KeyActionKind == KeyActionKind.Disable)) {
                return false;
            }

            return jobs.OfType<KeyActionPressedJobBase>().Any(i => i.ThroughSystem);
        }

        private void CatchDeviceChanged(DeviceChangedData deviceChangedData)
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
                    var displayConfiguration = ApplicationDiContainer.Get<DisplayConfiguration>();

                    Task.Run(() => {
                        // Forms で取得するディスプレイ数の合計値は少し遅れる
                        int waitMax = displayConfiguration.ChangedRetryCount;
                        int waitCount = 0;

                        var managedScreenCount = Screen.AllScreens.Length;
                        while(rawScreenCount != managedScreenCount) {
                            if(waitMax < ++waitCount) {
                                // タイムアウト
                                Logger.LogWarning("ディスプレイ数変更検知: タイムアウト");
                                break;
                            }
                            Thread.Sleep(displayConfiguration.ChangedRetryWaitTime);
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
            using(var context = mainDatabaseBarrier.WaitRead()) {
                var appPlatformSettingEntityDao = ApplicationDiContainer.Build<AppPlatformSettingEntityDao>(context, context.Implementation);
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
            var platformConfiguration = ApplicationDiContainer.Build<PlatformConfiguration>();
            HeartBeatSender = new HeartBeatSender(platformConfiguration.IdleDisableCycleTime, platformConfiguration.IdleCheckCycleTime, LoggerFactory);
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

            var delayWriter = ApplicationDiContainer.Build<IMainDatabaseDelayWriter>();
            delayWriter.Stock(c => {
                var appPlatformSettingEntityDao = ApplicationDiContainer.Build<AppPlatformSettingEntityDao>(c, c.Implementation);
                appPlatformSettingEntityDao.UpdateSuppressSystemIdle(flag, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        private void StartSupportExplorer()
        {
            var customConfiguration = ApplicationDiContainer.Build<ApplicationConfiguration>();
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

            var delayWriter = ApplicationDiContainer.Build<IMainDatabaseDelayWriter>();
            delayWriter.Stock(c => {
                var appPlatformSettingEntityDao = ApplicationDiContainer.Build<AppPlatformSettingEntityDao>(c, c.Implementation);
                appPlatformSettingEntityDao.UpdateSupportExplorer(flag, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        public bool GetProxyIsEnabled()
        {
            var mainDatabaseBarrier = ApplicationDiContainer.Build<IMainDatabaseBarrier>();
            using var transaction = mainDatabaseBarrier.WaitRead();
            var appProxySettingEntityDao = ApplicationDiContainer.Build<AppProxySettingEntityDao>(transaction, transaction.Implementation);
            return appProxySettingEntityDao.SelectProxyIsEnabled();
        }

        public void ToggleProxyIsEnabled()
        {
            var mainDatabaseBarrier = ApplicationDiContainer.Build<IMainDatabaseBarrier>();
            using var transaction = mainDatabaseBarrier.WaitWrite();
            var appProxySettingEntityDao = ApplicationDiContainer.Build<AppProxySettingEntityDao>(transaction, transaction.Implementation);
            appProxySettingEntityDao.UpdateToggleProxyIsEnabled(DatabaseCommonStatus.CreateCurrentAccount());
            transaction.Commit();
        }

        private void InitializeScheduler()
        {
            RebuildSchedulerSetting();
        }

        private void RebuildSchedulerSetting()
        {
            CronScheduler.ClearAllSchedule();

            var factory = new CronItemSettingFactory();

            var launcherItemIconRefreshSetting = factory.Parse(ApplicationDiContainer.Build<ScheduleConfiguration>().LauncherItemIconRefresh);
            launcherItemIconRefreshSetting.Mode = MultipleExecuteMode.Skip;

            var launcherItemConfiguration = ApplicationDiContainer.Build<LauncherItemConfiguration>();

            CronScheduler.AddSchedule(launcherItemIconRefreshSetting, ApplicationDiContainer.Build<LauncherIconRefresher>(launcherItemConfiguration.IconRefreshTime));
        }

        private void StartScheduler()
        {
            Logger.LogInformation("スケジューラ実行");
            LowScheduler.Start();
            CronScheduler.Start();
        }

        private void StopScheduler()
        {
            LowScheduler.Stop();

            if(CronScheduler.IsRunning) {
                Logger.LogInformation("スケジューラ停止");
                CronScheduler.Stop();
            }
        }

        private void CheckFullScreenState()
        {
            var fullScreenWatcher = ApplicationDiContainer.Build<IFullscreenWatcher>();

            foreach(var screen in Screen.AllScreens) {
                var hWnd = fullScreenWatcher.GetFullscreenWindowHandle(screen);
                if(hWnd != IntPtr.Zero) {
                    NotifyManager.SendFullscreenChanged(screen, true, hWnd);
                } else {
                    NotifyManager.SendFullscreenChanged(screen, false, IntPtr.Zero);
                }
            }
        }

        #endregion

        private void KeyboardHooker_KeyDown(object? sender, KeyboardHookEventArgs e)
        {
            var jobs = KeyActionChecker.Find(e.IsDown, e.Key, e.modifierKeyStatus, e.kbdll);
            if(0 < jobs.Count) {
                e.Handled = !IsThroughSystem(jobs);
                ExecuteKeyDownJobsAsync(jobs, e.modifierKeyStatus, CancellationToken.None).ConfigureAwait(false);
            } else {
                if(KeyboardNotifyLogId != NotifyLogId.Empty) {
                    if(!e.Key.IsModifierKey()) {
                        Logger.LogTrace("キー入力該当なし");
                        NotifyManager.ClearLog(KeyboardNotifyLogId);
                        KeyboardNotifyLogId = NotifyLogId.Empty;
                        NotifyManager.AppendLogAsync(new NotifyMessage(NotifyLogKind.Normal, Properties.Resources.String_Hook_Keyboard_Header, new NotifyLogContent(Properties.Resources.String_Hook_Keyboard_NotFound)), CancellationToken.None);
                    }
                }
            }

            if(BackgroundAddon != null) {
                if(BackgroundAddon.IsSupported(Bridge.Plugin.Addon.BackgroundKind.KeyboardHook)) {
                    var context = new BackgroundAddonProxyKeyboardContext(e);
                    BackgroundAddon.HookKeyDown(context);
                }
            }
        }

        private void KeyboardHooker_KeyUp(object? sender, KeyboardHookEventArgs e)
        {
            var jobs = KeyActionChecker.Find(e.IsDown, e.Key, new ModifierKeyStatus(), e.kbdll);
            ExecuteKeyUpJobsAsync(jobs, e.Key, e.modifierKeyStatus).ConfigureAwait(false);

            if(BackgroundAddon != null) {
                if(BackgroundAddon.IsSupported(Bridge.Plugin.Addon.BackgroundKind.KeyboardHook)) {
                    var context = new BackgroundAddonProxyKeyboardContext(e);
                    BackgroundAddon.HookKeyUp(context);
                }
            }
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

            if(BackgroundAddon != null) {
                if(BackgroundAddon.IsSupported(BackgroundKind.MouseHook)) {
                    var context = new BackgroundAddonProxyMouseMoveContext(e);
                    BackgroundAddon.HookMouseMove(context);
                }
            }
        }

        private void MouseHooker_MouseDown(object? sender, MouseHookEventArgs e)
        {
            if(NotifyManager.ExistsLog(KeyboardNotifyLogId)) {
                NotifyManager.ClearLog(KeyboardNotifyLogId);
                KeyboardNotifyLogId = NotifyLogId.Empty;
                Logger.LogTrace("キー入力待ちリセット");
            }
            KeyActionChecker.Reset();

            if(BackgroundAddon != null) {
                if(BackgroundAddon.IsSupported(BackgroundKind.MouseHook)) {
                    var context = new BackgroundAddonProxyMouseButtonContext(e);
                    BackgroundAddon.HookMouseDown(context);
                }
            }
        }

        private void MouseHooker_MouseUp(object? sender, MouseHookEventArgs e)
        {
            if(BackgroundAddon != null) {
                if(BackgroundAddon.IsSupported(BackgroundKind.MouseHook)) {
                    var context = new BackgroundAddonProxyMouseButtonContext(e);
                    BackgroundAddon.HookMouseUp(context);
                }
            }
        }


        private void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {
            Logger.LogInformation("セッション終了検知: Reason = {0}, Cancel = {1}", e.Reason, e.Cancel);

            CloseViews(true);
            DisposeElements();
            BackupSettingsDefault(ApplicationDiContainer);
        }


        private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            Logger.LogInformation("セッション変更検知: Reason = {0}", e.Reason);
            // そろそろ switch すべきちゃうんかと

            if(e.Reason == SessionSwitchReason.ConsoleConnect || e.Reason == SessionSwitchReason.SessionUnlock) {
                DelayResetScreenViewElements();
                if(e.Reason == SessionSwitchReason.SessionUnlock) {
                    // アップデート処理とかとか
                    CheckNewVersionsAsync(true, CancellationToken.None).ConfigureAwait(false);
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

        private void SystemEvents_DisplaySettingsChanging(object? sender, EventArgs e)
        {
            Logger.LogInformation("ディスプレイ変更検知");

            DelayResetScreenViewElements();
        }

        private void LowScheduler_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            LowScheduler.Stop();
            if(StatusManager.CanCallNotifyAreaMenu) {
                CheckFullScreenState();
            }
            if(StatusManager.CanCallNotifyAreaMenu) {
                LowScheduler.Start();
            }
        }
    }
}
