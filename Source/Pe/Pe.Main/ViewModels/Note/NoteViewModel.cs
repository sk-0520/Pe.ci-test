using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using ContentTypeTextNet.Pe.Main.ViewModels.Font;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.Note;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Core.Views;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Main.Models.Theme;
using ContentTypeTextNet.Pe.Main.Models.Note;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Note
{
    public class NoteViewModel : SingleModelViewModelBase<NoteElement>, IViewLifecycleReceiver, IFlushable
    {
        #region variable

        double _windowLeft;
        double _windowTop;
        double _windowWidth;
        double _windowHeight;

        bool _titleEditMode;
        string? _editingTile;

        bool _showContentKindChangeConfim;
        NoteContentKind _changingContentKind;
        NoteContentViewModelBase? _content;

        bool _showLinkChangeConfim;

        #endregion

        public NoteViewModel(NoteElement model, INoteTheme noteTheme, IOrderManager orderManager, IClipboardManager clipboardManager, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            NoteTheme = noteTheme;
            OrderManager = orderManager;
            ClipboardManager = clipboardManager;
            DispatcherWrapper = dispatcherWrapper;

#pragma warning disable CS8604 // Null 参照引数の可能性があります。
            Font = new NoteFontViewModel(Model.FontElement, DispatcherWrapper, LoggerFactory);
#pragma warning restore CS8604 // Null 参照引数の可能性があります。

            DragAndDrop = new DelegateDragAndDrop(LoggerFactory) {
                CanDragStart = CanDragStartFile,
                GetDragParameter = GetDragParameterFile,
                DragEnterAction = DragEnterAndOverFile,
                DragOverAction = DragEnterAndOverFile,
                DragLeaveAction = DragLeaveFile,
                DropAction = DropFile,
            };

            PropertyChangedHooker = new PropertyChangedHooker(DispatcherWrapper, LoggerFactory);
            PropertyChangedHooker.AddHook(nameof(Model.IsVisible), nameof(IsVisible));
            PropertyChangedHooker.AddHook(nameof(Model.IsTopmost), nameof(IsTopmost));
            PropertyChangedHooker.AddHook(nameof(Model.IsCompact), nameof(IsCompact));
            PropertyChangedHooker.AddHook(nameof(Model.IsLocked), nameof(IsLocked));
            PropertyChangedHooker.AddHook(nameof(Model.TextWrap), nameof(TextWrap));
            PropertyChangedHooker.AddHook(nameof(Model.Title), nameof(Title));
            PropertyChangedHooker.AddHook(nameof(Model.ForegroundColor), () => ApplyTheme());
            PropertyChangedHooker.AddHook(nameof(Model.BackgroundColor), () => ApplyTheme());
            PropertyChangedHooker.AddHook(nameof(Model.LayoutKind), nameof(LayoutKind));
            PropertyChangedHooker.AddHook(nameof(Model.ContentKind), nameof(ContentKind));
            PropertyChangedHooker.AddHook(nameof(Model.ContentElement), nameof(Content));
        }

        #region property
        public RequestSender CloseRequest { get; } = new RequestSender();

        public RequestSender TitleEditStartRequest { get; } = new RequestSender();
        public RequestSender SelectLinkFileRequest { get; } = new RequestSender();

        public RequestSender UnlinkRequest { get; } = new RequestSender();
        public RequestSender LinkChangeRequest { get; } = new RequestSender();

        bool CanLayoutNotify { get; set; }

        INoteTheme NoteTheme { get; }
        IOrderManager OrderManager { get; }
        IClipboardManager ClipboardManager { get; }
        IDispatcherWrapper DispatcherWrapper { get; }
        PropertyChangedHooker PropertyChangedHooker { get; }

        IDpiScaleOutputor DpiScaleOutputor { get; set; } = new EmptyDpiScaleOutputor();
        IDisposable? WindowHandleSource { get; set; }


        public Guid NoteId => Model.NoteId;
        public bool IsLink => Model.ContentElement?.IsLink ?? false;
        public string? LinkPath => Model.ContentElement?.GetLinkFilePath();

        public FontViewModel Font { get; }

        public NoteContentViewModelBase? Content
        {
            get
            {
                if(this._content == null || this._content.Kind != Model.ContentKind) {
                    if(this._content != null) {
                        this._content.Dispose();
                    }
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
                    this._content = NoteContentViewModelFactory.Create(Model.ContentElement, ClipboardManager, DispatcherWrapper, LoggerFactory);
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
                }

                return this._content;
            }
        }

        public bool IsVisible => Model.IsVisible;

        public bool IsTopmost => Model.IsTopmost;
        public bool IsCompact => Model.IsCompact;
        public bool IsLocked => Model.IsLocked;
        public bool TextWrap => Model.TextWrap;

        public double WindowLeft
        {
            get => this._windowLeft;
            set
            {
                if(SetProperty(ref this._windowLeft, value)) {
                    DelayNotifyWindowAreaChanged();
                }
            }
        }
        public double WindowTop
        {
            get => this._windowTop;
            set
            {
                if(SetProperty(ref this._windowTop, value)) {
                    DelayNotifyWindowAreaChanged();
                }
            }
        }

        public double WindowWidth
        {
            get => this._windowWidth;
            set
            {
                if(SetProperty(ref this._windowWidth, value)) {
                    DelayNotifyWindowAreaChanged();
                }
            }
        }
        double NormalWindowHeight { get; set; }
        public double WindowHeight
        {
            get => this._windowHeight;
            set
            {
                if(SetProperty(ref this._windowHeight, value)) {
                    if(!IsCompact) {
                        NormalWindowHeight = this._windowHeight;
                    }
                    DelayNotifyWindowAreaChanged();
                }
            }
        }

        public bool TitleEditMode
        {
            get => this._titleEditMode;
            set
            {
                if(SetProperty(ref this._titleEditMode, value)) {
                    if(TitleEditMode) {
                        TitleEditStartRequest.Send();
                    }
                }
            }
        }
        public string? EditingTitle
        {
            get => this._editingTile;
            set => SetProperty(ref this._editingTile, value);
        }

        public Color ForegroundColor
        {
            get => Model.ForegroundColor;
            set => Model.ChangeForegroundColorDelaySave(value);
        }
        public Color BackgroundColor
        {
            get => Model.BackgroundColor;
            set => Model.ChangeBackgroundColorDelaySave(value);
        }

        public string? Title => Model.Title;

        public NoteContentKind ContentKind
        {
            get => Model.ContentKind;
            set
            {
                if(IsLink) {
                    Logger.LogError("リンク中は変更できない");
                    return;
                }

                // DB みてあれこれ判断するので止めてるやつを全部実施, プロパティでやるにはでっかいなぁと思うがまぁいいでしょ ;)
                Flush();

                if(!Model.CanChangeContentKind(value)) {
                    // 単純変換が出来ない場合はあれやこれや頑張る
                    ChangingContentKind = value;
                    ShowContentKindChangeConfim = true;
                } else {
                    // 変換するがユーザー選択は不要
                    Model.ConvertContentKind(value);
                }
            }
        }

        public NoteLayoutKind LayoutKind
        {
            get => Model.LayoutKind;
            set
            {
                Flush();
                var rect = value switch
                {
                    NoteLayoutKind.Absolute => CurrentWindowToAbsoluteLayout(),
                    NoteLayoutKind.Relative => CurrentWindowToRelativeLayout(),
                    _ => throw new NotImplementedException()
                };
                var layout = new NoteLayoutData() {
                    NoteId = NoteId,
                    LayoutKind = value,
                    X = rect.X,
                    Y = rect.Y,
                    Width = rect.Width,
                    Height = rect.Height,
                };
                Model.ChangeLayoutKind(layout);
            }
        }

        public IDragAndDrop DragAndDrop { get; }

        bool PrepareToRemove { get; set; }

        #region theme
        public double CaptionHeight => NoteTheme.GetCaptionHeight();
        public Brush BorderBrush => NoteTheme.GetBorderBrush(GetColorPair());
        public Thickness BorderThickness => NoteTheme.GetBorderThickness();
        public Brush CaptionBackgroundNoneBrush => NoteTheme.GetCaptionButtonBackgroundBrush(NoteCaptionButtonState.None, GetColorPair());
        public Brush CaptionBackgroundOverBrush => NoteTheme.GetCaptionButtonBackgroundBrush(NoteCaptionButtonState.Over, GetColorPair());
        public Brush CaptionBackgroundPressedBrush => NoteTheme.GetCaptionButtonBackgroundBrush(NoteCaptionButtonState.Pressed, GetColorPair());
        public Brush? CaptionForeground { get; private set; }
        public Brush? CaptionBackground { get; private set; }
        public Brush? ContentBackground { get; private set; }
        public Brush? ContentForeground { get; private set; }

        public DependencyObject ResizeGripImage => NoteTheme.GetResizeGripImage(GetColorPair());

        public DependencyObject CaptionCompactEnabledImage => NoteTheme.GetCaptionImage(NoteCaption.Compact, true, GetColorPair());
        public DependencyObject CaptionCompactDisabledImage => NoteTheme.GetCaptionImage(NoteCaption.Compact, false, GetColorPair());
        public DependencyObject CaptionTopmostEnabledImage => NoteTheme.GetCaptionImage(NoteCaption.Topmost, true, GetColorPair());
        public DependencyObject CaptionTopmostDisabledImage => NoteTheme.GetCaptionImage(NoteCaption.Topmost, false, GetColorPair());

        public DependencyObject CaptionCloseImage => NoteTheme.GetCaptionImage(NoteCaption.Close, false, GetColorPair());
        public double MinHeight => CaptionHeight + BorderThickness.Top + BorderThickness.Bottom;

        #endregion

        #region content kind changing
        public NoteContentKind ChangingContentKind
        {
            get => this._changingContentKind;
            set => SetProperty(ref this._changingContentKind, value);
        }

        public bool ShowContentKindChangeConfim
        {
            get => this._showContentKindChangeConfim;
            set => SetProperty(ref this._showContentKindChangeConfim, value);
        }

        public bool ShowLinkChangeConfim
        {
            get => this._showLinkChangeConfim;
            private set => SetProperty(ref this._showLinkChangeConfim, value);
        }
        //bool CanLoadContentKind { get; set; }

        #endregion


        #endregion

        #region command

        public ICommand SwitchCompactCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                if(!IsCompact) {
                    NormalWindowHeight = WindowHeight;
                }
                Model.SwitchCompactDelaySave();
                // レイアウト変更(高さ)通知を抑制
                if(!IsCompact) {
                    this._windowHeight = NormalWindowHeight;
                } else {
                    this._windowHeight = 0;
                }
                RaisePropertyChanged(nameof(WindowHeight));
            }
        ));
        public ICommand SwitchTopmostCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.SwitchTopmostDelaySave();
            }
        ));

        public ICommand SwitchLockCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.SwitchLockDelaySave();
            }
        ));

        public ICommand SwitchTextWrapCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.SwitchTextWrapDelaySave();
            }
        ));

        public ICommand CancelTitleEditCommand => GetOrCreateCommand(() => new DelegateCommand<TextBox>(
            o => {
                TitleEditMode = false;
                EditingTitle = string.Empty;
                o.Select(0, 0);
            }
        ));
        public ICommand SaveTitleEditCommand => GetOrCreateCommand(() => new DelegateCommand<TextBox>(
            o => {
                TitleEditMode = false;
                Model.ChangeTitleDelaySave(EditingTitle ?? string.Empty);
                o.Select(0, 0);
            },
            o => TitleEditMode
        ));

        public ICommand ContentKindChangeConvertCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Flush();

                Model.ConvertContentKind(ChangingContentKind);
                ShowContentKindChangeConfim = false;
            }
        ));

        public ICommand ContentKindChangeCancelCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ShowContentKindChangeConfim = false;
            }
        ));

        public ICommand RemoveCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                PrepareToRemove = true;
                CloseRequest.Send();
            }
        ));

        public ICommand LinkChangeCancelCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ShowLinkChangeConfim = false;
            }
        ));

        public ICommand LinkChangeCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ShowLinkChangeConfim = true;
            }
        ));
        public ICommand UnlinkCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.Unlink(false);
                RaisePropertyChanged(nameof(IsLink));
                RaisePropertyChanged(nameof(LinkPath));
            },
            () => IsLink
        ).ObservesProperty(() => IsLink));

        public ICommand DeleteCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.Unlink(true);
                RaisePropertyChanged(nameof(IsLink));
                RaisePropertyChanged(nameof(LinkPath));
            },
            () => IsLink
        ).ObservesProperty(() => IsLink));


        public ICommand SaveLinkCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                var parameter = CreateLinkParameter(false);
                LinkChangeRequest.Send<NoteLinkChangeRequestResponse>(parameter, r => {
                    if(r.ResponseIsCancel) {
                        return;
                    }

                    if(r.ResponseFilePaths != null && 0 < r.ResponseFilePaths.Length) {
                        Model.OpenLinkContent(r.ResponseFilePaths[0], r.Encoding!, false);
                        RaisePropertyChanged(nameof(IsLink));
                        RaisePropertyChanged(nameof(LinkPath));
                    }
                });
            },
            () => !IsLink
        ).ObservesProperty(() => IsLink));
        public ICommand OpenLinkCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                var parameter = CreateLinkParameter(true);
                LinkChangeRequest.Send<NoteLinkChangeRequestResponse>(parameter, r => {
                    if(r.ResponseIsCancel) {
                        return;
                    }

                    if(r.ResponseFilePaths != null && 0 < r.ResponseFilePaths.Length) {
                        Model.OpenLinkContent(r.ResponseFilePaths[0], r.Encoding!, true);
                        RaisePropertyChanged(nameof(IsLink));
                        RaisePropertyChanged(nameof(LinkPath));
                    }
                });
            },
            () => !IsLink
        ).ObservesProperty(() => IsLink));


        #endregion

        #region function

        NoteLinkChangeRequestParameter CreateLinkParameter(bool isOpen)
        {
            var parameter = new NoteLinkChangeRequestParameter() {
                IsOpen = isOpen,
                Encoding = EncodingConverter.DefaultEncoding,
            };
            var contentKindFilter = ContentKind switch
            {
                NoteContentKind.Plain => new DialogFilterItem("text", "txt", "*.txt"),
                NoteContentKind.RichText => new DialogFilterItem("rtf", "rtf", "*.rtf"),
                _ => throw new NotImplementedException(),
            };
            parameter.Filter.Add(contentKindFilter);
            parameter.Filter.Add(new DialogFilterItem("all", string.Empty, "*.*"));

            return parameter;
        }


        (bool isCreated, NoteLayoutData layout) GetOrCreateLayout(NoteStartupPosition startupPosition)
        {
            if(startupPosition == NoteStartupPosition.Setting) {
                var settingLayout = Model.GetLayout();
                if(settingLayout != null) {
                    return (false, settingLayout);
                } else {
                    Logger.LogInformation("レイアウト未取得のため対象ディスプレイ中央表示: {0}, {1}", Model.DockScreen.DeviceName, ObjectDumper.GetDumpString(Model.DockScreen));
                    startupPosition = NoteStartupPosition.CenterScreen;
                }
            }

            //TODO: 未検証ゾーン
            var logicalScreenSize = UIUtility.ToLogicalPixel(Model.DockScreen.DeviceBounds.Size, DpiScaleOutputor);
            var layout = new NoteLayoutData() {
                NoteId = NoteId,
                LayoutKind = Model.LayoutKind,
            };

            if(startupPosition == NoteStartupPosition.CenterScreen) {
                if(layout.LayoutKind == NoteLayoutKind.Absolute) {
                    layout.Width = 200;
                    layout.Height = 200;
                    layout.X = (logicalScreenSize.Width / 2) - (layout.Width / 2);
                    layout.Y = (logicalScreenSize.Height / 2) - (layout.Height / 2);
                } else {
                    Debug.Assert(layout.LayoutKind == NoteLayoutKind.Relative);
                    layout.Width = 20;
                    layout.Height = 20;
                    layout.X = 0;
                    layout.Y = 0;
                }
            } else {
                Debug.Assert(startupPosition == NoteStartupPosition.CursorPosition);

                var deviceScreenBounds = Model.DockScreen.DeviceBounds;

                NativeMethods.GetCursorPos(out var podPoint);
                var deviceCursorLocation = PodStructUtility.Convert(podPoint);

                var deviceScreenCursorLocation = new Point(
                    deviceCursorLocation.X - deviceScreenBounds.X,
                    deviceCursorLocation.Y - deviceScreenBounds.Y
                );
                var logicalScreenCursorLocation = UIUtility.ToLogicalPixel(deviceScreenCursorLocation, DpiScaleOutputor);

                if(layout.LayoutKind == NoteLayoutKind.Absolute) {
                    layout.Width = 200;
                    layout.Height = 200;
                    layout.X = logicalScreenCursorLocation.X;
                    layout.Y = logicalScreenCursorLocation.Y;
                } else {
                    Debug.Assert(layout.LayoutKind == NoteLayoutKind.Relative);

                    layout.Width = 20;
                    layout.Height = 20;
                    layout.X = deviceScreenCursorLocation.X * (deviceScreenBounds.Width / 100);
                    layout.Y = deviceScreenCursorLocation.Y * (deviceScreenBounds.Height / 100);
                }
            }

            return (true, layout);
        }

        Rect AbsoluteLayoutToWindow(NoteLayoutData layout)
        {
            var logicalBounds = UIUtility.ToLogicalPixel(Model.DockScreen.DeviceBounds, DpiScaleOutputor);
            return new Rect(
                logicalBounds.X + layout.X,
                logicalBounds.Y + layout.Y,
                layout.Width,
                layout.Height
            );
        }
        Rect RelativeLayoutToWindow(NoteLayoutData layout)
        {
            var logicalBounds = UIUtility.ToLogicalPixel(Model.DockScreen.DeviceBounds, DpiScaleOutputor);
            var area = new Size(
                logicalBounds.Width / 100,
                logicalBounds.Height / 100
            );
            var center = new Point(
                logicalBounds.Width / 2,
                logicalBounds.Height / 2
            );

            var width = area.Width * layout.Width;
            var height = area.Height * layout.Height;
            return new Rect(
                logicalBounds.X + center.X + (area.Width / 2 * layout.X) - (width / 2),
                logicalBounds.Y + center.Y - (area.Height / 2 * layout.Y) - (height / 2),
                width,
                height
            );
        }
        Rect CurrentWindowToAbsoluteLayout()
        {
            var logicalScreenLocation = UIUtility.ToLogicalPixel(Model.DockScreen.DeviceBounds.Location, DpiScaleOutputor);
            return new Rect(
                WindowLeft - logicalScreenLocation.X,
                WindowTop - logicalScreenLocation.Y,
                WindowWidth,
                NormalWindowHeight
            );
        }
        Rect CurrentWindowToRelativeLayout()
        {
            var logicalBounds = UIUtility.ToLogicalPixel(Model.DockScreen.DeviceBounds, DpiScaleOutputor);
            var area = new Size(
                logicalBounds.Width / 100,
                logicalBounds.Height / 100
            );
            var center = new Point(
                logicalBounds.Width / 2,
                logicalBounds.Height / 2
            );

            return new Rect(
                ((WindowLeft - logicalBounds.X) + (WindowWidth / 2) - center.X) / (area.Width / 2),
                -((WindowTop - logicalBounds.Y) + (NormalWindowHeight / 2) - center.Y) / (area.Height / 2),
                WindowWidth / area.Width,
                NormalWindowHeight / area.Height
            );
        }

        void SetLayout(NoteLayoutData layout)
        {
            var rect = layout.LayoutKind switch
            {
                NoteLayoutKind.Absolute => AbsoluteLayoutToWindow(layout),
                NoteLayoutKind.Relative => RelativeLayoutToWindow(layout),
                _ => throw new NotImplementedException()
            };
            WindowLeft = rect.X;
            WindowTop = rect.Y;
            WindowWidth = rect.Width;
            NormalWindowHeight = rect.Height;

            if(IsCompact) {
                WindowHeight = 0;
            } else {
                WindowHeight = NormalWindowHeight;
            }

        }

        void ApplyCaption()
        {
            var captionPair = NoteTheme.GetCaptionBrush(GetColorPair());
            CaptionForeground = captionPair.Foreground;
            CaptionBackground = captionPair.Background;

            var contentPair = NoteTheme.GetContentBrush(GetColorPair());
            ContentForeground = contentPair.Foreground;
            ContentBackground = contentPair.Background;

            var propertyNames = new[] {
                nameof(CaptionForeground),
                nameof(CaptionBackground),
                nameof(ContentForeground),
                nameof(ContentBackground),
                nameof(CaptionCompactEnabledImage),
                nameof(CaptionCompactDisabledImage),
                nameof(CaptionTopmostEnabledImage),
                nameof(CaptionTopmostDisabledImage),
                nameof(CaptionCloseImage),
            };
            foreach(var propertyName in propertyNames) {
                RaisePropertyChanged(propertyName);
            }
        }

        void ApplyBorder()
        {
            var propertyNames = new[] {
                nameof(BorderBrush),
                nameof(BorderThickness),
                nameof(ResizeGripImage),
            };
            foreach(var propertyName in propertyNames) {
                RaisePropertyChanged(propertyName);
            }
        }

        void ApplyContent()
        {
            var propertyNames = new[] {
                nameof(ContentBackground),
            };
            foreach(var propertyName in propertyNames) {
                RaisePropertyChanged(propertyName);
            }
        }

        void ApplyTheme()
        {
            DispatcherWrapper.Begin(() => {
                ApplyCaption();
                ApplyBorder();
                ApplyContent();
            }, DispatcherPriority.Render);
        }

        void DelayNotifyWindowAreaChanged()
        {
            Logger.LogDebug("モデルへの位置・サイズ通知: {0}, {1}", Model.NoteId, CanLayoutNotify);
            if(!CanLayoutNotify) {
                Logger.LogDebug("モデルへの通知抑制中");
                return;
            }

            var viewAreaChangeTargets = ViewAreaChangeTarget.Location;

            var nowScreen = DpiScaleOutputor.GetOwnerScreen();
            if(Model.DockScreen.DeviceName != nowScreen.DeviceName) {
                Logger.LogDebug("所属スクリーン変更: {0} -> {1}", Model.DockScreen.DeviceName, nowScreen.DeviceName);
                Model.ChangeDockScreen(nowScreen);
                viewAreaChangeTargets |= ViewAreaChangeTarget.Screen;
            }

            var rect = Model.LayoutKind switch
            {
                NoteLayoutKind.Absolute => CurrentWindowToAbsoluteLayout(),
                NoteLayoutKind.Relative => CurrentWindowToRelativeLayout(),
                _ => throw new NotImplementedException()
            };
            // 最小化中はウィンドウサイズに対して何もしない
            if(!IsCompact) {
                viewAreaChangeTargets |= ViewAreaChangeTarget.Suze;
            }

            Model.ChangeViewAreaDelaySave(viewAreaChangeTargets, rect.Location, rect.Size);
        }

        IReadOnlyColorPair<Color> GetColorPair() => ColorPair.Create(Model.ForegroundColor, Model.BackgroundColor);

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch(msg) {
                case (int)WM.WM_NCLBUTTONDBLCLK:
                    if(WindowsUtility.ConvertHTFromWParam(wParam) == HT.HTCAPTION) {
                        if(!IsLocked) {
                            EditingTitle = Title;
                            TitleEditMode = true;
                        }
                    }
                    break;

                default:
                    break;
            }
            return IntPtr.Zero;
        }

        #region DragAndDrop

        private bool CanDragStartFile(UIElement sender, MouseEventArgs e)
        {
            return false;
        }
        private IResultSuccessValue<DragParameter> GetDragParameterFile(UIElement sender, MouseEventArgs e)
        {
            throw new NotSupportedException();
        }

        private void DragEnterAndOverFile(UIElement sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;

            if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
                e.Effects = DragDropEffects.Copy;
            }
        }

        private void DragLeaveFile(UIElement sender, DragEventArgs e)
        { }

        private void DropFile(UIElement sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;

            if(e.Effects != DragDropEffects.Copy) {
                var filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
            }
        }

        #endregion

        #endregion

        #region IViewLifecycleReceiver

        public void ReceiveViewInitialized(Window window)
        {
            // 各ディスプレイのDPIで事故らないように原点をディスプレイへ移動してウィンドウ位置・サイズをいい感じに頑張る
            var hWnd = HandleUtility.GetWindowHandle(window);
            NativeMethods.SetWindowPos(hWnd, new IntPtr((int)HWND.HWND_TOP), (int)Model.DockScreen.DeviceBounds.X, (int)Model.DockScreen.DeviceBounds.Y, 0, 0, SWP.SWP_NOSIZE);

            // タイトルバーのダブルクリックを拾う必要がある
            var hWndSource = HwndSource.FromHwnd(hWnd);
            hWndSource.AddHook(WndProc);
            WindowHandleSource = hWndSource;

            DpiScaleOutputor = (IDpiScaleOutputor)window;

            var layoutValue = GetOrCreateLayout(Model.StartupPosition);
            if(layoutValue.isCreated) {
                Model.SaveLayout(layoutValue.layout);
            }

            SetLayout(layoutValue.layout);

            ApplyTheme();

            CanLayoutNotify = true;
        }

        public void ReceiveViewLoaded(Window window)
        {
            if(!IsVisible) {
                window.Visibility = Visibility.Collapsed;
            }
        }

        public void ReceiveViewUserClosing(CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewUserClosing();
        }

        public void ReceiveViewClosing(CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewClosing();
        }

        public void ReceiveViewClosed()
        {
            Model.ReceiveViewClosed();

            if(PrepareToRemove) {
                Flush();
                var noteId = Model.NoteId;
                // 削除するにあたりこいつはもう呼び出せない
                Dispose();
                OrderManager.RemoveNoteElement(noteId);

            }
        }

        #endregion

        #region SingleModelViewModelBase

        protected override void AttachModelEventsImpl()
        {
            base.AttachModelEventsImpl();

            Model.PropertyChanged += Model_PropertyChanged;
        }

        protected override void DetachModelEventsImpl()
        {
            base.DetachModelEventsImpl();

            Model.PropertyChanged -= Model_PropertyChanged;
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Flush();
                if(disposing) {
                    WindowHandleSource?.Dispose();
                    PropertyChangedHooker.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        #region IFlush

        public void Flush()
        {
            Model.SafeFlush();
        }

        #endregion

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChangedHooker.Execute(e, RaisePropertyChanged);
        }

    }
}
