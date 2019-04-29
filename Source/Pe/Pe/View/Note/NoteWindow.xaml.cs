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
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Logic.CodePack;
using ContentTypeTextNet.Pe.Main.Model.Note;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Dialogs.Controls;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;

namespace ContentTypeTextNet.Pe.Main.View.Note
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
        ILogger Logger { get; set; }

        PopupAttacher PopupAttacher { get; set; }


        #endregion

        #region command

        ICommand _TitleEditStartCommand;
        public ICommand TitleEditStartCommand
        {
            get
            {
                return this._TitleEditStartCommand ?? (this._TitleEditStartCommand = new DelegateCommand<InteractionRequestedEventArgs>(
                    o => {
                        this.inputTitle.Focus();
                        this.inputTitle.SelectAll();
                    }
                ));
            }
        }

        ICommand _SelectLinkFileCommand;
        public ICommand SelectLinkFileCommand
        {
            get
            {
                return this._SelectLinkFileCommand ?? (this._SelectLinkFileCommand = new DelegateCommand<InteractionRequestedEventArgs>(
                    o => {
                        var context = (NoteLinkSelectNotification)o.Context;
                        var dialog = new CommonSaveFileDialog();
                        dialog.SetFilters(context.Filter, true, Logger.Factory);
                        //TODO: メモ量程度には合わせたい
                        var encodings = new CommonFileDialogComboBox();
                        encodings.Items.Add(new CommonFileDialogComboBoxItem("plain"));

                        dialog.Controls.Add(encodings);

                        using(dialog) {
                            var popupIsOpen = this.popup.IsOpen;
                            if(popupIsOpen) {
                                this.popup.Visibility = Visibility.Collapsed;
                            }
                            var result = dialog.ShowDialog();
                            context.ResponseIsCancel = result != CommonFileDialogResult.Ok;
                            if(popupIsOpen) {
                                this.popup.Visibility = Visibility.Visible;
                            }
                        }
                        o.Callback();
                    }
                ));
            }
        }
        #endregion

        #region function
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
