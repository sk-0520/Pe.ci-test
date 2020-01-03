using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.Command;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Theme;
using ContentTypeTextNet.Pe.Main.ViewModels.Font;
using ContentTypeTextNet.Pe.Main.Views.Command;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Command
{
    public class CommandViewModel : SingleModelViewModelBase<CommandElement>, IViewLifecycleReceiver
    {
        #region variable

        bool _isOpend;
        CommandItemViewModel? _currentSelectedItem;
        CommandItemViewModel? _selectedItem;
        string _inputValue = string.Empty;
        InputState _inputState;

        #endregion

        public CommandViewModel(CommandElement model, IGeneralTheme generalTheme, ICommandTheme commandTheme, IPlatformTheme platformTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            GeneralTheme = generalTheme;
            CommandTheme = commandTheme;
            PlatformTheme = platformTheme;
            DispatcherWrapper = dispatcherWrapper;

            ThemeProperties = new ThemeProperties(this);

            CommandItemCollection = new ActionModelViewModelObservableCollectionManager<WrapModel<ICommandItem>, CommandItemViewModel>(Model.CommandItems) {
                ToViewModel = m => new CommandItemViewModel(m.Data, IconBox, DispatcherWrapper, LoggerFactory),
            };
            CommandItems = CommandItemCollection.GetDefaultView();

            Font = new FontViewModel(Model.Font!, DispatcherWrapper, LoggerFactory);

            PlatformTheme.Changed += PlatformTheme_Changed;

            PropertyChangedHooker = new PropertyChangedHooker(DispatcherWrapper, LoggerFactory);
        }


        #region property
        public RequestSender ScrollSelectedItemRequest { get; } = new RequestSender();

        IGeneralTheme GeneralTheme { get; }
        ICommandTheme CommandTheme { get; }
        IPlatformTheme PlatformTheme { get; }
        IDispatcherWrapper DispatcherWrapper { get; }

        ModelViewModelObservableCollectionManagerBase<WrapModel<ICommandItem>, CommandItemViewModel> CommandItemCollection { get; }
        public ICollectionView CommandItems { get; }

        public CommandItemViewModel? CurrentSelectedItem
        {
            get => this._currentSelectedItem;
            set => SetProperty(ref this._currentSelectedItem, value);
        }

        public CommandItemViewModel? SelectedItem
        {
            get => this._selectedItem;
            set
            {
                SetProperty(ref this._selectedItem, value);
                if(SelectedItem != null) {
                    CurrentSelectedItem = SelectedItem;
                }
            }
        }

        ThemeProperties ThemeProperties { get; }
        PropertyChangedHooker PropertyChangedHooker { get; }

        IDpiScaleOutputor DpiScaleOutputor { get; set; } = new EmptyDpiScaleOutputor();
        TextBox? InputCommand { get; set; }

        public bool IsOpend
        {
            get => this._isOpend;
            set => SetProperty(ref this._isOpend, value);
        }

        public IconBox IconBox => Model.IconBox;

        public string InputValue
        {
            get => this._inputValue;
            set
            {
                CurrentSelectedItem = SelectedItem;
                SetProperty(ref this._inputValue, value);

                var isEmpty = string.IsNullOrWhiteSpace(this._inputValue);
                if(isEmpty) {
                    InputState = InputState.Empty;
                } else {
                    InputState = InputState.Finding;
                }

                Model.UpdateCommandItemsAsync(this._inputValue).ContinueWith(t => {
                    SelectedItem = CommandItemCollection.ViewModels.FirstOrDefault();

                    if(SelectedItem == null && !string.IsNullOrWhiteSpace(this._inputValue)) {
                        CurrentSelectedItem = null;
                        InputState = InputState.NotFound;
                    } else {
                        InputState = InputState.Listup;
                    }
                });
            }
        }

        public FontViewModel Font { get; private set; }

        public InputState InputState
        {
            get => this._inputState;
            set => SetProperty(ref this._inputState, value);
        }

        #region theme

        [ThemeProperty]
        public Thickness ViewBorderThickness => CommandTheme.GetViewBorderThickness();
        [ThemeProperty]
        public Thickness ViewResizeThickness
        {
            get
            {
                var thickness = CommandTheme.GetViewBorderThickness();
                thickness.Top = 0;
                thickness.Bottom = 0;
                return thickness;
            }
        }
        [ThemeProperty]
        public Brush ViewActiveBorderBrush => CommandTheme.GetViewBorderBrush(true);
        [ThemeProperty]
        public Brush ViewInactiveBorderBrush => CommandTheme.GetViewBorderBrush(false);

        [ThemeProperty]
        public Brush ViewActiveBackgroundBrush => CommandTheme.GetViewBackgroundBrush(true);
        [ThemeProperty]
        public Brush ViewInactiveBackgroundBrush => CommandTheme.GetViewBackgroundBrush(false);
        [ThemeProperty]
        public double GripWidth => CommandTheme.GetGripWidth();
        [ThemeProperty]
        public Brush GripActiveBrush => CommandTheme.GetGripBrush(true);
        [ThemeProperty]
        public Brush GripInactiveBrush => CommandTheme.GetGripBrush(true);

        [ThemeProperty]
        public Thickness SelectedIconMargin => CommandTheme.GetSelectedIconMargin(IconBox);

        [ThemeProperty]
        public Thickness InputBorderThickness => CommandTheme.GetInputBorderThickness();

        [ThemeProperty]
        public Brush InputEmptyBorderBrush => CommandTheme.GetInputBorderBrush(InputState.Empty);
        [ThemeProperty]
        public Brush InputFindingBorderBrush => CommandTheme.GetInputBorderBrush(InputState.Finding);
        [ThemeProperty]
        public Brush InputListupBorderBrush => CommandTheme.GetInputBorderBrush(InputState.Listup);
        [ThemeProperty]
        public Brush InputNotFoundBorderBrush => CommandTheme.GetInputBorderBrush(InputState.NotFound);

        [ThemeProperty]
        public Brush InputEmptyBackground => CommandTheme.GetInputBackground(InputState.Empty);
        [ThemeProperty]
        public Brush InputFindingBackground => CommandTheme.GetInputBackground(InputState.Finding);
        [ThemeProperty]
        public Brush InputListupBackground => CommandTheme.GetInputBackground(InputState.Listup);
        [ThemeProperty]
        public Brush InputNotFoundBackground => CommandTheme.GetInputBackground(InputState.NotFound);

        [ThemeProperty]
        public Brush InputEmptyForeground => CommandTheme.GetInputForeground(InputState.Empty);
        [ThemeProperty]
        public Brush InputFindingForeground => CommandTheme.GetInputForeground(InputState.Finding);
        [ThemeProperty]
        public Brush InputListupForeground => CommandTheme.GetInputForeground(InputState.Listup);
        [ThemeProperty]
        public Brush InputNotFoundForeground => CommandTheme.GetInputForeground(InputState.NotFound);

        #endregion

        #endregion

        #region command

        public ICommand ExecuteCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Logger.LogInformation("exec!");
            },
            () => SelectedItem != null
        ));

        public ICommand UpSelectItemCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                UpDownSelectItem(true);
            }
        ));

        public ICommand DownSelectItemCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                UpDownSelectItem(false);
            }
        ));
        #endregion

        #region function

        void UpDownSelectItem(bool isUp)
        {
            if(CommandItemCollection.ViewModels.Count == 0) {
                Logger.LogTrace("列挙アイテムなし");
                return;
            }

            if(SelectedItem == null) {
                // 多分ここには来ないはずだけど一応
                SelectedItem = isUp
                    ? CommandItemCollection.ViewModels.First()
                    : CommandItemCollection.ViewModels.Last()
                ;
            } else {
                var index = CommandItemCollection.ViewModels.IndexOf(SelectedItem);
                if(isUp) {
                    SelectedItem = index == 0
                        ? CommandItemCollection.ViewModels.Last()
                        : CommandItemCollection.ViewModels[index - 1]
                    ;
                } else {
                    SelectedItem = index == CommandItemCollection.ViewModels.Count - 1
                        ? CommandItemCollection.ViewModels[0]
                        : CommandItemCollection.ViewModels[index + 1]
                    ;
                }
            }

            ScrollSelectedItemRequest.Send();
        }

        #endregion

        #region IViewLifecycleReceiver

        public void ReceiveViewInitialized(Window window)
        {
            DpiScaleOutputor = (IDpiScaleOutputor)window;
        }

        public void ReceiveViewLoaded(Window window)
        {
            Model.UpdateCommandItemsAsync(string.Empty).ContinueWith(t => {
                SelectedItem = CommandItemCollection.ViewModels.FirstOrDefault();
            });
        }

        public void ReceiveViewUserClosing(CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewUserClosing();
            Model.Hide();
        }

        public void ReceiveViewClosing(CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewClosing();
        }

        public void ReceiveViewClosed()
        {
            Model.ReceiveViewClosed();
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
                    PlatformTheme.Changed -= PlatformTheme_Changed;
                    PropertyChangedHooker.Dispose();
                    CommandItemCollection.Dispose();
                    Font.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        private void PlatformTheme_Changed(object? sender, EventArgs e)
        {
            DispatcherWrapper.Begin(() => {
                foreach(var themePropertyName in ThemeProperties.GetPropertyNames()) {
                    RaisePropertyChanged(themePropertyName);
                }
            }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChangedHooker.Execute(e, RaisePropertyChanged);
        }


    }
}
