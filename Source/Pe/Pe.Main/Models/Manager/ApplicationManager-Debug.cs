#pragma warning disable S3261 // Namespaces should not be empty

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element._Debug_;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using ContentTypeTextNet.Pe.Main.ViewModels._Debug_;
using ContentTypeTextNet.Pe.Main.Views._Debug_;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
#if DEBUG
    partial class ApplicationManager
    {
        #region property

        bool IsDevDebug { get; } = !true;

        #endregion


        #region function

        void DebugExecuteBefore()
        {
            Logger.LogDebug("デバッグ用前処理");

            //DebugColorPicker();
            DebugIssue714();
            Exit(true);
        }

        void DebugExecuteAfter()
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

        void DebugCustomize()
        {
            // LauncherGroups.Sequence を調整すること
            var i = LauncherToolbarElements.First().LauncherItems.FirstOrDefault();
            if(i != null) {
                i.OpenCustomizeView(Screen.PrimaryScreen);
            }
        }

        void DebugExtendsExecute()
        {
            var i = LauncherToolbarElements.First().LauncherItems.FirstOrDefault();
            if(i != null) {
                i.OpenExtendsExecuteView(Screen.PrimaryScreen);
            }
        }

        void DebugStdIoExecute()
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
            var result = launcherExecutor.Execute(LauncherItemKind.File, data, data, env, LauncherRedoData.GetDisable(), Screen.PrimaryScreen);
        }

        KeyboradHooker? dbgKeyboradHooker { get; set; }
        MouseHooker? dbgMouseHooker { get; set; }
        void DebugHook()
        {
            dbgKeyboradHooker = new KeyboradHooker(LoggerFactory);
            dbgKeyboradHooker.KeyDown += (sender, e) => {
                Logger.LogTrace("UP: key = {0}, mods = {1}, {2}", e.Key, e.modifierKeyStatus, e.kbdll);
            };
            dbgKeyboradHooker.KeyUp += (sender, e) => {
                Logger.LogTrace("DW: key = {0}, mods = {1}, {2}", e.Key, e.modifierKeyStatus, e.kbdll);
            };
            dbgKeyboradHooker.Register();

            dbgMouseHooker = new MouseHooker(LoggerFactory);
            //dbgMouseHooker.Register();
        }

        KeyActionChecker? dbgKeyActionChecker { get; set; }
        KeyActionAssistant? dbgKeyActionAssistant { get; set; }
        void DebugKeyAction()
        {
            dbgKeyActionChecker = new KeyActionChecker(LoggerFactory);
            dbgKeyActionAssistant = new KeyActionAssistant(LoggerFactory);
            dbgKeyActionAssistant.SelfJobInputId = dbgKeyActionChecker.SelfJobInputId;

            dbgKeyActionChecker.DisableJobs.Add(new KeyActionDisableJob(
                new KeyActionDisableData(Guid.NewGuid(), false),
                new KeyMappingData() {
                    Key = System.Windows.Input.Key.X,
                }
            ));
            dbgKeyActionChecker.DisableJobs.Add(new KeyActionDisableJob(
                new KeyActionDisableData(Guid.NewGuid(), false),
                new KeyMappingData() {
                    Key = System.Windows.Input.Key.L,
                }
            ));
            dbgKeyActionChecker.DisableJobs.Add(new KeyActionDisableJob(
                new KeyActionDisableData(Guid.NewGuid(), false),
                new KeyMappingData() {
                    Key = System.Windows.Input.Key.E,
                    Super = ModifierKey.Any,
                }
            ));
            dbgKeyActionChecker.ReplaceJobs.Add(new KeyActionReplaceJob(
                new KeyActionReplaceData(Guid.NewGuid(), Key.LeftShift),
                new KeyMappingData() {
                    Key = System.Windows.Input.Key.LeftCtrl,
                }
            ));
            dbgKeyActionChecker.ReplaceJobs.Add(new KeyActionReplaceJob(
                new KeyActionReplaceData(Guid.NewGuid(), Key.X),
                new KeyMappingData() {
                    Key = System.Windows.Input.Key.RightShift,
                }
            ));

            dbgKeyboradHooker = new KeyboradHooker(LoggerFactory);
            dbgKeyboradHooker.KeyDown += (sender, e) => {
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
            dbgKeyboradHooker.KeyUp += (sender, e) => {
                var jobs = dbgKeyActionChecker.Find(true, e.Key, new ModifierKeyStatus(), e.kbdll);
                Task.Run(() => {
                    dbgKeyActionAssistant.CleanupReplaceJob(e.Key, e.modifierKeyStatus);

                    foreach(var job in jobs) {
                        // 何もやることはないハズ
                    }
                });
            };
            dbgKeyboradHooker.Register();
        }

        void DebugSetting()
        {
            ShowSettingView();
        }
        void DebugColorPicker()
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

        void DebugEnvironmentExecuteFile()
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

        void Uninstall()
        {
            var about = ApplicationDiContainer.Build<Element.About.AboutElement>();
            var path = @"x:a.bat";
            var uninstallTarget = UninstallTarget.Application | UninstallTarget.Batch;
            uninstallTarget |= UninstallTarget.User | UninstallTarget.Machine | UninstallTarget.Temporary;
            about.CreateUninstallBatch(path, uninstallTarget);
        }

        class ttt: Control
        {


            public string? Text
            {
                get { return (string)GetValue(TextProperty); }
                set { SetValue(TextProperty, value); }
            }

            // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
                nameof(TextProperty),
                typeof(string),
                typeof(ttt),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    TextPropertyChanged
                )
            );

            private static void TextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                if(d is ttt control) {
                    control.Text = (string)e.NewValue;
                    control.InvalidateVisual();
                }
            }

            protected override void OnRender(DrawingContext drawingContext)
            {
                var text = Text;

                if(string.IsNullOrEmpty(text)) {
                    var emptyText = new FormattedText(
                        "M",
                        CultureInfo.CurrentUICulture,
                        FlowDirection,
                        new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                        FontSize,
                        Foreground,
                        VisualTreeHelper.GetDpi(this).PixelsPerDip
                    );

                    Height = emptyText.Height;
                    drawingContext.DrawRectangle(Background, new Pen(), new Rect(0, 0, ActualWidth, Height));
                    return;
                }

                var formattedText = new FormattedText(
                    text,
                    CultureInfo.CurrentUICulture,
                    FlowDirection,
                    new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                    FontSize,
                    Foreground,
                    VisualTreeHelper.GetDpi(this).PixelsPerDip
                );
                Height = formattedText.Height;

                drawingContext.DrawRectangle(Background, new Pen(), new Rect(0, 0, ActualWidth, Height));

                if(formattedText.Width <= ActualWidth) {
                    drawingContext.DrawText(formattedText, new Point(0, 0));
                    return;
                }

                Debug.WriteLine("何とかして縮める");

                var markChars = new[] { '\\', '/', };
                var ellipsis = "...";

                string markedText;
                int fronLength;

                var markIndex = text.LastIndexOfAny(markChars);
                if(markIndex != -1) {
                    var markChar = text[markIndex];
                    var lasIndex = text.LastIndexOf(markChar);

                    if(lasIndex < ellipsis.Length) {
                        drawingContext.DrawText(formattedText, new Point(0, 0));
                        return;
                    }

                    markedText = text.Substring(lasIndex);
                    fronLength = lasIndex - 1;
                } else {
                    var lasIndex = text.Length / 2;

                    if(lasIndex < 2) {
                        drawingContext.DrawText(formattedText, new Point(0, 0));
                        return;
                    }

                    markedText = text.Substring(lasIndex);
                    fronLength = lasIndex - 1;
                }

                while(true) {
                    var front = text.Substring(0, fronLength);

                    var shortText = front + ellipsis + markedText;

                    var shortFormattedText = new FormattedText(
                       shortText,
                       CultureInfo.CurrentUICulture,
                       FlowDirection,
                       new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                       FontSize,
                       Foreground,
                       VisualTreeHelper.GetDpi(this).PixelsPerDip
                   );

                    if(shortFormattedText.Width <= ActualWidth) {
                        drawingContext.DrawText(shortFormattedText, new Point(0, 0));
                        return;
                    }

                    fronLength -= 1;

                    if(fronLength == 0) {
                            drawingContext.DrawText(shortFormattedText, new Point(0, 0));
                        return;
                    }
                }
            }
        }

        void DebugIssue714()
        {
            var panel = new StackPanel();
            panel.Children.Add(new ttt() {
                Text = "abc def ghi jkl mno",
                Background = new SolidColorBrush(Colors.Red),
            });
            panel.Children.Add(new ttt() {
                Text = "あいうえおかきくけこさしすせそ",
                Background = new SolidColorBrush(Colors.Lime),
            });
            panel.Children.Add(new ttt() {
                Text = @"abc\def\ghi\jkl\mno",
                Background = new SolidColorBrush(Colors.Yellow),
            });
            panel.Children.Add(new ttt() {
                Text = @"あいう\えおか\きくけ\こさし\すせそ",
                Background = new SolidColorBrush(Colors.Green),
            });
            panel.Children.Add(new ttt() {
                Text = @"abc/def/ghi/jkl/mno",
                Background = new SolidColorBrush(Colors.Peru),
                FontSize = 20,
            });
            panel.Children.Add(new ttt() {
                Text = "あいう/えおか/きくけ/こさし/すせそ",
                Background = new SolidColorBrush(Colors.Pink),
                FontSize = 20,
            });

            var output = new ttt() {
                Foreground = new SolidColorBrush(Colors.White),
                Background = new SolidColorBrush(Colors.Black),
                FontSize = 20,
            };
            var input = new TextBox() {
                Name = "input",
            };
            output.SetBinding(ttt.TextProperty, new Binding("Text") {
                //ElementName = "input",
                Source = input,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
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
