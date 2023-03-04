using System;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Standard.Base;
using ICSharpCode.AvalonEdit.Document;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize
{
    public class LauncherItemCustomizeCommentViewModel: LauncherItemCustomizeDetailViewModelBase, IFlushable
    {
        #region variable

        //TextDocument? _commentDocument;

        #endregion

        public LauncherItemCustomizeCommentViewModel(LauncherItemCustomizeEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            CommentLazyChanger = new LazyAction("コメント編集: " + Model.LauncherItemId.ToString(), TimeSpan.FromSeconds(3), LoggerFactory);

            CommentDocument = new TextDocument(Model.Comment);
            CommentDocument.TextChanged += CommentDocument_TextChanged;
        }

        #region property

        private LazyAction CommentLazyChanger { get; }
        public TextDocument CommentDocument { get; }


        #endregion

        #region command
        #endregion

        #region function

        private void ChangedComment()
        {
            Model.Comment = DispatcherWrapper.Get(() => CommentDocument.Text);
        }

        #endregion

        #region CustomizeLauncherDetailViewModelBase

        protected override void InitializeImpl()
        {
            //CommentDocument = new TextDocument(Model.Comment);
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    CommentLazyChanger.Dispose();
                }

                CommentDocument.TextChanged -= CommentDocument_TextChanged;
            }

            base.Dispose(disposing);
        }

        #endregion

        #region IFlushable
        public void Flush()
        {
            CommentLazyChanger.SafeFlush();
        }
        #endregion

        private void CommentDocument_TextChanged(object? sender, EventArgs e)
        {
            CommentLazyChanger.DelayAction(ChangedComment);
        }
    }
}
