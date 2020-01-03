using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.Command;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Theme;
using ContentTypeTextNet.Pe.Main.Views.Command;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Command
{
    public class CommandViewModel : SingleModelViewModelBase<CommandElement>, IViewLifecycleReceiver
    {
        #region variable

        bool _isOpend;
        CommandItemViewModel? _selectedItem;

        #endregion

        public CommandViewModel(CommandElement model, IGeneralTheme generalTheme, ICommandTheme commandTheme, IPlatformTheme platformTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            GeneralTheme = generalTheme;
            CommandTheme = commandTheme;
            PlatformTheme = platformTheme;
            DispatcherWrapper = dispatcherWrapper;

            ThemeProperties = new ThemeProperties(this);

            CommandItemCollection = new ActionModelViewModelObservableCollectionManager<WrapModel<IReadOnlyCommandItem>, CommandItemViewModel>(Model.CommandItems) {
                ToViewModel = m => new CommandItemViewModel(m.Data, IconBox, DispatcherWrapper, LoggerFactory),
            };
            CommandItems = CommandItemCollection.GetDefaultView();

            PlatformTheme.Changed += PlatformTheme_Changed;

            PropertyChangedHooker = new PropertyChangedHooker(DispatcherWrapper, LoggerFactory);
        }


        #region property
        IGeneralTheme GeneralTheme { get; }
        ICommandTheme CommandTheme { get; }
        IPlatformTheme PlatformTheme { get; }
        IDispatcherWrapper DispatcherWrapper { get; }

        ModelViewModelObservableCollectionManagerBase<WrapModel<IReadOnlyCommandItem>, CommandItemViewModel> CommandItemCollection { get; }
        public ICollectionView CommandItems { get; }

        public CommandItemViewModel? SelectedItem
        {
            get => this._selectedItem;
            set => SetProperty(ref this._selectedItem, value);
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
        public Thickness InputBorderThickness => CommandTheme.GetInputBorderThickness();

        [ThemeProperty]
        public Brush InputEmptyBorderBrush => CommandTheme.GetInputBorderBrush(InputState.Empty);
        [ThemeProperty]
        public Brush InputFindingBorderBrush => CommandTheme.GetInputBorderBrush(InputState.Finding);
        [ThemeProperty]
        public Brush InputNotFoundBorderBrush => CommandTheme.GetInputBorderBrush(InputState.NotFound);

        [ThemeProperty]
        public Brush InputEmptyBackground => CommandTheme.GetInputBackground(InputState.Empty);
        [ThemeProperty]
        public Brush InputFindingBackground => CommandTheme.GetInputBackground(InputState.Finding);
        [ThemeProperty]
        public Brush InputNotFoundBackground => CommandTheme.GetInputBackground(InputState.NotFound);

        [ThemeProperty]
        public Brush InputEmptyForeground => CommandTheme.GetInputForeground(InputState.Empty);
        [ThemeProperty]
        public Brush InputFindingForeground => CommandTheme.GetInputForeground(InputState.Finding);
        [ThemeProperty]
        public Brush InputNotFoundForeground => CommandTheme.GetInputForeground(InputState.NotFound);

        #endregion

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region IViewLifecycleReceiver

        public void ReceiveViewInitialized(Window window)
        {
            DpiScaleOutputor = (IDpiScaleOutputor)window;

            var commandWindow = (CommandWindow)window;
            InputCommand = commandWindow.inputCommand;

            InputCommand.TextChanged += InputCommand_TextChanged;
        }



        public void ReceiveViewLoaded(Window window)
        {
            Model.UpdateCommandItemsAsync(string.Empty);
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
            if(InputCommand != null) {
                InputCommand.TextChanged -= InputCommand_TextChanged;
            }

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
                    if(InputCommand != null) {
                        InputCommand.TextChanged -= InputCommand_TextChanged;
                    }

                    PlatformTheme.Changed -= PlatformTheme_Changed;
                    PropertyChangedHooker.Dispose();
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

        private void InputCommand_TextChanged(object sender, TextChangedEventArgs e)
        {
            Model.UpdateCommandItemsAsync(InputCommand!.Text).ContinueWith(t => {
                SelectedItem = CommandItemCollection.ViewModels.FirstOrDefault();
            });
        }

    }
}
