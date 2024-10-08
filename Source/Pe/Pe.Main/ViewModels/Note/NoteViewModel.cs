using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.Note;
using ContentTypeTextNet.Pe.Main.Models.Element.NotifyLog;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Note;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Theme;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using ContentTypeTextNet.Pe.Main.ViewModels.Font;
using ContentTypeTextNet.Pe.Main.ViewModels.NotifyLog;
using ContentTypeTextNet.Pe.Main.ViewModels.Setting;
using ContentTypeTextNet.Pe.Main.Views.Note;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Note
{
    public class NoteViewModel: ElementViewModelBase<NoteElement>, IViewLifecycleReceiver, IFlushable
    {
        #region variable

        private double _windowLeft;
        private double _windowTop;
        private double _windowWidth;
        private double _windowHeight;

        private bool _titleEditMode;
        private string? _editingTile;

        private bool _showContentKindChangeConfirm;
        private NoteContentKind _changingContentKind;
        private NoteContentViewModelBase? _content;

        private bool _showLinkChangeConfirm;
        private bool _isPopupRemoveNote;

        private bool _windowMoving = false;

        /// <summary>
        /// 検索中か。
        /// </summary>
        private bool _isSearching = false;
        /// <summary>
        /// 検索文字列。
        /// </summary>
        private string _searchValue = string.Empty;

        #endregion

        public NoteViewModel(NoteElement model, NoteConfiguration noteConfiguration, INoteTheme noteTheme, IGeneralTheme generalTheme, IPlatformTheme platformTheme, ApplicationConfiguration applicationConfiguration, IOrderManager orderManager, ICultureService cultureService, IClipboardManager clipboardManager, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        {
            NoteConfiguration = noteConfiguration;
            NoteTheme = noteTheme;
            GeneralTheme = generalTheme;
            PlatformTheme = platformTheme;
            ApplicationConfiguration = applicationConfiguration;
            OrderManager = orderManager;
            CultureService = cultureService;
            ClipboardManager = clipboardManager;

            PlatformTheme.Changed += PlatformTheme_Changed;

            Debug.Assert(Model.FontElement != null);
            Font = new NoteFontViewModel(Model.FontElement, DispatcherWrapper, LoggerFactory);

            FileDragAndDrop = new DelegateDragAndDrop(true, LoggerFactory) {
                CanDragStart = FileCanDragStart,
                GetDragParameter = FileGetDragParameter,
                DragEnterAction = FileDragEnterAndOver,
                DragOverAction = FileDragEnterAndOver,
                DragLeaveAction = FileDragLeave,
                DropActionAsync = FileDropAsync,
            };

            FileCollection = new ModelViewModelObservableCollectionManager<NoteFileElement, NoteFileViewModel>(Model.Files, new ModelViewModelObservableCollectionOptions<NoteFileElement, NoteFileViewModel>() {
                ToViewModel = m => new NoteFileViewModel(m, UserTracker, DispatcherWrapper, LoggerFactory)
            });
            FileItems = FileCollection.GetDefaultView();

            PropertyChangedObserver = new PropertyChangedObserver(DispatcherWrapper, LoggerFactory);
            PropertyChangedObserver.AddObserver(nameof(Model.IsVisible), nameof(IsVisible));
            PropertyChangedObserver.AddObserver(nameof(Model.IsTopmost), nameof(IsTopmost));
            PropertyChangedObserver.AddObserver(nameof(Model.IsCompact), nameof(IsCompact));
            PropertyChangedObserver.AddObserver(nameof(Model.IsLocked), nameof(IsLocked));
            PropertyChangedObserver.AddObserver(nameof(Model.TextWrap), nameof(TextWrap));
            PropertyChangedObserver.AddObserver(nameof(Model.Title), nameof(Title));
            PropertyChangedObserver.AddObserver(new ObserveItem(nameof(Model.CaptionPosition), new[] { nameof(CaptionPosition) }, null, () => ApplyTheme()));
            PropertyChangedObserver.AddObserver(nameof(Model.ForegroundColor), () => ApplyTheme());
            PropertyChangedObserver.AddObserver(nameof(Model.BackgroundColor), () => ApplyTheme());
            PropertyChangedObserver.AddObserver(nameof(Model.LayoutKind), nameof(LayoutKind));
            PropertyChangedObserver.AddObserver(nameof(Model.ContentKind), nameof(ContentKind));
            PropertyChangedObserver.AddObserver(nameof(Model.ContentElement), nameof(Content));
            PropertyChangedObserver.AddObserver(nameof(Model.IsVisibleBlind), nameof(IsVisibleBlind));
            PropertyChangedObserver.AddObserver(nameof(Model.IsVisibleBlind), () => ApplyTheme());
            PropertyChangedObserver.AddObserver(nameof(Model.HiddenCompact), () => HideCompact());
        }


        #region property

        public RequestSender CloseRequest { get; } = new RequestSender();

        public RequestSender TitleEditStartRequest { get; } = new RequestSender();
        //public RequestSender SelectLinkFileRequest { get; } = new RequestSender();

        public RequestSender LinkChangeRequest { get; } = new RequestSender();

        bool CanLayoutNotify { get; set; }

        private NoteConfiguration NoteConfiguration { get; }
        private INoteTheme NoteTheme { get; }
        private IGeneralTheme GeneralTheme { get; }
        private IPlatformTheme PlatformTheme { get; }
        private IOrderManager OrderManager { get; }
        private ICultureService CultureService { get; }
        private IClipboardManager ClipboardManager { get; }
        private PropertyChangedObserver PropertyChangedObserver { get; }

        private IDpiScaleOutpour DpiScaleOutpour { get; set; } = new EmptyDpiScaleOutpour();
        private FrameworkElement? CaptionElement { get; set; }
        private TextBoxBase? InputSearchElement { get; set; }
        private IDisposable? WindowHandleSource { get; set; }

        private ApplicationConfiguration ApplicationConfiguration { get; }

        public NoteId NoteId => Model.NoteId;

        public ModelViewModelObservableCollectionManager<NoteFileElement, NoteFileViewModel> FileCollection { get; }
        public ICollectionView FileItems { get; }


        public bool IsLink
        {
            get
            {
                if(IsDisposed) {
                    return false;
                }

                if(Model.ContentElement == null) {
                    return false;
                }

                if(Model.ContentElement.IsDisposed) {
                    return false;
                }

                return Model.ContentElement.IsLink;
            }
        }
        public string? LinkPath
        {
            get
            {
                if(IsDisposed) {
                    return string.Empty;
                }

                if(Model.ContentElement == null) {
                    return string.Empty;
                }

                if(Model.ContentElement.IsDisposed) {
                    return string.Empty;
                }

                return Model.ContentElement.GetLinkFilePath();
            }
        }

        public FontViewModel Font { get; }

        public NoteContentViewModelBase? Content
        {
            get
            {
                if(this._content == null || this._content.Kind != Model.ContentKind) {
                    if(this._content != null) {
                        this._content.Dispose();
                    }
                    Debug.Assert(Model.ContentElement != null);
                    this._content = NoteContentViewModelFactory.Create(Model.ContentElement, NoteConfiguration, ClipboardManager, DispatcherWrapper, LoggerFactory);
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
        /// <summary>
        /// ウィンドウの非最小化時の横幅。
        /// </summary>
        private double NormalWindowWidth { get; set; }
        /// <summary>
        /// ウィンドウの横幅。
        /// </summary>
        /// <remarks>
        /// <para>最小化時も含む。</para>
        /// </remarks>
        public double WindowWidth
        {
            get => this._windowWidth;
            set
            {
                if(SetProperty(ref this._windowWidth, value)) {
                    if(!IsCompact) {
                        NormalWindowWidth = this._windowWidth;
                    }
                    DelayNotifyWindowAreaChanged();
                }
            }
        }
        /// <summary>
        /// ウィンドウの非最小化時の横幅。
        /// </summary>
        private double NormalWindowHeight { get; set; }
        /// <summary>
        /// ウィンドウの縦幅。
        /// </summary>
        /// <remarks>
        /// <para>最小化時も含む。</para>
        /// </remarks>
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

        public NoteCaptionPosition CaptionPosition
        {
            get => Model.CaptionPosition;
            set
            {
                if(IsCompact) {
                    Logger.LogWarning("最小化時には変更不可");
                    return;
                }

                Model.ChangeCaptionPositionDelaySave(value);
            }
        }

        public NoteContentKind ContentKind
        {
            get => Model.ContentKind;
            set
            {
                if(IsLink) {
                    Logger.LogWarning("リンク中は変更できない");
                    return;
                }
                if(value == Model.ContentKind) {
                    // 同一値はなんもしない
                    return;
                }

                // DB みてあれこれ判断するので止めてるやつを全部実施, プロパティでやるにはでっかいなぁと思うがまぁいいでしょ ;)
                Flush();

                if(!Model.CanChangeContentKind(value)) {
                    // 単純変換が出来ない場合はあれやこれや頑張る
                    ChangingContentKind = value;
                    ShowContentKindChangeConfirm = true;
                } else {
                    // 変換するがユーザー選択は不要
                    _ = Model.ConvertContentKindAsync(value, CancellationToken.None);
                }
            }
        }

        public NoteLayoutKind LayoutKind
        {
            get => Model.LayoutKind;
            set
            {
                if(value == Model.LayoutKind) {
                    // 同一値はなんもしない
                    return;
                }

                Flush();
                var rect = value switch {
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

        public IDragAndDrop FileDragAndDrop { get; }

        private bool PrepareToRemove { get; set; }
        public bool IsPopupRemoveNote
        {
            get => this._isPopupRemoveNote;
            set => SetProperty(ref this._isPopupRemoveNote, value);
        }

        public bool WindowMoving
        {
            get => this._windowMoving;
            private set => SetProperty(ref this._windowMoving, value);
        }

        public double WindowMovingOpacity => NoteConfiguration.MovingOpacity;

        public IReadOnlyList<NoteHiddenMode> HiddenModeItems { get; } = Enum.GetValues<NoteHiddenMode>().OrderBy(i => i).ToList();

        public NoteHiddenMode SelectedHiddenMode
        {
            get => Model.HiddenMode;
            set
            {
                if(SelectedHiddenMode != value) {
                    Model.ChangeHiddenModeDelaySave(value);
                }
            }
        }

        public bool IsVisibleBlind => Model.IsVisibleBlind;

        #region theme

        [ThemeProperty]
        public double CaptionSize => NoteTheme.GetCaptionHeight();
        [ThemeProperty]
        public Brush BorderBrush => NoteTheme.GetBorderBrush(CaptionPosition, GetColorPair());
        [ThemeProperty]
        public Thickness BorderThickness => NoteTheme.GetBorderThickness();
        [ThemeProperty]
        public Brush CaptionBackgroundNoneBrush => NoteTheme.GetCaptionButtonBackgroundBrush(NoteCaptionButtonState.None, CaptionPosition, GetColorPair());
        [ThemeProperty]
        public Brush CaptionBackgroundOverBrush => NoteTheme.GetCaptionButtonBackgroundBrush(NoteCaptionButtonState.Over, CaptionPosition, GetColorPair());
        [ThemeProperty]
        public Brush CaptionBackgroundPressedBrush => NoteTheme.GetCaptionButtonBackgroundBrush(NoteCaptionButtonState.Pressed, CaptionPosition, GetColorPair());
        [ThemeProperty]
        public Brush? CaptionForeground { get; private set; }
        [ThemeProperty]
        public Brush? CaptionBackground { get; private set; }
        [ThemeProperty]
        public Brush? ContentBackground { get; private set; }
        [ThemeProperty]
        public Brush? ContentForeground { get; private set; }

        [ThemeProperty]
        public DependencyObject ResizeGripImage => NoteTheme.GetResizeGripImage(CaptionPosition, GetColorPair());

        [ThemeProperty]
        public DependencyObject CaptionCompactEnabledImage => NoteTheme.GetCaptionImage(NoteCaptionButtonKind.Compact, CaptionPosition, true, GetColorPair());
        [ThemeProperty]
        public DependencyObject CaptionCompactDisabledImage => NoteTheme.GetCaptionImage(NoteCaptionButtonKind.Compact, CaptionPosition, false, GetColorPair());
        [ThemeProperty]
        public DependencyObject CaptionTopmostEnabledImage => NoteTheme.GetCaptionImage(NoteCaptionButtonKind.Topmost, CaptionPosition, true, GetColorPair());
        [ThemeProperty]
        public DependencyObject CaptionTopmostDisabledImage => NoteTheme.GetCaptionImage(NoteCaptionButtonKind.Topmost, CaptionPosition, false, GetColorPair());

        [ThemeProperty]
        public DependencyObject CaptionCloseImage => NoteTheme.GetCaptionImage(NoteCaptionButtonKind.Close, CaptionPosition, false, GetColorPair());
        [ThemeProperty]
        public double MinHeight => CaptionSize + BorderThickness.Top + BorderThickness.Bottom;
        [ThemeProperty]
        public double MinWidth => CaptionSize + BorderThickness.Left + BorderThickness.Right;

        [ThemeProperty]
        public System.Windows.Media.Effects.Effect BlindEffect => NoteTheme.GetBlindEffect(GetColorPair());
        [ThemeProperty]
        public DependencyObject BlindContent => NoteTheme.GetBlindContent(GetColorPair());

        #endregion

        #region content kind changing
        public NoteContentKind ChangingContentKind
        {
            get => this._changingContentKind;
            set => SetProperty(ref this._changingContentKind, value);
        }

        public bool ShowContentKindChangeConfirm
        {
            get => this._showContentKindChangeConfirm;
            set
            {
                SetProperty(ref this._showContentKindChangeConfirm, value);
                if(ShowContentKindChangeConfirm) {
                    RaisePropertyChanged(nameof(ChangingContentKindMessage));
                }
            }
        }
        public string ChangingContentKindMessage
        {
            get
            {
                return TextUtility.ReplaceFromDictionary(
                    Properties.Resources.String_Note_KindChanging_Change_Format,
                    new Dictionary<string, string>() {
                        ["FROM-KIND"] = CultureService.GetString(ContentKind, Models.ResourceNameKind.Normal),
                        ["TO-KIND"] = CultureService.GetString(ChangingContentKind, Models.ResourceNameKind.Normal),
                    }
                );
            }
        }

        #endregion

        public bool ShowLinkChangeConfirm
        {
            get => this._showLinkChangeConfirm;
            private set => SetProperty(ref this._showLinkChangeConfirm, value);
        }

        /// <summary>
        /// 検索中か。
        /// </summary>
        public bool IsSearching
        {
            get => this._isSearching;
            private set => SetProperty(ref this._isSearching, value);
        }

        /// <summary>
        /// 検索文字列。
        /// </summary>
        public string SearchValue
        {
            get => this._searchValue;
            set => SetProperty(ref this._searchValue, value);
        }

        #endregion

        #region command

        private ICommand? _ToggleCompactCommand;
        public ICommand ToggleCompactCommand => this._ToggleCompactCommand ??= new DelegateCommand(
            () => {
                ToggleCompact();
            }
        );

        private ICommand? _ToggleTopmostCommand;
        public ICommand ToggleTopmostCommand => this._ToggleTopmostCommand ??= new DelegateCommand(
            () => {
                Model.ToggleTopmostDelaySave();
            }
        );

        private ICommand? _ToggleLockCommand;
        public ICommand ToggleLockCommand => this._ToggleLockCommand ??= new DelegateCommand(
            () => {
                Model.ToggleLockDelaySave();
            }
        );

        private ICommand? _ToggleTextWrapCommand;
        public ICommand ToggleTextWrapCommand => this._ToggleTextWrapCommand ??= new DelegateCommand(
            () => {
                Model.ToggleTextWrapDelaySave();
            }
        );

        private ICommand? _CancelTitleEditCommand;
        public ICommand CancelTitleEditCommand => this._CancelTitleEditCommand ??= new DelegateCommand<TextBox>(
            o => {
                TitleEditMode = false;
                EditingTitle = string.Empty;
                o.Select(0, 0);
            }
        );

        private ICommand? _SaveTitleEditCommand;
        public ICommand SaveTitleEditCommand => this._SaveTitleEditCommand ??= new DelegateCommand<TextBox>(
            o => {
                TitleEditMode = false;
                Model.ChangeTitleDelaySave(EditingTitle ?? string.Empty);
                o.Select(0, 0);
            },
            o => TitleEditMode
        );

        private ICommand? _ViewActivatedCommand;
        public ICommand ViewActivatedCommand => this._ViewActivatedCommand ??= new DelegateCommand<Window>(
            o => {
                Model.StopHidden(true);
            }
        );

        private ICommand? _ViewDeactivatedCommand;
        public ICommand ViewDeactivatedCommand => this._ViewDeactivatedCommand ??= new DelegateCommand<Window>(
            o => {
                if(o.IsVisible) {
                    if(!IsCompact && Model.HiddenMode != NoteHiddenMode.None) {
                        Model.StartHidden();
                    }
                }
            }
        );

        private ICommand? _ContentKindChangeConvertCommand;
        public ICommand ContentKindChangeConvertCommand => this._ContentKindChangeConvertCommand ??= new DelegateCommand(
            async () => {
                Flush();

                await Model.ConvertContentKindAsync(ChangingContentKind, CancellationToken.None);
                ShowContentKindChangeConfirm = false;
            }
        );

        private ICommand? _ContentKindChangeCancelCommand;
        public ICommand ContentKindChangeCancelCommand => this._ContentKindChangeCancelCommand ??= new DelegateCommand(
            () => {
                ShowContentKindChangeConfirm = false;
            }
        );

        private ICommand? _RemoveCommand;
        public ICommand RemoveCommand => this._RemoveCommand ??= new DelegateCommand(
            () => {
                PrepareToRemove = true;
                IsPopupRemoveNote = false;
                CloseRequest.Send();
            }
        );

        private ICommand? _SilentCloseCommand;
        public ICommand SilentCloseCommand => this._SilentCloseCommand ??= new DelegateCommand(
           () => {
               IsPopupRemoveNote = false;
               // Viewへの通知はユーザー操作に当たらないため明示的に非表示を設定
               Model.ChangeVisibleDelaySave(false);
               CloseRequest.Send();
           }
       );

        private ICommand? _LinkChangeCancelCommand;
        public ICommand LinkChangeCancelCommand => this._LinkChangeCancelCommand ??= new DelegateCommand(
            () => {
                ShowLinkChangeConfirm = false;
            }
        );

        private ICommand? _LinkChangeCommand;
        public ICommand LinkChangeCommand => this._LinkChangeCommand ??= new DelegateCommand(
            () => {
                ShowLinkChangeConfirm = true;
            }
        );

        private ICommand? _UnlinkCommand;
        public ICommand UnlinkCommand => this._UnlinkCommand ??= new DelegateCommand(
            () => {
                Model.Unlink(false);
                switch(Content) {
                    case NotePlainContentViewModel plain:
                        Model.ContentElement?.ChangePlainContent(plain.Content);
                        break;

                    case NoteRichTextContentViewModel rich:
                        rich.SafeFlush();
                        break;

                    default:
                        throw new NotImplementedException();
                }
                RaisePropertyChanged(nameof(IsLink));
                RaisePropertyChanged(nameof(LinkPath));
                ShowLinkChangeConfirm = false;
            },
            () => IsLink
        ).ObservesProperty(() => IsLink);

        private ICommand? _DeleteCommand;
        public ICommand DeleteCommand => this._DeleteCommand ??= new DelegateCommand(
            () => {
                Model.Unlink(true);
                RaisePropertyChanged(nameof(IsLink));
                RaisePropertyChanged(nameof(LinkPath));
                ShowLinkChangeConfirm = false;
            },
            () => IsLink
        ).ObservesProperty(() => IsLink);


        private ICommand? _SaveLinkCommand;
        public ICommand SaveLinkCommand => this._SaveLinkCommand ??= new DelegateCommand(
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
                ShowLinkChangeConfirm = false;
            },
            () => !IsLink
        ).ObservesProperty(() => IsLink);

        private ICommand? _OpenLinkCommand;
        public ICommand OpenLinkCommand => this._OpenLinkCommand ??= new DelegateCommand(
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
                ShowLinkChangeConfirm = false;
            },
            () => !IsLink
        ).ObservesProperty(() => IsLink);

        private ICommand? _OpenLinkFileDirectoryCommand;
        public ICommand OpenLinkFileDirectoryCommand => this._OpenLinkFileDirectoryCommand ??= new DelegateCommand(
            () => {
                if(string.IsNullOrWhiteSpace(LinkPath)) {
                    Logger.LogWarning("リンク未設定");
                    return;
                }

                try {
                    var systemExecutor = new SystemExecutor();
                    systemExecutor.OpenDirectoryWithFileSelect(Environment.ExpandEnvironmentVariables(LinkPath));
                } catch(Exception ex) {
                    Logger.LogError(ex, ex.Message);
                }
                ShowLinkChangeConfirm = false;
            }
        );

        private ICommand? _UnlinkFileCommand;
        public ICommand UnlinkFileCommand => this._UnlinkFileCommand ??= new DelegateCommand<NoteFileViewModel>(
            o => {
                Model.UnlinkFile(o.NoteFileId);
            }
        );

        private ICommand? _ContentSearchCommand;
        public ICommand ContentSearchCommand => this._ContentSearchCommand ??= new DelegateCommand<NoteFileViewModel>(
            o => {
                Logger.LogDebug("Ctrl+F");
                if(!IsSearching) {
                    SearchValue = string.Empty;
                }
                IsSearching = true;

                if(InputSearchElement is not null) {
                    InputSearchElement.SelectAll();
                    InputSearchElement.Focus();
                }
            },
            o => !IsCompact
        ).ObservesProperty(() => IsCompact);

        private ICommand? _CloseSearchCommand;
        public ICommand CloseSearchCommand => this._CloseSearchCommand ??= new DelegateCommand<NoteFileViewModel>(
            o => {
                IsSearching = false;
            }
        );

        private ICommand? _SearchNextCommand;
        public ICommand SearchNextCommand => this._SearchNextCommand ??= new DelegateCommand<NoteFileViewModel>(
            o => {
                SearchContent(SearchValue, true);
            },
            o => CanSearchContent()
        );

        private ICommand? _SearchPrevCommand;
        public ICommand SearchPrevCommand => this._SearchPrevCommand ??= new DelegateCommand<NoteFileViewModel>(
            o => {
                SearchContent(SearchValue, false);
            },
            o => CanSearchContent()
        );

        #endregion

        #region function

        private void ToggleCompact()
        {
            // 未変更情報
            if(!IsCompact) {
                if(CaptionPosition.IsVertical()) {
                    NormalWindowHeight = WindowHeight;
                } else {
                    NormalWindowWidth = WindowWidth;
                }

                IsSearching = false;
            }
            Model.ToggleCompactDelaySave();

            // 変更済み情報
            // レイアウト変更(高さ)通知を抑制
            if(!IsCompact) {
                if(CaptionPosition.IsVertical()) {
                    this._windowHeight = NormalWindowHeight;

                    if(CaptionPosition == NoteCaptionPosition.Bottom) {
                        WindowTop -= NormalWindowHeight - CaptionSize - (BorderThickness.Top + BorderThickness.Bottom);
                    }
                } else {
                    Debug.Assert(CaptionPosition.IsHorizontal());
                    this._windowWidth = NormalWindowWidth;

                    if(CaptionPosition == NoteCaptionPosition.Right) {
                        WindowLeft -= NormalWindowWidth - CaptionSize - (BorderThickness.Left + BorderThickness.Right);
                    }
                }
            } else {
                if(CaptionPosition.IsVertical()) {
                    this._windowHeight = 0;

                    if(CaptionPosition == NoteCaptionPosition.Bottom) {
                        WindowTop += NormalWindowHeight - CaptionSize - (BorderThickness.Top + BorderThickness.Bottom);
                    }
                } else {
                    this._windowWidth = 0;

                    if(CaptionPosition == NoteCaptionPosition.Right) {
                        WindowLeft += NormalWindowWidth - CaptionSize - (BorderThickness.Left + BorderThickness.Right);
                    }
                }
            }

            var propertyNames = new[] {
                nameof(WindowHeight),
                nameof(WindowWidth),
            };
            foreach(var propertyName in propertyNames) {
                RaisePropertyChanged(propertyName);
            }
        }

        private void HideCompact()
        {
            if(Model.HiddenCompact && !IsCompact) {
                ToggleCompact();
            }
        }

        private NoteLinkChangeRequestParameter CreateLinkParameter(bool isOpen)
        {
            var encodingConverter = new EncodingConverter(LoggerFactory!);

            var parameter = new NoteLinkChangeRequestParameter() {
                IsOpen = isOpen,
            };
            var contentKindFilter = ContentKind switch {
                NoteContentKind.Plain => new DialogFilterItem(Properties.Resources.String_FileDialog_Filter_Note_Plain, "txt", "*.txt"),
                NoteContentKind.RichText => new DialogFilterItem(Properties.Resources.String_FileDialog_Filter_Note_RichText, "rtf", "*.rtf"),
                _ => throw new NotImplementedException(),
            };
            parameter.Filter.Add(contentKindFilter);
            parameter.Filter.Add(new DialogFilterItem(Properties.Resources.String_FileDialog_Filter_Common_All, string.Empty, "*"));

            switch(ContentKind) {
                case NoteContentKind.Plain:
                    parameter.Encoding = EncodingUtility.UTF8N;
                    parameter.Encodings.Add(EncodingUtility.UTF8N);
                    parameter.Encodings.Add(EncodingUtility.UTF8Bom);
                    parameter.Encodings.Add(Encoding.Unicode);
                    parameter.Encodings.Add(Encoding.UTF32);
                    break;

                case NoteContentKind.RichText:
                    parameter.Encoding = Encoding.ASCII;
                    break;

                default:
                    throw new NotImplementedException();
            }

            return parameter;
        }

        private (bool isCreated, NoteLayoutData layout) GetOrCreateLayout(NoteStartupPosition startupPosition)
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

            var logicalScreenSize = UIUtility.ToLogicalPixel(Model.DockScreen.DeviceBounds.Size, DpiScaleOutpour);
            var layout = new NoteLayoutData() {
                NoteId = NoteId,
                LayoutKind = Model.LayoutKind,
            };

            if(startupPosition == NoteStartupPosition.CenterScreen) {
                if(layout.LayoutKind == NoteLayoutKind.Absolute) {
                    layout.Width = ApplicationConfiguration.Note.LayoutAbsoluteSize.Width;
                    layout.Height = ApplicationConfiguration.Note.LayoutAbsoluteSize.Height;
                    layout.X = (logicalScreenSize.Width / 2) - (layout.Width / 2);
                    layout.Y = (logicalScreenSize.Height / 2) - (layout.Height / 2);
                } else {
                    Debug.Assert(layout.LayoutKind == NoteLayoutKind.Relative);
                    layout.Width = ApplicationConfiguration.Note.LayoutRelativeSize.Width;
                    layout.Height = ApplicationConfiguration.Note.LayoutRelativeSize.Height;
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
                var logicalScreenCursorLocation = UIUtility.ToLogicalPixel(deviceScreenCursorLocation, DpiScaleOutpour);

                if(layout.LayoutKind == NoteLayoutKind.Absolute) {
                    layout.Width = ApplicationConfiguration.Note.LayoutAbsoluteSize.Width;
                    layout.Height = ApplicationConfiguration.Note.LayoutAbsoluteSize.Height;
                    layout.X = logicalScreenCursorLocation.X;
                    layout.Y = logicalScreenCursorLocation.Y;
                } else {
                    Debug.Assert(layout.LayoutKind == NoteLayoutKind.Relative);
                    var area = new Size(
                        deviceScreenBounds.Width / 100,
                        deviceScreenBounds.Height / 100
                    );
                    var center = new Point(
                        deviceScreenBounds.Width / 2,
                        deviceScreenBounds.Height / 2
                    );

                    layout.Width = ApplicationConfiguration.Note.LayoutRelativeSize.Width;
                    layout.Height = ApplicationConfiguration.Note.LayoutRelativeSize.Height;

                    var width = area.Width * layout.Width;
                    var height = area.Height * layout.Height;

                    var x = deviceCursorLocation.X - deviceScreenBounds.X;
                    var y = deviceCursorLocation.Y - deviceScreenBounds.Y;

                    layout.X = ((x) + (width / 2) - center.X) / (area.Width / 2);
                    layout.Y = -((y) + (height / 2) - center.Y) / (area.Height / 2);
                }
            }

            return (true, layout);
        }

        private Rect AbsoluteLayoutToWindow(NoteLayoutData layout)
        {
            var logicalBounds = UIUtility.ToLogicalPixel(Model.DockScreen.DeviceBounds, DpiScaleOutpour);
            return new Rect(
                logicalBounds.X + layout.X,
                logicalBounds.Y + layout.Y,
                layout.Width,
                layout.Height
            );
        }

        private Rect RelativeLayoutToWindow(NoteLayoutData layout)
        {
            var logicalBounds = UIUtility.ToLogicalPixel(Model.DockScreen.DeviceBounds, DpiScaleOutpour);
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

        private Rect CurrentWindowToAbsoluteLayout()
        {
            var logicalScreenLocation = UIUtility.ToLogicalPixel(Model.DockScreen.DeviceBounds.Location, DpiScaleOutpour);
            return new Rect(
                WindowLeft - logicalScreenLocation.X,
                WindowTop - logicalScreenLocation.Y,
                NormalWindowWidth,
                NormalWindowHeight
            );
        }

        private Rect CurrentWindowToRelativeLayout()
        {
            var logicalBounds = UIUtility.ToLogicalPixel(Model.DockScreen.DeviceBounds, DpiScaleOutpour);
            var area = new Size(
                logicalBounds.Width / 100,
                logicalBounds.Height / 100
            );
            var center = new Point(
                logicalBounds.Width / 2,
                logicalBounds.Height / 2
            );

            return new Rect(
                ((WindowLeft - logicalBounds.X) + (NormalWindowWidth / 2) - center.X) / (area.Width / 2),
                -((WindowTop - logicalBounds.Y) + (NormalWindowHeight / 2) - center.Y) / (area.Height / 2),
                NormalWindowWidth / area.Width,
                NormalWindowHeight / area.Height
            );
        }

        private void SetLayout(NoteLayoutData layout)
        {
            var rect = layout.LayoutKind switch {
                NoteLayoutKind.Absolute => AbsoluteLayoutToWindow(layout),
                NoteLayoutKind.Relative => RelativeLayoutToWindow(layout),
                _ => throw new NotImplementedException()
            };
            WindowLeft = rect.X;
            WindowTop = rect.Y;
            NormalWindowWidth = rect.Width;
            NormalWindowHeight = rect.Height;

            if(IsCompact) {
                if(CaptionPosition.IsVertical()) {
                    WindowWidth = NormalWindowWidth;
                    WindowHeight = 0;
                } else {
                    WindowWidth = 0;
                    WindowHeight = NormalWindowHeight;
                }
            } else {
                WindowHeight = NormalWindowHeight;
                WindowWidth = NormalWindowWidth;
            }
        }

        private void ApplyCaption()
        {
            DispatcherWrapper.VerifyAccess();

            var captionPair = NoteTheme.GetCaptionBrush(CaptionPosition, GetColorPair());
            CaptionForeground = captionPair.Foreground;
            CaptionBackground = captionPair.Background;

            var contentPair = NoteTheme.GetContentBrush(CaptionPosition, GetColorPair());
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

        private void ApplyBorder()
        {
            DispatcherWrapper.VerifyAccess();

            var propertyNames = new[] {
                nameof(BorderBrush),
                nameof(BorderThickness),
                nameof(ResizeGripImage),
            };
            foreach(var propertyName in propertyNames) {
                RaisePropertyChanged(propertyName);
            }
        }

        private void ApplyContent()
        {
            DispatcherWrapper.VerifyAccess();

            var propertyNames = new[] {
                nameof(ContentBackground),
            };
            foreach(var propertyName in propertyNames) {
                RaisePropertyChanged(propertyName);
            }
        }

        private void ApplyBlind()
        {
            DispatcherWrapper.VerifyAccess();

            var propertyNames = new[] {
                nameof(BlindEffect),
                nameof(BlindContent),
            };
            foreach(var propertyName in propertyNames) {
                RaisePropertyChanged(propertyName);
            }
        }

        private void ApplyTheme()
        {
            ThrowIfDisposed();

            DispatcherWrapper.BeginAsync(vm => {
                if(vm.IsDisposed) {
                    return;
                }

                vm.ApplyCaption();
                vm.ApplyBorder();
                vm.ApplyContent();
                if(IsVisibleBlind) {
                    vm.ApplyBlind();
                }
            }, this, DispatcherPriority.Render);
        }

        private void DelayNotifyWindowAreaChanged()
        {
            if(IsDisposed) {
                return;
            }

            Logger.LogDebug("モデルへの位置・サイズ通知: {0}, {1}", Model.NoteId, CanLayoutNotify);
            if(!CanLayoutNotify) {
                Logger.LogDebug("モデルへの通知抑制中");
                return;
            }

            var viewAreaChangeTargets = ViewAreaChangeTarget.Location;

            var rect = Model.LayoutKind switch {
                NoteLayoutKind.Absolute => CurrentWindowToAbsoluteLayout(),
                NoteLayoutKind.Relative => CurrentWindowToRelativeLayout(),
                _ => throw new NotImplementedException()
            };
            // 最小化中はウィンドウサイズに対して何もしない
            if(!IsCompact) {
                viewAreaChangeTargets |= ViewAreaChangeTarget.Size;
            }

            Model.ChangeViewAreaDelaySave(viewAreaChangeTargets, rect.Location, rect.Size);
        }

        private ColorPair<Color> GetColorPair() => ColorPair.Create(Model.ForegroundColor, Model.BackgroundColor);

        private IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch(msg) {
                case (int)WM.WM_NCHITTEST: {
                        if(IsLocked) {
                            break;
                        }
                        if(TitleEditMode) {
                            break;
                        }

                        Debug.Assert(CaptionElement != null);

                        var deviceScreenPoint = new Point(
                            WindowsUtility.LOWORD(lParam),
                            WindowsUtility.HIWORD(lParam)
                        );
                        // [#694] スクリーン系は良しなに変換してくれるっぽい？
                        //var logicalScreenPoint = UIUtility.ToLogicalPixel(deviceScreenPoint, DpiScaleOutputor);
                        var logicalPoint = CaptionElement.PointFromScreen(deviceScreenPoint);
                        if(0 <= logicalPoint.X && 0 <= logicalPoint.Y && logicalPoint.X <= CaptionElement.ActualWidth && logicalPoint.Y <= CaptionElement.ActualHeight) {
                            handled = true;
                            return new IntPtr((int)HT.HTCAPTION);
                        }
                        break;
                    }

                case (int)WM.WM_NCLBUTTONDBLCLK:
                    if(WindowsUtility.ConvertHTFromWParam(wParam) == HT.HTCAPTION) {
                        if(!IsLocked) {
                            EditingTitle = Title;
                            TitleEditMode = true;
                        }
                    }
                    break;

                case (int)WM.WM_NCMBUTTONDOWN:
                    if(WindowsUtility.ConvertHTFromWParam(wParam) == HT.HTCAPTION) {
                        if(!IsLocked) {
                            ToggleCompact();
                        }
                    }
                    break;

                case (int)WM.WM_MOVING:
                    Logger.LogDebug("WM_MOVING");
                    if(!WindowMoving) {
                        WindowMoving = true;
                    }
                    break;

                case (int)WM.WM_EXITSIZEMOVE:
                    Logger.LogDebug("WM_EXITSIZEMOVE");
                    if(WindowMoving) {
                        var screen = DpiScaleOutpour.GetOwnerScreen();
                        Model.SaveDisplayDelaySave(screen);
                    }
                    WindowMoving = false;
                    break;

                case (int)WM.WM_SYSCOMMAND: {
                        var sc = WindowsUtility.ConvertSCFromWParam(wParam);
                        switch(sc) {
                            case SC.SC_MINIMIZE:
                                /*
                                if(!IsCompact) {
                                    DispatcherWrapper.BeginAsync(() => {
                                        Model.ToggleCompactDelaySave();
                                    });
                                }
                                */
                                handled = true;
                                break;

                            case SC.SC_MAXIMIZE:
                                handled = true;
                                break;

                            case SC.SC_SIZE:
                                if(IsLocked || IsCompact) {
                                    handled = true;
                                }
                                break;

                            case SC.SC_MOVE:
                                handled = IsLocked;
                                break;

                            default:
                                break;
                        }
                    }
                    break;

                default:
                    break;
            }

            return IntPtr.Zero;
        }

        #region DragAndDrop

        private bool FileCanDragStart(UIElement sender, MouseEventArgs e)
        {
            return false;
        }
        private IResultSuccess<DragParameter> FileGetDragParameter(UIElement sender, MouseEventArgs e)
        {
            throw new NotSupportedException();
        }

        private void FileDragEnterAndOver(UIElement sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
                var filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                if(filePaths.Length == 1) {
                    e.Effects = DragDropEffects.Copy;
                    e.Handled = true;
                }
            }
        }

        private void FileDragLeave(UIElement sender, DragEventArgs e)
        { }

        private async Task FileDropAsync(UIElement sender, DragEventArgs e, CancellationToken cancellationToken)
        {
            if(e.Effects.HasFlag(DragDropEffects.Copy) && e.Data.GetDataPresent(DataFormats.FileDrop)) {
                var filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                if(filePaths.Length == 1) {
                    e.Handled = true;
                    Logger.LogDebug("A");
                    await DispatcherWrapper.BeginAsync(async () => {
                        var result = await Model.AddFileAsync(filePaths[0], NoteFileKind.Reference, cancellationToken);
                        Logger.LogDebug("C: {Result}", result);
                    }, DispatcherPriority.ApplicationIdle);
                    Logger.LogDebug("B");
                }
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

            window.Deactivated += Window_Deactivated;

            // タイトルバーのダブルクリックを拾う必要がある
            var hWndSource = HwndSource.FromHwnd(hWnd);
            hWndSource.AddHook(WndProc);
            WindowHandleSource = hWndSource;

            CaptionElement = ((NoteWindow)window).inputTitle;
            InputSearchElement = ((NoteWindow)window).inputSearch;

            DpiScaleOutpour = (IDpiScaleOutpour)window;

            var layoutValue = GetOrCreateLayout(Model.StartupPosition);
            if(layoutValue.isCreated) {
                Model.SaveLayout(layoutValue.layout);
            }
            Model.ReceiveInitialized();

            SetLayout(layoutValue.layout);

            ApplyTheme();

            CanLayoutNotify = true;
        }

        public void ReceiveViewLoaded(Window window)
        {
            if(IsVisible) {
                if(!IsTopmost) {
                    Model.SetTopmost(true);
                    Model.SetTopmost(false);
                }
            } else {
                window.Visibility = Visibility.Collapsed;
            }
        }

        public void ReceiveViewUserClosing(Window window, CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewUserClosing();
        }

        public void ReceiveViewClosing(Window window, CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewClosing();
        }

        public async Task ReceiveViewClosedAsync(Window window, bool isUserOperation, CancellationToken cancellationToken)
        {
            window.Deactivated -= Window_Deactivated;

            await Model.ReceiveViewClosedAsync(isUserOperation, cancellationToken);

            if(PrepareToRemove) {
                Flush();
                var noteId = Model.NoteId;
                // 削除するにあたりこいつはもう呼び出せない
                Dispose();
                OrderManager.RemoveNoteElement(noteId);

            }
        }

        private bool CanSearchContent()
        {
            return IsSearching && 0 < SearchValue.Length;
        }

        private void SearchContent(string searchValue, bool toNext)
        {
            Logger.LogDebug(toNext ? "Next" : "Prev");

            var focusedInputSearch = InputSearchElement?.IsFocused ?? false;

            Content?.SearchContent(searchValue, toNext);

            if(focusedInputSearch) {
                InputSearchElement?.Focus();
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
                if(disposing) {
                    this._content?.Dispose();
                    this._content = null;
                }
                Flush();
                PlatformTheme.Changed -= PlatformTheme_Changed;
                if(disposing) {
                    WindowHandleSource?.Dispose();
                    PropertyChangedObserver.Dispose();
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

        private void Model_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            PropertyChangedObserver.Execute(e, RaisePropertyChanged);
        }

        private void PlatformTheme_Changed(object? sender, EventArgs e)
        {
            ApplyTheme();
        }

        private void Window_Deactivated(object? sender, EventArgs e)
        {
            if(ShowContentKindChangeConfirm) {
                ShowContentKindChangeConfirm = false;
            }
            if(ShowLinkChangeConfirm) {
                ShowLinkChangeConfirm = false;
            }
            if(IsSearching) {
                IsSearching = false;
            }
        }
    }
}
