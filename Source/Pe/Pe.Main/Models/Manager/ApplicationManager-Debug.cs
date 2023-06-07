#pragma warning disable S3261 // Namespaces should not be empty

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Standard.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element._Debug_;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using ContentTypeTextNet.Pe.Main.ViewModels._Debug_;
using ContentTypeTextNet.Pe.Main.Views;
using ContentTypeTextNet.Pe.Main.Views._Debug_;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
#if DEBUG
    partial class ApplicationManager
    {
        #region property

        private bool IsDevDebug { get; } = !true;

        #endregion


        #region function

        private void DebugExecuteBefore()
        {
            Logger.LogDebug("デバッグ用前処理");

            //DebugColorPicker();
            //DebugIssue714();
            //Exit(true);
        }

        private void DebugExecuteAfter()
        {
            Logger.LogDebug("デバッグ用後処理");

            //DebugCustomize();
            //DebugExtendsExecute();
            //DebugStdIoExecute();
            //DebugHook();
            //DebugKeyAction();
            //DebugSetting();
            //ShowCommandView();
            //ShowAboutView();
            //DebugEnvironmentExecuteFile();
            //ShowFeedbackView();
            //Uninstall();

            //Exit(true);
        }

        private void DebugCustomize()
        {
            // LauncherGroups.Sequence を調整すること
            var i = LauncherToolbarElements.First().LauncherItems.FirstOrDefault();
            if(i != null) {
                i.OpenCustomizeView(Screen.PrimaryScreen!);
            }
        }

        private void DebugExtendsExecute()
        {
            var i = LauncherToolbarElements.First().LauncherItems.FirstOrDefault();
            if(i != null) {
                i.OpenExtendsExecuteView(Screen.PrimaryScreen!);
            }
        }

        private void DebugStdIoExecute()
        {
            var batchPath = @".\temp.bat";
            File.WriteAllText(batchPath, @"
echo test1
echo test2
ping localhost
rem ping 127.0.0.1
rem ping localhost
rem ping 127.0.0.1
rem ping localhost
rem ping 127.0.0.1
rem ping localhost
rem ping 127.0.0.1
test
echo end
            ", Encoding.GetEncoding("shift_jis"));
            var launcherExecutor = new LauncherExecutor(EnvironmentPathExecuteFileCache.Instance, OrderManager, NotifyManager, new ApplicationDispatcherWrapper(Timeout.InfiniteTimeSpan), LoggerFactory);
            var data = new LauncherFileData() {
                //Path = batchPath,
                Path = "cmd",
                Option = "/c " + batchPath,
                IsEnabledStandardInputOutput = true,
            };
            var env = new List<LauncherEnvironmentVariableData>();
            var result = launcherExecutor.Execute(LauncherItemKind.File, data, data, env, LauncherRedoData.GetDisable(), Screen.PrimaryScreen ?? throw new InvalidOperationException("Screen.PrimaryScreen is null"));
        }
        
        private KeyboardHooker? dbgKeyboardHooker { get; set; }
        private MouseHooker? dbgMouseHooker { get; set; }
        private void DebugHook()
        {
            dbgKeyboardHooker = new KeyboardHooker(LoggerFactory);
            dbgKeyboardHooker.KeyDown += (sender, e) => {
                Logger.LogTrace("UP: key = {0}, mods = {1}, {2}", e.Key, e.modifierKeyStatus, e.kbdll);
            };
            dbgKeyboardHooker.KeyUp += (sender, e) => {
                Logger.LogTrace("DW: key = {0}, mods = {1}, {2}", e.Key, e.modifierKeyStatus, e.kbdll);
            };
            dbgKeyboardHooker.Register();

            dbgMouseHooker = new MouseHooker(LoggerFactory);
            //dbgMouseHooker.Register();
        }

        private KeyActionChecker? dbgKeyActionChecker { get; set; }
        private KeyActionAssistant? dbgKeyActionAssistant { get; set; }
        private void DebugKeyAction()
        {
            dbgKeyActionChecker = new KeyActionChecker(LoggerFactory);
            dbgKeyActionAssistant = new KeyActionAssistant(LoggerFactory);
            dbgKeyActionAssistant.SelfJobInputId = dbgKeyActionChecker.SelfJobInputId;

            dbgKeyActionChecker.DisableJobs.Add(new KeyActionDisableJob(
                new KeyActionDisableData(KeyActionId.NewId(), false),
                new KeyMappingData() {
                    Key = System.Windows.Input.Key.X,
                }
            ));
            dbgKeyActionChecker.DisableJobs.Add(new KeyActionDisableJob(
                new KeyActionDisableData(KeyActionId.NewId(), false),
                new KeyMappingData() {
                    Key = System.Windows.Input.Key.L,
                }
            ));
            dbgKeyActionChecker.DisableJobs.Add(new KeyActionDisableJob(
                new KeyActionDisableData(KeyActionId.NewId(), false),
                new KeyMappingData() {
                    Key = System.Windows.Input.Key.E,
                    Super = ModifierKey.Any,
                }
            ));
            dbgKeyActionChecker.ReplaceJobs.Add(new KeyActionReplaceJob(
                new KeyActionReplaceData(KeyActionId.NewId(), Key.LeftShift),
                new KeyMappingData() {
                    Key = System.Windows.Input.Key.LeftCtrl,
                }
            ));
            dbgKeyActionChecker.ReplaceJobs.Add(new KeyActionReplaceJob(
                new KeyActionReplaceData(KeyActionId.NewId(), Key.X),
                new KeyMappingData() {
                    Key = System.Windows.Input.Key.RightShift,
                }
            ));

            dbgKeyboardHooker = new KeyboardHooker(LoggerFactory);
            dbgKeyboardHooker.KeyDown += (sender, e) => {
                var jobs = dbgKeyActionChecker.Find(true, e.Key, e.modifierKeyStatus, e.kbdll);
                if(jobs.Any()) {
                    e.Handled = true;
                    Task.Run(() => {
                        foreach(var job in jobs) {
                            if(job.CommonData.KeyActionKind == KeyActionKind.Replace) {
                                var replaceJob = (KeyActionReplaceJob)job;
                                dbgKeyActionAssistant.ExecuteReplaceJob(replaceJob, e.modifierKeyStatus);
                            }
                        }
                    });
                }
            };
            dbgKeyboardHooker.KeyUp += (sender, e) => {
                var jobs = dbgKeyActionChecker.Find(true, e.Key, new ModifierKeyStatus(), e.kbdll);
                Task.Run(() => {
                    dbgKeyActionAssistant.CleanupReplaceJob(e.Key, e.modifierKeyStatus);

                    foreach(var job in jobs) {
                        // 何もやることはないハズ
                    }
                });
            };
            dbgKeyboardHooker.Register();
        }

        private void DebugSetting()
        {
            ShowSettingView();
        }
        private void DebugColorPicker()
        {
            using(var di = ApplicationDiContainer.CreateChildContainer()) {
                di.RegisterMvvm<DebugColorPickerElement, DebugColorPickerViewModel, DebugColorPickerWindow>();
                var model = di.Build<DebugColorPickerElement>();
                var view = di.Build<DebugColorPickerWindow>();
                var windowItem = new WindowItem(WindowKind.Debug, model, view);
                WindowManager.Register(windowItem);
                view.ShowDialog();
            }
        }

        private void DebugEnvironmentExecuteFile()
        {
            var eef = ApplicationDiContainer.Build<Platform.EnvironmentExecuteFile>();
            var pef = eef.GetPathExecuteFiles();

            var cmd = eef.Get("cmd", pef);
            var powershell = eef.Get("powershell", pef);
            var pwsh = eef.Get("pwsh", pef);
        }

        public void DebugStartupEnd()
        {
            //DebugSetting();
        }

        private void Uninstall()
        {
            var about = ApplicationDiContainer.Build<Element.About.AboutElement>();
            var path = @"x:a.bat";
            var uninstallTarget = UninstallTarget.Application | UninstallTarget.Batch;
            uninstallTarget |= UninstallTarget.User | UninstallTarget.Machine | UninstallTarget.Temporary;
            about.CreateUninstallBatch(path, uninstallTarget);
        }

        private void DebugIssue714()
        {
            var panel = new StackPanel();
            panel.Children.Add(new EllipsisTextBlock() {
                Text = "abc def ghi jkl mno",
                Background = new SolidColorBrush(Colors.Red),
            });
            panel.Children.Add(new EllipsisTextBlock() {
                Text = "あいうえおかきくけこさしすせそ",
                Background = new SolidColorBrush(Colors.Lime),
            });
            panel.Children.Add(new EllipsisTextBlock() {
                Text = @"abc\def\ghi\jkl\mno",
                Background = new SolidColorBrush(Colors.Yellow),
            });
            panel.Children.Add(new EllipsisTextBlock() {
                Text = @"あいう\えおか\きくけ\こさし\すせそ",
                Background = new SolidColorBrush(Colors.Green),
            });
            panel.Children.Add(new EllipsisTextBlock() {
                Text = @"abc/def/ghi/jkl/mno",
                Background = new SolidColorBrush(Colors.Peru),
                FontSize = 20,
            });
            panel.Children.Add(new EllipsisTextBlock() {
                Text = "あいう/えおか/きくけ/こさし/すせそ",
                Background = new SolidColorBrush(Colors.Pink),
                FontSize = 20,
            });

            var output = new EllipsisTextBlock() {
                Foreground = new SolidColorBrush(Colors.White),
                Background = new SolidColorBrush(Colors.Black),
                FontSize = 20,
            };
            var input = new TextBox() {
                Name = "input",
            };
            output.SetBinding(EllipsisTextBlock.TextProperty, new Binding("Text") {
                //ElementName = "input",
                Source = input,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                //Mode = BindingMode.OneWay,
            });
            panel.Children.Add(output);
            panel.Children.Add(input);

            var window = new Window() {
                Content = panel,
                Width = 400,
                Height = 200,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
            };
            window.ShowDialog();
        }

        #endregion
    }
#endif
}
