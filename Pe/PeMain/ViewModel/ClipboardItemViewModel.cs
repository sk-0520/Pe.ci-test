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
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.View.Window;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Item;
//using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
    public class ClipboardItemViewModel: HasViewSingleModelWrapperBodyViewModelBase<ClipboardIndexItemModel, ClipboardBodyItemModel>
    {
        #region define

        const string defineEnabled = "Type";

        #endregion

        public ClipboardItemViewModel(ClipboardIndexItemModel model, IAppSender appSender, IAppNonProcess appNonProcess)
            : base(model, appSender, appNonProcess)
        { }

        #region property

        ContentTypeTextNet.Pe.PeMain.Data.ClipboardHtmlData HtmlModel { get; set; }

        public ImageSource ItemTypeImage
        {
            get
            {
                var type = ClipboardUtility.GetSingleClipboardType(Model.Type);
                var map = new Dictionary<ClipboardType, ImageSource>() {
                    { ClipboardType.Text, AppResource.ClipboardTextImage },
                    { ClipboardType.Rtf, AppResource.ClipboardRtfImage },
                    { ClipboardType.Html, AppResource.ClipboardHtmlImage },
                    { ClipboardType.Image, AppResource.ClipboardImageImage },
                    { ClipboardType.Files, AppResource.ClipboardFileImage },
                };

                return map[type];
            }
        }

        public DateTime CreateTimestamp { get { return Model.History.CreateTimestamp; } }

        public string Name
        {
            get { return Model.Name; }
            set { SetModelValue(value); }
        }

        #region Type

        public bool EnabledClipboardTypesText
        {
            get { return Model.Type.HasFlag(ClipboardType.Text); }
            set { SetClipboardType(Model, Model.Type, ClipboardType.Text, nameof(Model.Type)); }
        }
        public bool EnabledClipboardTypesRtf
        {
            get { return Model.Type.HasFlag(ClipboardType.Rtf); }
            set { SetClipboardType(Model, Model.Type, ClipboardType.Rtf, nameof(Model.Type)); }
        }
        public bool EnabledClipboardTypesHtml
        {
            get { return Model.Type.HasFlag(ClipboardType.Html); }
            set { SetClipboardType(Model, Model.Type, ClipboardType.Html, nameof(Model.Type)); }
        }
        public bool EnabledClipboardTypesImage
        {
            get { return Model.Type.HasFlag(ClipboardType.Image); }
            set { SetClipboardType(Model, Model.Type, ClipboardType.Image, nameof(Model.Type)); }
        }
        public bool EnabledClipboardTypesFiles
        {
            get { return Model.Type.HasFlag(ClipboardType.Files); }
            set { SetClipboardType(Model, Model.Type, ClipboardType.Files, nameof(Model.Type)); }
        }

        #endregion

        public string Text
        {
            get { return BodyModel.Text ?? string.Empty; }
        }

        //public FlowDocument Rtf
        //{
        //	get
        //	{
        //		//return BodyModel.Rtf;
        //		var result = new FlowDocument();
        //		return result;
        //	}
        //}

        public string Rtf
        {
            get { return BodyModel.Rtf ?? string.Empty; }
            set { /* dummy Mode=OneWay */}
        }

        public string HtmlCode
        {
            get
            {
                if(HtmlModel != null) {
                    return HtmlModel.ToHtml();
                } else {
                    return null;
                }
            }
        }

        public string HtmlUri
        {
            get
            {
                if(HtmlModel != null && HtmlModel.SourceURL != null) {
                    return HtmlModel.SourceURL.OriginalString;
                } else {
                    return string.Empty;
                }
            }
        }

        public BitmapSource Image
        {
            get
            {
                if(BodyModel.Image != null) {
                    return BodyModel.Image;
                } else {
                    return null;
                }
            }
        }

        public IEnumerable<ClipboardFileItemViewModel> Files
        {
            get
            {
                if(BodyModel.Files != null && BodyModel.Files.Any()) {
                    return BodyModel.Files.Select(f => new ClipboardFileItemViewModel() {
                        Path = f,
                        Name = SystemEnvironmentUtility.IsExtensionShow() ? Path.GetFileName(f) : Path.GetFileNameWithoutExtension(f),
                    });
                } else {
                    return null;
                }
            }
        }

        public DateTime Sort
        {
            get { return Model.Sort; }
            set { SetModelValue(value); }
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

                        ClipboardUtility.OutputTextForNextWindow(hWnd, Text, AppNonProcess, AppNonProcess.ClipboardWatcher);
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
                        var clipboardItem = new ClipboardData() {
                            Type = Model.Type,
                            Body = BodyModel,
                        };

                        //TODO: #429暫定対応なので原因解明が必要
                        try {
                            ClipboardUtility.CopyClipboardItem(clipboardItem, AppNonProcess.ClipboardWatcher);
                            AppNonProcess.Logger.Trace("copy: " + Name);
                        } catch(ArgumentNullException ex) {
                            AppNonProcess.Logger.Warning(ex);
                        }
                    }
                );

                return result;
            }
        }

        public ICommand CopyTextCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        ClipboardUtility.CopyText(Text, AppNonProcess.ClipboardWatcher);
                    }
                );

                return result;
            }
        }

        public ICommand CopyRtfCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        ClipboardUtility.CopyRtf(Rtf, AppNonProcess.ClipboardWatcher);
                    }
                );

                return result;
            }
        }

        public ICommand CopyHtmlCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        ClipboardUtility.CopyHtml(BodyModel.Html, AppNonProcess.ClipboardWatcher);
                    }
                );

                return result;
            }
        }

        public ICommand CopyImageCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        ClipboardUtility.CopyImage(BodyModel.Image, AppNonProcess.ClipboardWatcher);
                    }
                );

                return result;
            }
        }

        public ICommand CopyFilesCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        ClipboardUtility.CopyFile(BodyModel.Files, AppNonProcess.ClipboardWatcher);
                    }
                );

                return result;
            }
        }

        public ICommand CopyHtmlUriCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        ClipboardUtility.CopyText(HtmlUri, AppNonProcess.ClipboardWatcher);
                    }
                );

                return result;
            }
        }

        public ICommand OpenUriCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        try {
                            ExecuteUtility.ExecuteCommand(HtmlUri, AppNonProcess);
                        } catch(Exception ex) {
                            AppNonProcess.Logger.Warning(ex);
                        }
                    },
                    o => {
                        return EnabledClipboardTypesHtml && !string.IsNullOrWhiteSpace(HtmlUri);
                    }
                );

                return result;
            }
        }

        #endregion

        #region function

        void SetClipboardType(object obj, ClipboardType nowValue, ClipboardType clipboardType, string memberName, [CallerMemberName]string propertyName = "")
        {
            SetPropertyValue(obj, nowValue ^ clipboardType, memberName, propertyName);
        }

        #endregion

        #region HasViewSingleModelWrapperBodyViewModelBase

        protected override IndexKind IndexKind { get { return IndexKind.Clipboard; } }

        protected override void CorrectionBodyModel(ClipboardBodyItemModel bodyModel)
        {
            if(Model.Type.HasFlag(ClipboardType.Html)) {
                HtmlModel = ClipboardUtility.ConvertClipboardHtmlFromFromRawHtml(bodyModel.Html ?? string.Empty, AppNonProcess);
            } else {
                HtmlModel = null;
            }
        }

        #endregion
    }
}
