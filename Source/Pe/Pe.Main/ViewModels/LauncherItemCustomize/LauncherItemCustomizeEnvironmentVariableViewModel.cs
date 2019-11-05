using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ICSharpCode.AvalonEdit.Document;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using System.Collections.ObjectModel;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize
{
    public class LauncherItemCustomizeEnvironmentVariableViewModel : LauncherItemCustomizeDetailViewModelBase
    {
        #region variable

        TextDocument? _mergeTextDocument;
        TextDocument? _removeTextDocument;

        #endregion

        public LauncherItemCustomizeEnvironmentVariableViewModel(LauncherItemCustomizeElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property
        #endregion

        #region command

        public TextDocument? MergeTextDocument
        {
            get => this._mergeTextDocument;
            set => SetProperty(ref this._mergeTextDocument, value);
        }
        public TextDocument? RemoveTextDocument
        {
            get => this._removeTextDocument;
            set => SetProperty(ref this._removeTextDocument, value);
        }

        public ObservableCollection<string> MergeErros { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> RemoveErros { get; } = new ObservableCollection<string>();

        #endregion

        #region function

        public IReadOnlyCollection<LauncherEnvironmentVariableData> GetEnvironmentVariableItems()
        {
            var envConf = new EnvironmentVariableConfiguration(LoggerFactory);
            var envMergeItems = envConf.GetMergeItems(MergeTextDocument!);
            var envRemoveItems = envConf.GetRemoveItems(RemoveTextDocument!);
            var envVarItems = envConf.Join(envMergeItems, envRemoveItems);

            return envVarItems;
        }

        #endregion

        #region CustomizeLauncherDetailViewModelBase

        protected override void InitializeImpl()
        {
            var envItems = Model.LoadEnvironmentVariableItems();
            var envConf = new EnvironmentVariableConfiguration(LoggerFactory);

            MergeTextDocument = envConf.CreateMergeDocument(envItems);
            RemoveTextDocument = envConf.CreateRemoveDocument(envItems);
        }

        protected override void ValidateDomain()
        {

            var envConf = new EnvironmentVariableConfiguration(LoggerFactory);

            envConf.SetValidateCommon(MergeTextDocument!, envConf.ValidateMergeDocument, seq => AddErrors(seq, nameof(MergeTextDocument)), MergeErros);
            envConf.SetValidateCommon(RemoveTextDocument!, envConf.ValidateRemoveDocument, seq => AddErrors(seq, nameof(RemoveTextDocument)), RemoveErros);
        }

        #endregion
    }
}
