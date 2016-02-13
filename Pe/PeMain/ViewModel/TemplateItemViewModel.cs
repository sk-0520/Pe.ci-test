/**
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
namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
    using System;
    using System.Windows.Input;
    using System.Windows.Media;
    using ContentTypeTextNet.Library.SharedLibrary.View.Window;
    using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
    using ContentTypeTextNet.Pe.Library.PeData.Define;
    using ContentTypeTextNet.Pe.Library.PeData.Item;
    using ContentTypeTextNet.Pe.PeMain.Define;
    using ContentTypeTextNet.Pe.PeMain.IF;
    using ContentTypeTextNet.Pe.PeMain.Logic;
    using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
    using ICSharpCode.AvalonEdit.Document;

    public class TemplateItemViewModel: HasViewSingleModelWrapperBodyViewModelBase<TemplateIndexItemModel, TemplateBodyItemModel>
    {
        #region variable

        bool _replaceViewSelected = false;
        string _replaced;
        TextDocument _document;

        #endregion

        public TemplateItemViewModel(TemplateIndexItemModel model, IAppSender appSender, IAppNonProcess appNonProcess)
            : base(model, appSender, appNonProcess)
        { }

        #region property

        ProgramTemplateProcessor Processor { get; set; }

        public string Name
        {
            get { return Model.Name; }
            set { SetModelValue(value); }
        }

        public TemplateReplaceMode TemplateReplaceMode
        {
            get { return Model.TemplateReplaceMode; }
            set
            {
                if(SetModelValue(value)) {
                    OnPropertyChangedItemType();
                }
            }
        }

        public bool ReplaceViewSelected
        {
            get { return this._replaceViewSelected; }
            set
            {
                if(SetVariableValue(ref this._replaceViewSelected, value)) {
                    if(value) {
                        SetReplacedValue();
                    }
                }
            }
        }

        public string Source
        {
            get { return BodyModel.Source; }
            set { SetPropertyValue(BodyModel, value); }
        }

        public TextDocument Document
        {
            get
            {
                if(this._document == null) {
                    this._document = new TextDocument(Source ?? string.Empty);
                    this._document.TextChanged += Document_TextChanged;
                }
                return this._document;
            }
        }

        public string Replaced
        {
            get { return this._replaced; }
            set { SetVariableValue(ref this._replaced, value); }
        }

        public bool IsReplace
        {
            get { return TemplateReplaceMode != TemplateReplaceMode.None; }
        }

        public string ItemTypeText
        {
            get
            {
                switch(TemplateReplaceMode) {
                    case TemplateReplaceMode.None:
                        return AppNonProcess.Language["template/replace/plain"];
                    case TemplateReplaceMode.Text:
                        return AppNonProcess.Language["template/replace/text"];
                    case TemplateReplaceMode.Program:
                        return AppNonProcess.Language["template/replace/program"];
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        #endregion

        #region command

        public ICommand SendItemCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var apiWindow = (WindowsAPIWindowBase)o;
                        var hWnd = apiWindow.Handle;
                        // TODO: なんだかなぁ。
                        SetReplacedValue();
                        ClipboardUtility.OutputTextForNextWindow(hWnd, Replaced, AppNonProcess, AppNonProcess.ClipboardWatcher);
                    }
                );

                return result;
            }
        }

        public ICommand CopyItemCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        SetReplacedValue();
                        ClipboardUtility.CopyText(Replaced, AppNonProcess.ClipboardWatcher);
                        AppNonProcess.Logger.Trace("copy: " + Name);
                    }
                );

                return result;
            }
        }

        #endregion

        #region function

        void OnPropertyChangedItemType()
        {
            var propertyNames = new[] {
                nameof(IsReplace),
                nameof(ItemTypeText),
            };
            CallOnPropertyChange(propertyNames);
        }

        public void SetReplacedValue()
        {
            if(TemplateReplaceMode == TemplateReplaceMode.None) {
                Replaced = Source ?? string.Empty;
            } else {
                if(TemplateReplaceMode == TemplateReplaceMode.Program) {
                    Processor = TemplateUtility.MakeTemplateProcessor(BodyModel.Source, Processor, AppNonProcess);
                }
                Replaced = TemplateUtility.ToPlainText(Model, BodyModel, Processor, DateTime.Now, AppNonProcess);
            }
        }

        #endregion

        #region HasViewSingleModelWrapperBodyViewModelBase

        protected override IndexKind IndexKind { get { return IndexKind.Template; } }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(Processor != null) {
                    Processor.Dispose();
                    Processor = null;
                }
                if(this._document != null) {
                    this._document.TextChanged -= Document_TextChanged;
                    this._document = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        private void Document_TextChanged(object sender, EventArgs e)
        {
            Source = Document.Text;
        }

    }
}
