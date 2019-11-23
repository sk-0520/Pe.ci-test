using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize
{
    public class LauncherItemCustomizeEditorViewModel : SingleModelViewModelBase<LauncherItemCustomizeEditorElement>, ILauncherItemId
    {
        #region variable

        List<LauncherItemCustomizeDetailViewModelBase>? _customizeItems;

        #endregion

        public LauncherItemCustomizeEditorViewModel(LauncherItemCustomizeEditorElement model, ILoggerFactory loggerFactory) : base(model, loggerFactory)
        { }

        #region property

        public List<LauncherItemCustomizeDetailViewModelBase> CustomizeItems
        {
            get
            {
                if(this._customizeItems == null) {
                    this._customizeItems = CreateCustomizeItems().ToList();
                    foreach(var item in this._customizeItems) {
                        item.Initialize();
                    }
                }

                return this._customizeItems;
            }
        }

        public LauncherItemCustomizeCommonViewModel Common => (LauncherItemCustomizeCommonViewModel)CustomizeItems.First(i => i is LauncherItemCustomizeCommonViewModel);

        #endregion


        #region function

        IEnumerable<LauncherItemCustomizeDetailViewModelBase> CreateCustomizeItems()
        {
            yield return new LauncherItemCustomizeCommonViewModel(Model, LoggerFactory);

            switch(Model.Kind) {
                case LauncherItemKind.File:
                    yield return new LauncherItemCustomizeFileViewModel(Model, LoggerFactory);
                    yield return new LauncherItemCustomizeEnvironmentVariableViewModel(Model, LoggerFactory);
                    break;

                default:
                    throw new NotImplementedException();
            }

            yield return new LauncherItemCustomizeTagViewModel(Model, LoggerFactory);
            yield return new LauncherItemCustomizeCommentViewModel(Model, LoggerFactory);
        }

        public void Save()
        {
            var common = CustomizeItems.OfType<LauncherItemCustomizeCommonViewModel>().First();
            var tag = CustomizeItems.OfType<LauncherItemCustomizeTagViewModel>().First();
            var comment = CustomizeItems.OfType<LauncherItemCustomizeCommentViewModel>().First();

            switch(Model.Kind) {
                case LauncherItemKind.File:
                    var file = CustomizeItems.OfType<LauncherItemCustomizeFileViewModel>().First();
                    var env = CustomizeItems.OfType<LauncherItemCustomizeEnvironmentVariableViewModel>().First();

                    var itemData = new LauncherItemData() {
                        LauncherItemId = Model.LauncherItemId,
                        Kind = Model.Kind,
                        Code = common.Code,
                        Name = common.Name,
                        IsEnabledCommandLauncher = true,
                        Comment = comment.CommentDocument!.Text,
                        Icon = new IconData() {
                            Path = common.IconData!.Path,
                            Index = common.IconData!.Index,
                        },
                    };
                    var fileData = new LauncherFileData() {
                        Path = file.Path ?? string.Empty,
                        Option = file.Option ?? string.Empty,
                        WorkDirectoryPath = file.WorkingDirectoryPath ?? string.Empty,
                        IsEnabledCustomEnvironmentVariable = file.IsEnabledCustomEnvironmentVariable,
                        IsEnabledStandardInputOutput = file.IsEnabledStandardInputOutput,
                        StandardInputOutputEncoding = file.StandardInputOutputEncoding ?? EncodingConverter.DefaultStandardInputOutputEncoding,
                        RunAdministrator = file.RunAdministrator,
                    };

                    var envVarItems = env.GetEnvironmentVariableItems();

                    var tagItems = tag.GetTagItems();
                    Model.SaveFile(itemData, fileData, envVarItems, tagItems);
                    break;
            }

        }

        #endregion

        #region ILauncherItemId

        public Guid LauncherItemId => Model.LauncherItemId;

        #endregion

    }
}
