/*
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forms = System.Windows.Forms;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using System.ComponentModel;
using System.Windows;

namespace ContentTypeTextNet.Library.SharedLibrary.CompatibleForms
{
    /// <summary>
    /// <see cref="System.Windows.Forms.FolderBrowserDialog"/>互換クラス。
    /// </summary>
    public class FolderBrowserDialog: DisposeFinalizeBase
    {
        public FolderBrowserDialog()
            : base()
        {
            this.Dialog = new Forms.FolderBrowserDialog();
        }

        #region property

        Forms.FolderBrowserDialog Dialog { get; set; }

        /// <summary>
        ///  <see cref="Component"/> を格納している <see cref="IContainer"/> を取得します。
        /// </summary>
        [BrowsableAttribute(false)]
        public IContainer Container { get { return Dialog.Container; } }

        /// <summary>
        /// ダイアログ ボックスのツリー ビュー コントロールの上部に表示する説明テキストを取得または設定します。
        /// </summary>
        [BrowsableAttribute(true)]
        public string Description
        {
            get { return Dialog.Description; }
            set { this.Description = value; }
        }

        /// <summary>
        /// 参照の開始位置とするルート フォルダーを取得または設定します。
        /// </summary>
        [BrowsableAttribute(true)]
        public Environment.SpecialFolder RootFolder
        {
            get { return Dialog.RootFolder; }
            set { Dialog.RootFolder = value; }
        }

        /// <summary>
        /// ユーザーが選択したパスを取得または設定します。
        /// </summary>
        [BrowsableAttribute(true)]
        public string SelectedPath
        {
            get { return Dialog.SelectedPath; }
            set { Dialog.SelectedPath = value; }
        }

        /// <summary>
        /// フォルダー参照ダイアログ ボックスに [新しいフォルダー] ボタンを表示するかどうかを示す値を取得または設定します。
        /// </summary>
        [BrowsableAttribute(true)]
        public bool ShowNewFolderButton
        {
            get { return Dialog.ShowNewFolderButton; }
            set { Dialog.ShowNewFolderButton = value; }
        }

        /// <summary>
        /// <see cref="Component"/> の <see cref="ISite"/> を取得または設定します。
        /// </summary>
        [BrowsableAttribute(false)]
        public virtual ISite Site
        {
            get { return Dialog.Site; }
            set { Dialog.Site = value; }
        }

        /// <summary>
        /// コントロールに関するデータを格納するオブジェクトを取得または設定します。
        /// </summary>
        [BindableAttribute(true)]
        public Object Tag
        {
            get { return Dialog.Tag; }
            set { Dialog.Tag = value; }
        }

        #endregion

        #region function

        public bool? ShowDialog()
        {
            var compatibleresult = Dialog.ShowDialog();
            return compatibleresult == Forms.DialogResult.OK;
        }

        public bool? ShowDialog(Window owner)
        {
            var form = new CompatibleFormWindow(owner);
            var compatibleresult = Dialog.ShowDialog(form);
            return compatibleresult == Forms.DialogResult.OK;
        }

        #endregion

        #region DisposeFinalizeBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(Dialog != null) {
                    Dialog.Dispose();
                    Dialog = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
