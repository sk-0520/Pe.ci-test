/*
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF.WindowsViewExtend;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility.UI;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.Library.FormsCushion;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.IF;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.PeMain.Define;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.IF.ViewExtend;
using ContentTypeTextNet.Pe.PeMain.Logic.Property;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
using ContentTypeTextNet.Pe.PeMain.View;

namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
    public class NoteViewModel: HasViewSingleModelWrapperViewModelBase<NoteIndexItemModel, NoteWindow>, IHasAppNonProcess, IWindowHitTestData, IWindowAreaCorrectionData, ICaptionDoubleClickData, IHasAppSender, IColorPair, INoteMenuItem
    {
        #region static

        public Thickness CaptionPadding { get { return Constants.noteCaptionPadding; } }

        #endregion

        #region variable

        IndexBodyItemModelBase _indexBody = null;
        double _compactHeight;
        Visibility _titleEditVisibility = Visibility.Collapsed;

        bool _editingTitle = false;
        bool _editingBody = false;

        Brush _borderBrush;

        bool _formatWarning;

        #endregion

        public NoteViewModel(NoteIndexItemModel model, NoteWindow view, IAppNonProcess appNonProcess, IAppSender appSender)
            : base(model, view)
        {
            AppNonProcess = appNonProcess;
            AppSender = appSender;

            BorderBrush = MakeBorderBrush();

            SetCompactArea();

            ResetFormatWarning();

            ResetChangeFlag();
        }

        #region property

        NoteBodyItemModel IndexBody {
            get
            {
                if(this._indexBody == null) {
                    this._indexBody = AppSender.SendLoadIndexBody(Library.PeData.Define.IndexKind.Note, Model.Id);
                    if(this._indexBody == null) {
                        var bodyItem = new NoteBodyItemModel();
                        SettingUtility.InitializeNoteBodyItem(bodyItem, true, AppNonProcess);
                        this._indexBody = bodyItem;
                    }
                }
                return this._indexBody as NoteBodyItemModel;
            }
        }

        public bool IsTemporary { get; set; }

        public Brush BorderBrush
        {
            get { return this._borderBrush; }
            set { SetVariableValue(ref this._borderBrush, value); }
        }

        public double TitleHeight { get { return Constants.noteCaptionHeight + CaptionPadding.GetHorizon(); } }

        public Visibility CaptionButtonVisibility
        {
            get
            {
                if(IsLocked) {
                    return Visibility.Collapsed;
                } else {
                    return Visibility.Visible;
                }
            }
        }

        public Visibility TitleCaptionVisibility
        {
            get { return this._titleEditVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible; }
        }
        public Visibility TitleEditVisibility
        {
            get { return this._titleEditVisibility; }
            set
            {
                if(SetVariableValue(ref this._titleEditVisibility, value)) {
                    OnPropertyChanged(nameof(TitleCaptionVisibility));
                }
            }
        }

        public string Name
        {
            get { return Model.Name; }
            set
            {
                if(SetModelValue(value)) {
                    CallOnPropertyChangeDisplayItem();
                    if(HasView) {
                        var map = new Dictionary<string, string>() {
                            { LanguageKey.noteTitle, Name },
                        };
                        LanguageUtility.SetTitle(View, AppNonProcess.Language, map);
                    }
                }
            }
        }

        public bool IsLocked
        {
            get { return Model.IsLocked; }
            set
            {
                if(SetModelValue(value)) {
                    OnPropertyChanged(nameof(CaptionButtonVisibility));
                    OnPropertyChanged(nameof(IsBodyReadOnly));
                }
            }
        }

        public bool IsCompacted
        {
            get { return Model.IsCompacted; }
            set
            {
                if(SetModelValue(value)) {
                    if(value) {
                        SetCompactArea();
                    }
                    CallOnPropertyChangeDisplayItem();
                    OnPropertyChanged(nameof(WindowHeight));
                }
            }
        }

        public bool IsBodyReadOnly
        {
            get
            {
                if(IsLocked) {
                    return !this._editingBody;
                }

                return IsLocked;
            }
        }

        public bool AutoLineFeed
        {
            get { return Model.AutoLineFeed; }
            set { SetModelValue(value); }
        }

        public string BodyText
        {
            get
            {
                return IndexBody.Text ?? string.Empty;
            }
            set
            {
                if(IsTemporary) {
                    return;
                }
                var indexBody = IndexBody;
                if(SetPropertyValue(indexBody, value, nameof(indexBody.Text))) {
                    indexBody.History.Update();
                    AppSender.SendSaveIndexBody(IndexBody, Model.Id, Timing.Delay);
                }
            }
        }

        public string BodyRtf
        {
            get
            {
                return IndexBody.Rtf ?? string.Empty;
            }
            set
            {
                if(IsTemporary) {
                    return;
                }
                var indexBody = IndexBody;
                if(SetPropertyValue(indexBody, value, nameof(indexBody.Rtf))) {
                    indexBody.History.Update();
                    AppSender.SendSaveIndexBody(IndexBody, Model.Id, Timing.Delay);
                }
            }
        }

        //public FontFamily FontFamily
        //{
        //	get { return FontUtility.MakeFontFamily(Model.Font.Family, SystemFonts.MessageFontFamily); }
        //	set 
        //	{
        //		if(value != null) {
        //			var fontFamily = FontUtility.GetOriginalFontFamilyName(value);
        //			SetPropertyValue(Model.Font, fontFamily, "Family");
        //		}
        //	}
        //}

        //public bool FontBold
        //{
        //	get { return Model.Font.Bold; }
        //	set { SetPropertyValue(Model.Font, value, "Bold"); }
        //}

        //public bool FontItalic
        //{
        //	get { return Model.Font.Italic; }
        //	set { SetPropertyValue(Model.Font, value, "Italic"); }
        //}

        //public double FontSize
        //{
        //	get { return Model.Font.Size; }
        //	set { SetPropertyValue(Model.Font, value, "Size"); }

        //}

        #region font

        public FontFamily FontFamily
        {
            get { return FontModelProperty.GetFamilyDefault(Model.Font); }
            set { FontModelProperty.SetFamily(Model.Font, value, OnPropertyChanged); }
        }

        public bool FontBold
        {
            get { return FontModelProperty.GetBold(Model.Font); }
            set { FontModelProperty.SetBold(Model.Font, value, OnPropertyChanged); }
        }

        public bool FontItalic
        {
            get { return FontModelProperty.GetItalic(Model.Font); }
            set { FontModelProperty.SetItalic(Model.Font, value, OnPropertyChanged); }
        }

        public double FontSize
        {
            get { return FontModelProperty.GetSize(Model.Font); }
            set { FontModelProperty.SetSize(Model.Font, value, OnPropertyChanged); }
        }

        public Brush ForeColorBrush
        {
            get { return new SolidColorBrush(ForeColor); }
        }

        #endregion

        public NoteKind NoteKind
        {
            get { return Model.NoteKind; }
            set
            {
                var prev = Model.NoteKind;
                if(prev != value) {
                    var convertedValue = ConvertBodyValue(prev, value);
                    if(SetModelValue(value)) {
                        switch(value) {
                            case NoteKind.Text:
                                BodyText = convertedValue;
                                break;

                            case NoteKind.Rtf:
                                BodyRtf = convertedValue;
                                break;

                            default:
                                throw new NotImplementedException();
                        }
                        ResetFormatWarning();
                    }
                }
            }
        }

        public bool FormatWarning
        {
            get { return this._formatWarning; }
            set { SetVariableValue(ref this._formatWarning, value); }
        }

        #endregion

        #region command

        public ICommand SaveIndexCommnad
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        EndEditTitle();
                        EndEditBody();

                        if(IsTemporary) {
                            return;
                        }

                        if(IsChanged) {
                            Model.History.Update();
                            AppSender.SendSaveIndex(IndexKind.Note, Timing.Delay);
                            ResetChangeFlag();
                        }
                        if(HasView) {
                            // フォーカス外れたときにうまいこと反映されない対策
                            DoTargetEditor(
                                c => BodyText = c.Text,
                                c => BodyRtf = c.Text
                            );

                            ResetChangeFlag();
                        }
                    }
                );

                return result;
            }
        }

        //public ICommand SwitchCompactCommand
        //{
        //	get
        //	{
        //		var result = CreateCommand(
        //			o => {
        //				IsCompacted = !IsCompacted;
        //			}
        //		);

        //		return result;
        //	}
        //}

        //public ICommand SwitchTopMostCommand
        //{
        //	get
        //	{
        //		var result = CreateCommand(
        //			o => {
        //				IsTopmost = !IsTopmost;
        //			}
        //		);

        //		return result;
        //	}
        //}

        public ICommand HideCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        if(HasView) {
                            // 表示切替はイベント内で実施。
                            View.UserClose();
                        }
                    }
                );

                return result;
            }
        }

        public ICommand RemoveCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        if(HasView) {
                            View.Close();
                        }
                        AppSender.SendRemoveIndex(IndexKind.Note, Model.Id, Timing.Delay);
                    }
                );

                return result;
            }
        }

        public ICommand CopyBodyCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        CopyBody();
                    }
                );

                return result;
            }
        }

        public ICommand EditTitleCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        EditTitle();
                    }
                );

                return result;
            }
        }

        public ICommand HideTitleEditCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        EndEditTitle();
                    }
                );

                return result;
            }
        }

        public ICommand EditBodyCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        EditBody();
                    }
                );

                return result;
            }
        }

        public ICommand ReturnTitleCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        if(this._editingTitle) {
                            EndEditTitle();
                        }
                    }
                );

                return result;
            }
        }

        public ICommand AcceptFormatWarningCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        AcceptFormatWarning();
                    }
                );
            }
        }

        #endregion

        #region HasViewSingleModelWrapperViewModelBase

        protected override void InitializeView()
        {
            SetCompactArea();
            OnPropertyChanged(nameof(IsBodyReadOnly));

            View.UserClosing += View_UserClosing;
            PopupUtility.Attachment(View, View.popup);
            View.popup.Opened += Popup_Opened;

            base.InitializeView();
        }

        protected override void UninitializeView()
        {
            View.popup.Opened -= Popup_Opened;
            View.UserClosing -= View_UserClosing;

            base.UninitializeView();
        }

        protected override void CallOnPropertyChangeDisplayItem()
        {
            base.CallOnPropertyChangeDisplayItem();

            var propertyNames = new[] {
                nameof(MenuIcon),
                nameof(MenuText),
            };
            CallOnPropertyChange(propertyNames);
        }

        #endregion

        #region IColorPair

        public Color ForeColor
        {
            get { return ColorPairProperty.GetNoneAlphaForeColor(Model); }
            set
            {
                if(ColorPairProperty.SetNoneAlphaForekColor(Model, value, OnPropertyChanged)) {
                    CallOnPropertyChange(nameof(ForeColorBrush));
                    CallOnPropertyChangeDisplayItem();
                }
            }
        }

        public Color BackColor
        {
            get { return ColorPairProperty.GetNoneAlphaBackColor(Model); }
            set
            {
                if(ColorPairProperty.SetNoneAlphaBackColor(Model, value, OnPropertyChanged)) {
                    BorderBrush = MakeBorderBrush();
                    CallOnPropertyChangeDisplayItem();
                }
            }
        }

        #endregion

        #region function

        void DoTargetEditor(Action<TextBox> textAction, Action<Xceed.Wpf.Toolkit.RichTextBox> rtfAction)
        {
            var editor = UIUtility.FindLogicalChildren<TextBoxBase>(View.content).First();

            switch(NoteKind) {
                case NoteKind.Text:
                    CastUtility.AsAction(editor, textAction);
                    break;

                case NoteKind.Rtf:
                    CastUtility.AsAction(editor, rtfAction);
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        void CopyBody()
        {
            switch(NoteKind) {
                case NoteKind.Text:
                    if(string.IsNullOrEmpty(BodyText)) {
                        AppNonProcess.Logger.Information("empty body");
                        return;
                    }

                    ClipboardUtility.CopyText(BodyText, AppNonProcess.ClipboardWatcher);
                    break;

                case NoteKind.Rtf:
                    var rtf = BodyRtf;
                    if(string.IsNullOrEmpty(rtf)) {
                        AppNonProcess.Logger.Information("empty body");
                        return;
                    }
                    var converter = new RichTextConverter();
                    var text = converter.ToPlainText(rtf);
                    if(string.IsNullOrEmpty(text)) {
                        AppNonProcess.Logger.Information("empty body(RTF->Text)", rtf);
                        return;
                    }
                    var dataObject = new DataObject();
                    dataObject.SetText(text, TextDataFormat.UnicodeText);
                    dataObject.SetText(rtf, TextDataFormat.Rtf);
                    ClipboardUtility.CopyDataObject(dataObject, AppNonProcess.ClipboardWatcher);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        void ResetFormatWarning()
        {
            FormatWarning = NoteKind == NoteKind.Rtf;
        }

        void AcceptFormatWarning()
        {
            FormatWarning = false;
        }

        void SetCompactArea()
        {
            this._compactHeight = CaptionArea.Height + ResizeThickness.GetVertical();
        }

        void EditTitle()
        {
            TitleEditVisibility = Visibility.Visible;
            this._editingTitle = true;
            if(HasView) {
                View.title.SelectAll();
                View.title.Focus();
            }
        }

        void EndEditTitle()
        {
            if(this._editingTitle) {
                this._editingTitle = false;
                if(HasView) {
                    Name = View.title.Text;
                }
                TitleEditVisibility = Visibility.Collapsed;
            }
        }

        void EditBody()
        {
            this._editingBody = true;
            OnPropertyChanged(nameof(IsBodyReadOnly));
            if(HasView) {
                //TODO: いったん外す
                //if(View.body.SelectionLength == 0) {
                //    View.body.SelectAll();
                //}
                //View.body.Focus();
            }
        }

        void EndEditBody()
        {
            if(this._editingBody) {
                this._editingBody = false;
                OnPropertyChanged(nameof(IsBodyReadOnly));
            }
        }

        Brush MakeBorderBrush()
        {
            var brush = new SolidColorBrush(Model.BackColor);
            FreezableUtility.SafeFreeze(brush);

            return brush;
        }

        string ConvertBodyValue(NoteKind prevKind, NoteKind nextKind)
        {
            Debug.Assert(prevKind != nextKind);

            var converter = new RichTextConverter();

            switch(nextKind) {
                case NoteKind.Text:
                    //if(string.IsNullOrWhiteSpace(BodyText)) {
                        return converter.ToPlainText(BodyRtf);
                    //}

                case NoteKind.Rtf: {
                        var text = converter.ToPlainText(BodyRtf);
                        //if(string.IsNullOrWhiteSpace(text)) {
                            return converter.ToRichText(BodyText, Model.Font);
                        //}
                    }

                default:
                    throw new NotImplementedException();
            }
        }


        #endregion

        #region ITopMost

        public bool IsTopmost
        {
            get { return TopMostProperty.GetTopMost(Model); }
            set { TopMostProperty.SetTopMost(Model, value, OnPropertyChanged); }
        }

        #endregion

        #region IVisible

        public Visibility Visibility
        {
            get { return VisibleVisibilityProperty.GetVisibility(Model); }
            set { VisibleVisibilityProperty.SetVisibility(Model, value, OnPropertyChanged); }
        }

        public bool IsVisible
        {
            get { return VisibleVisibilityProperty.GetVisible(Model); }
            set { VisibleVisibilityProperty.SetVisible(Model, value, OnPropertyChanged); }
        }

        #endregion

        #region window

        public double WindowLeft
        {
            get { return WindowAreaProperty.GetWindowLeft(Model); }
            set
            {
                if(!IsLocked) {
                    WindowAreaProperty.SetWindowLeft(Model, value, OnPropertyChanged);
                }
            }
        }

        public double WindowTop
        {
            get { return WindowAreaProperty.GetWindowTop(Model); }
            set
            {
                if(!IsLocked) {
                    WindowAreaProperty.SetWindowTop(Model, value, OnPropertyChanged);
                }
            }
        }
        public double WindowWidth
        {
            get
            {
                return WindowAreaProperty.GetWindowWidth(Model);
            }
            set
            {
                if(!IsLocked && !IsCompacted) {
                    WindowAreaProperty.SetWindowWidth(Model, value, OnPropertyChanged);
                }
            }
        }
        public double WindowHeight
        {
            get
            {
                if(IsCompacted) {
                    return this._compactHeight;
                } else {
                    return WindowAreaProperty.GetWindowHeight(Model);
                }
            }
            set
            {
                if(!IsLocked && !IsCompacted) {
                    WindowAreaProperty.SetWindowHeight(Model, value, OnPropertyChanged);
                }
            }
        }

        #endregion

        #region IHasAppNonProcess

        public IAppNonProcess AppNonProcess { get; private set; }

        #endregion

        #region IWindowHitTestData

        /// <summary>
        /// ヒットテストを行うか
        /// </summary>
        public bool UsingBorderHitTest { get { return !(IsCompacted || IsLocked); } }

        public bool UsingCaptionHitTest
        {
            get
            {
                if(this._editingTitle) {
                    return false;
                }

                return !IsLocked;
            }
        }

        /// <summary>
        /// タイトルバーとして認識される領域。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Rect CaptionArea
        {
            get
            {
                var resizeThickness = ResizeThickness;
                var rect = new Rect(
                    resizeThickness.Left,
                    resizeThickness.Top,
                    View.caption.ActualWidth,
                    TitleHeight
                );

                return rect;
            }
        }
        /// <summary>
        /// サイズ変更に使用する境界線。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Thickness ResizeThickness { get { return SystemParameters.WindowResizeBorderThickness; } }

        #endregion

        #region IWindowAreaCorrectionData

        /// <summary>
        /// ウィンドウサイズの倍数制御を行うか。
        /// </summary>
        public bool UsingMultipleResize { get { return false; } }
        /// <summary>
        /// ウィンドウサイズの倍数制御に使用する元となる論理サイズ。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Size MultipleSize { get { throw new NotImplementedException(); } }
        /// <summary>
        /// タイトルバーとかボーダーを含んだ領域。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Thickness MultipleThickness { get { throw new NotImplementedException(); } }
        /// <summary>
        /// 移動制限を行うか。
        /// </summary>
        public bool UsingMoveLimitArea { get { return false; } }
        /// <summary>
        /// 移動制限に使用する論理領域。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Rect MoveLimitArea { get { throw new NotImplementedException(); } }
        /// <summary>
        /// 最大化・最小化を抑制するか。
        /// </summary>
        public bool UsingMaxMinSuppression { get { return true; } }

        #endregion

        #region ICaptionDoubleClickData

        public void OnCaptionDoubleClick(object sender, CancelEventArgs e)
        {
            if(IsLocked) {
                e.Cancel = true;
                return;
            }

            EditTitle();
        }

        #endregion

        #region IHavingAppSender

        public IAppSender AppSender { get; private set; }

        #endregion

        #region INoteMenuItem

        public string MenuText { get { return NoteUtility.MakeMenuText(Model); } }

        public FrameworkElement MenuIcon { get { return NoteUtility.MakeMenuIcon(Model); } }

        public ICommand NoteMenuSelectedCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        if(HasView) {
                            View.Activate();
                        }
                    }
                );

                return result;
            }
        }

        #endregion

        #region HasViewSingleModelWrapperViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                AppNonProcess = null;
                AppSender = null;
            }

            base.Dispose(disposing);
        }

        #endregion

        private void View_UserClosing(object sender, CancelEventArgs e)
        {
            IsVisible = false;
        }

        private void Popup_Opened(object sender, EventArgs e)
        {
            ResetFormatWarning();
        }

    }
}
