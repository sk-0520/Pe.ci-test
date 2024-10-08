using System;
using System.ComponentModel;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.Base;
using WinForms = System.Windows.Forms;

namespace ContentTypeTextNet.Pe.Core.Compatibility.Forms
{
    /// <summary>
    /// <see cref="System.Windows.Forms.FolderBrowserDialog"/>互換クラス。
    /// </summary>
    [Obsolete("気が向いたら破棄する")]
    public class FolderBrowserDialog: DisposerBase
    {
        public FolderBrowserDialog()
            : base()
        {
            Dialog = new WinForms.FolderBrowserDialog();
        }

        #region property

        private WinForms.FolderBrowserDialog Dialog { get; set; }

        /// <inheritdoc cref="System.ComponentModel.Component.Container"/>
        [BrowsableAttribute(false)]
        public IContainer? Container { get { return Dialog.Container; } }

        /// <inheritdoc cref="WinForms.FolderBrowserDialog.Description"/>
        [BrowsableAttribute(true)]
        public string Description
        {
            get { return Dialog.Description; }
            set { Dialog.Description = value; }
        }

        /// <inheritdoc cref="WinForms.FolderBrowserDialog.RootFolder"/>
        [BrowsableAttribute(true)]
        public Environment.SpecialFolder RootFolder
        {
            get { return Dialog.RootFolder; }
            set { Dialog.RootFolder = value; }
        }

        /// <inheritdoc cref="WinForms.FolderBrowserDialog.SelectedPath"/>
        [BrowsableAttribute(true)]
        public string SelectedPath
        {
            get { return Dialog.SelectedPath; }
            set { Dialog.SelectedPath = value; }
        }

        /// <inheritdoc cref="WinForms.FolderBrowserDialog.ShowNewFolderButton"/>
        [BrowsableAttribute(true)]
        public bool ShowNewFolderButton
        {
            get { return Dialog.ShowNewFolderButton; }
            set { Dialog.ShowNewFolderButton = value; }
        }

        /// <inheritdoc cref="System.ComponentModel.IComponent.Site"/>
        [BrowsableAttribute(false)]
        public virtual ISite? Site
        {
            get { return Dialog.Site; }
            set { Dialog.Site = value; }
        }

        /// <inheritdoc cref="WinForms.Control.Tag"/>
        [BindableAttribute(true)]
        public object? Tag
        {
            get { return Dialog.Tag; }
            set { Dialog.Tag = value; }
        }

        #endregion

        #region function

        /// <inheritdoc cref="WinForms.CommonDialog.ShowDialog"/>
        public bool? ShowDialog()
        {
            var compatibleResult = Dialog.ShowDialog();
            return compatibleResult == WinForms.DialogResult.OK;
        }

        /// <inheritdoc cref="WinForms.CommonDialog.ShowDialog(WinForms.IWin32Window)"/>
        public bool? ShowDialog(Window owner)
        {
            var form = new CompatibleFormWindow(owner);
            var compatibleResult = Dialog.ShowDialog(form);
            return compatibleResult == WinForms.DialogResult.OK;
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(Dialog != null) {
                    Dialog.Dispose();
                    Dialog = null!;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
