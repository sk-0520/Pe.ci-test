using System;
using System.Collections.ObjectModel;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ICSharpCode.AvalonEdit.Document;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Library.Base;
using ContentTypeTextNet.Pe.Library.Base.Linq;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize
{
    public class LauncherItemCustomizeEnvironmentVariableViewModel: LauncherItemCustomizeDetailViewModelBase, IFlushable
    {
        #region variable

        //TextDocument? _mergeTextDocument;
        //TextDocument? _removeTextDocument;

        #endregion

        public LauncherItemCustomizeEnvironmentVariableViewModel(LauncherItemCustomizeEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            EnvironmentVariableDelayChanger = new DelayAction("環境変数編集:" + Model.LauncherItemId.ToString(), TimeSpan.FromSeconds(5), LoggerFactory);
        }

        #region property
        private DelayAction EnvironmentVariableDelayChanger { get; }
        public TextDocument? MergeTextDocument { get; private set; }
        public TextDocument? RemoveTextDocument { get; private set; }

        #endregion

        #region command

        public ObservableCollection<string> MergeErrors { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> RemoveErrors { get; } = new ObservableCollection<string>();

        #endregion

        #region function

        void ChangedEnvironmentVariable()
        {
            var envConf = new EnvironmentVariableConfiguration(LoggerFactory);

            var envMergeItems = DispatcherWrapper.Get(() => envConf.GetMergeItems(MergeTextDocument!));
            var envRemoveItems = DispatcherWrapper.Get(() => envConf.GetRemoveItems(RemoveTextDocument!));
            var envVarItems = envConf.Join(envMergeItems, envRemoveItems);

            Model.EnvironmentVariableItems!.SetRange(envVarItems);
        }

        #endregion

        #region CustomizeLauncherDetailViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    EnvironmentVariableDelayChanger.Dispose();
                }

                if(MergeTextDocument != null) {
                    MergeTextDocument.TextChanged -= TextDocument_TextChanged;
                }
                if(RemoveTextDocument != null) {
                    RemoveTextDocument.TextChanged -= TextDocument_TextChanged;
                }
            }

            base.Dispose(disposing);
        }

        protected override void InitializeImpl()
        {
            if(Model.IsLazyLoad) {
                return;
            }

            var envItems = Model.EnvironmentVariableItems!;
            var envConf = new EnvironmentVariableConfiguration(LoggerFactory);
            MergeTextDocument = envConf.CreateMergeDocument(envItems);
            RemoveTextDocument = envConf.CreateRemoveDocument(envItems);

            MergeTextDocument.TextChanged += TextDocument_TextChanged;
            RemoveTextDocument.TextChanged += TextDocument_TextChanged;
        }

        protected override void ValidateDomain()
        {

            var envConf = new EnvironmentVariableConfiguration(LoggerFactory);

            envConf.SetValidateCommon(MergeTextDocument!, envConf.ValidateMergeDocument, seq => AddErrors(seq, nameof(MergeTextDocument)), MergeErrors);
            envConf.SetValidateCommon(RemoveTextDocument!, envConf.ValidateRemoveDocument, seq => AddErrors(seq, nameof(RemoveTextDocument)), RemoveErrors);
        }

        #endregion

        #region IFlushable

        public void Flush()
        {
            EnvironmentVariableDelayChanger.SafeFlush();
        }

        #endregion

        private void TextDocument_TextChanged(object? sender, EventArgs e)
        {
            if(Model.IsLazyLoad) {
                return;
            }

            EnvironmentVariableDelayChanger.Callback(ChangedEnvironmentVariable);
        }
    }
}
