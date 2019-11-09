using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Views;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Note;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;

namespace ContentTypeTextNet.Pe.Main.Views.Note
{
    /// <summary>
    /// NoteWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class NoteWindow : Window, IDpiScaleOutputor
    {
        public NoteWindow()
        {
            InitializeComponent();

            // あとで考える
            //Language = XmlLanguage.GetLanguage(CultureInfo.CurrentUICulture.Name);
        }

        #region property

        [Injection]
        ILogger? Logger { get; set; }
        [Injection]
        ILoggerFactory? LoggerFactory { get; set; }

        PopupAttacher? PopupAttacher { get; set; }

        CommandStore CommandStore { get; } = new CommandStore();

        #endregion

        #region command

        ICommand? _TitleEditStartCommand;
        public ICommand TitleEditStartCommand
        {
            get
            {
                return this._TitleEditStartCommand ?? (this._TitleEditStartCommand = new DelegateCommand<RequestEventArgs>(
                    o => {
                        this.inputTitle.Focus();
                        this.inputTitle.SelectAll();
                    }
                ));
            }
        }

        [Obsolete]
        ICommand? _SelectLinkFileCommand;
        [Obsolete]
        public ICommand SelectLinkFileCommand
        {
            get
            {
                return this._SelectLinkFileCommand ?? (this._SelectLinkFileCommand = new DelegateCommand<RequestEventArgs>(
                    o => {
                        //var context = (NoteLinkSelectNotification)o.Context;
                        //SelectLinkFile(context, o.Callback);
                        throw new NotImplementedException("Obsolete");
                    }
                ));
            }
        }

        ICommand? _CloseCommand;
        public ICommand CloseCommand
        {
            get
            {
                return this._CloseCommand ?? (this._CloseCommand = new DelegateCommand<RequestEventArgs>(
                    o => {
                        Close();
                    }
                ));
            }
        }

        [Obsolete]
        public ICommand UnlinkCommand => CommandStore.GetOrCreate(() => new DelegateCommand<RequestEventArgs>(
            o => {
                throw new NotImplementedException("Obsolete");
            }
        ));

        public ICommand LinkChangeCommand => CommandStore.GetOrCreate(() => new DelegateCommand<RequestEventArgs>(
            o => {
                var linkParameter = (NoteLinkChangeRequestParameter)o.Parameter;

                FileSystemDialogBase dialog = linkParameter.IsOpen switch
                {
                    true => new OpenFileDialog(),
                    false => new SaveFileDialog(),
                };
                using(dialog) {
                    dialog.FileName = linkParameter.FilePath;
                    dialog.Filters.SetRange(linkParameter.Filter);

                    var encodingConverter = new EncodingConverter(LoggerFactory!);

                    var encodings = new[] {
                        CustomizeDialogComboBoxItem.Create(encodingConverter.ToString(encodingConverter.Encoding), encodingConverter.Encoding),
                        CustomizeDialogComboBoxItem.Create(encodingConverter.ToString(Encoding.UTF8), Encoding.UTF8),
                        CustomizeDialogComboBoxItem.Create(encodingConverter.ToString(Encoding.Unicode), Encoding.Unicode),
                        //TODO: 文字コードは追々かんがえるよ
                        CustomizeDialogComboBoxItem.Create(encodingConverter.ToString(Encoding.UTF32), Encoding.UTF32),
                    };
                    var defaultItem = encodings.FirstOrDefault(i => EncodingUtility.ToString(i.Value) == EncodingUtility.ToString(linkParameter.Encoding!));
                    var index = Array.IndexOf(encodings, defaultItem);
                    if(index == -1) {
                        index = 0;
                    }

                    CustomizeDialogComboBox<Encoding> encodingComboBox;
                    using(dialog.Customize.Grouping(nameof(Encoding))) {
                        encodingComboBox = dialog.Customize.AddComboBox<Encoding>();
                        foreach(var encoding in encodings) {
                            encodingComboBox.AddItem(encoding);
                        }
                        encodingComboBox.SelectedIndex = index;
                    }

                    if(dialog.ShowDialog(this).GetValueOrDefault()) {
                        o.Callback(new NoteLinkChangeRequestResponse() {
                            ResponseIsCancel = false,
                            ResponseFilePaths = new[] { dialog.FileName },
                            Encoding = encodings[encodingComboBox.SelectedIndex].Value,
                        });
                    } else {
                        o.Callback(new NoteLinkChangeRequestResponse() {
                            ResponseIsCancel = true,
                        });
                    }
                }
            }
        ));

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
            PopupAttacher = new PopupAttacher(this, this.popup);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            PopupAttacher?.Dispose();
        }

        #endregion

        #region IDpiScaleOutputor

        public Point GetDpiScale() => UIUtility.GetDpiScale(this);
        public Screen GetOwnerScreen() => Screen.FromHandle(HandleUtility.GetWindowHandle(this));

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
