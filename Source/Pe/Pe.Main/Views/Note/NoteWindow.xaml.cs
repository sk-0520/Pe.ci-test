using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Standard.DependencyInjection;
using ContentTypeTextNet.Pe.Core.Views;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Note;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using ContentTypeTextNet.Pe.Standard.Base;

namespace ContentTypeTextNet.Pe.Main.Views.Note
{
    /// <summary>
    /// NoteWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class NoteWindow: Window, IDpiScaleOutpour
    {
        public NoteWindow()
        {
            InitializeComponent();

            // あとで考える
            //Language = XmlLanguage.GetLanguage(CultureInfo.CurrentUICulture.Name);
        }

        #region property

        [Inject]
        private ILogger? Logger { get; set; }
        [Inject]
        private ILoggerFactory? LoggerFactory { get; set; }

        private PopupAdjuster? PopupAdjuster { get; set; }

        #endregion

        #region command

        private ICommand? _TitleEditStartCommand;
        public ICommand TitleEditStartCommand => this._TitleEditStartCommand ??= new DelegateCommand<RequestEventArgs>(
            o => {
                this.inputTitle.Focus();
                this.inputTitle.SelectAll();
            }
        );

        private ICommand? _CloseCommand;
        public ICommand CloseCommand => this._CloseCommand ??= new DelegateCommand<RequestEventArgs>(
            o => {
                Close();
            }
        );

        private ICommand? _LinkChangeCommand;
        public ICommand LinkChangeCommand => this._LinkChangeCommand ??= new DelegateCommand<RequestEventArgs>(
            o => {
                var linkParameter = (NoteLinkChangeRequestParameter)o.Parameter;

                FileSystemDialogBase dialog = linkParameter.IsOpen switch {
                    true => new OpenFileDialog(),
                    false => new SaveFileDialog(),
                };
                using(dialog) {
                    dialog.FileName = linkParameter.FilePath;
                    dialog.Filters.SetRange(linkParameter.Filter);

                    var encodingConverter = new EncodingConverter(LoggerFactory!);
                    var encodings = linkParameter.Encodings
                        .Select(i => CustomizeDialogComboBoxItem.Create(encodingConverter.ToDisplayText(i), i))
                        .ToArray()
                    ;
                    var defaultItem = encodings.FirstOrDefault(i => EncodingUtility.ToString(i.Value) == EncodingUtility.ToString(linkParameter.Encoding!));
                    var index = Array.IndexOf(encodings, defaultItem);
                    if(index == -1) {
                        index = 0;
                    }

                    CustomizeDialogComboBox<Encoding>? encodingComboBox = null;
                    if(0 < encodings.Length) {
                        using(dialog.Customize.Grouping(Properties.Resources.String_Command_Encoding_AA)) {
                            encodingComboBox = dialog.Customize.AddComboBox<Encoding>();
                            foreach(var encoding in encodings) {
                                encodingComboBox.AddItem(encoding);
                            }
                            encodingComboBox.SelectedIndex = index;
                        }
                    }

                    if(dialog.ShowDialog(this).GetValueOrDefault()) {
                        o.Callback(new NoteLinkChangeRequestResponse() {
                            ResponseIsCancel = false,
                            ResponseFilePaths = new[] { dialog.FileName },
                            Encoding = 0 < encodings.Length
                                ? encodings[encodingComboBox!.SelectedIndex].Value
                                : linkParameter.Encoding
                            ,
                        });
                    } else {
                        o.Callback(new NoteLinkChangeRequestResponse() {
                            ResponseIsCancel = true,
                        });
                    }
                }
            }
        );

        #endregion

        #region function

        //void SelectLinkFile(NoteLinkSelectNotification context, Action callback)
        //{
        //    var dialog = new CommonSaveFileDialog() {
        //        EnsurePathExists = true,
        //        EnsureValidNames = true,
        //    };

        //    dialog.SetFilters(context.Filter, true, LoggerFactory);

        //    //TODO: メモ帳程度には合わせたい
        //    var encodings = new[] {
        //        EncodingUtility.UTF8n,
        //        Encoding.Unicode,
        //        Encoding.UTF32,
        //        Encoding.Default,
        //        Encoding.UTF8,
        //    };
        //    var encodingControl = new CommonFileDialogComboBox();
        //    foreach(var encoding in encodings) {
        //        var item = new CommonFileDialogComboBoxItem(EncodingUtility.ToString(encoding));
        //        encodingControl.Items.Add(item);
        //    }
        //    encodingControl.SelectedIndex = 0;

        //    dialog.Controls.Add(encodingControl);

        //    using(dialog) {
        //        var popupIsOpen = this.popup.IsOpen;
        //        if(popupIsOpen) {
        //            this.popup.Visibility = Visibility.Collapsed;
        //        }
        //        var result = dialog.ShowDialog();
        //        context.ResponseIsCancel = result != CommonFileDialogResult.Ok;
        //        if(!context.ResponseIsCancel) {
        //            context.ResponseEncoding = encodings[encodingControl.SelectedIndex];
        //            context.ResponseFilePaths = new[] { dialog.FileName };
        //        }
        //        if(popupIsOpen) {
        //            this.popup.Visibility = Visibility.Visible;
        //        }
        //    }

        //    callback();
        //}

        #endregion

        #region Window

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            UIUtility.SetToolWindowStyle(this, false, false);
            PopupAdjuster = new PopupAdjuster(this, this.popup);

#if DEBUG || BETA
            var devElement = new System.Windows.Controls.Border() {
                Background = new SolidColorBrush(Color.FromArgb(0x60, 0xff, 0xff, 0xff)),
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(1),
                Padding = new Thickness(1, 0, 1, 0),
                IsHitTestVisible = false,
                Child = new System.Windows.Controls.TextBlock() {
                    Text = Models.BuildStatus.BuildType.ToString(),
                    Opacity = 0.9,
                    FontSize = 9,
                }
            };
            var grid = UIUtility.FindLogicalChildren<Grid>(this).First();
            grid.Children.Add(devElement);
#endif
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            PopupAdjuster?.Dispose();
        }

        #endregion

        #region IDpiScaleOutputor

        public Point GetDpiScale() => UIUtility.GetDpiScale(this);
        public IScreen GetOwnerScreen() => Screen.FromHandle(HandleUtility.GetWindowHandle(this));

        #endregion

        private void CloseWindowCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CloseWindowCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }
    }
}
