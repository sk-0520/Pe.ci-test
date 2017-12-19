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
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.Model.Unmanaged.Com;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
    /// <summary>
    /// ショートカットファイル処理。
    /// </summary>
    public class ShortcutFile: DisposeFinalizeBase
    {
        #region define

        const int _argumentLength = 1024;
        const int _descriptionLength = 1024 * 5;

        #endregion

        #region static

        private static StringBuilder CreateStringBuffer()
        {
            return CreateStringBuffer((int)MAX.MAX_PATH);
        }

        private static StringBuilder CreateStringBuffer(int max)
        {
            return new StringBuilder(max, max);
        }

        private static ComModel<IShellLink> CreateShellLink()
        {
            return new ComModel<IShellLink>((IShellLink)new ShellLinkObject());
        }

        #endregion

        #region variable

        protected ComModel<IShellLink> _shellLink = null;
        protected ComModel<IPersistFile> _persistFile = null;

        #endregion

        /// <summary>
        /// ショートカットを作成するためにオブジェクト生成。
        /// </summary>
        public ShortcutFile()
            : base()
        {
            this._shellLink = CreateShellLink();
        }

        /// <summary>
        /// ショートカットを読み込むためにオブジェクト生成。
        /// </summary>
        /// <param name="path">読み込むショートカットファイルパス。</param>
        public ShortcutFile(string path)
            : base()
        {
            Load(path);
        }

        #region property

        protected ComModel<IPersistFile> PersistFile
        {
            get
            {
                if(this._persistFile == null) {
                    var result = (IPersistFile)this._shellLink.Com;

                    this._persistFile = new ComModel<IPersistFile>(result);
                }

                return this._persistFile;
            }
        }

        /// <summary>
        /// ショートカット先パス。
        /// </summary>
        public string TargetPath
        {
            get
            {
                var resultBuffer = CreateStringBuffer();
                var findData = new WIN32_FIND_DATA();

                this._shellLink.Com.GetPath(resultBuffer, resultBuffer.MaxCapacity, out findData, SLGP_FLAGS.SLGP_UNCPRIORITY);

                return resultBuffer.ToString();
            }
            set
            {
                this._shellLink.Com.SetPath(value);
            }
        }

        /// <summary>
        /// 引数。
        /// </summary>
        public string Arguments
        {
            get
            {
                var resultBuffer = CreateStringBuffer(_argumentLength);

                this._shellLink.Com.GetArguments(resultBuffer, resultBuffer.Capacity);

                return resultBuffer.ToString();
            }
            set
            {
                this._shellLink.Com.SetArguments(value);
            }
        }

        /// <summary>
        /// コメント。
        /// </summary>
        public string Description
        {
            get
            {
                var resultBuffer = CreateStringBuffer(_descriptionLength);

                this._shellLink.Com.GetDescription(resultBuffer, resultBuffer.Capacity);

                return resultBuffer.ToString();
            }
            set
            {
                this._shellLink.Com.SetDescription(value);
            }
        }

        /// <summary>
        /// 作業ディレクトリ。
        /// </summary>
        public string WorkingDirectory
        {
            get
            {
                var resultBuffer = CreateStringBuffer();

                this._shellLink.Com.GetWorkingDirectory(resultBuffer, resultBuffer.MaxCapacity);

                return resultBuffer.ToString();
            }
            set
            {
                this._shellLink.Com.SetWorkingDirectory(value);
            }
        }

        /// <summary>
        /// 表示方法。
        /// </summary>
        public SW ShowCommand
        {
            get
            {
                int rawShowCommand;

                this._shellLink.Com.GetShowCmd(out rawShowCommand);

                return (SW)rawShowCommand;
            }
            set
            {
                this._shellLink.Com.SetShowCmd((int)value);
            }
        }

        #endregion

        #region function

        /// <summary>
        /// アイコン取得。
        /// </summary>
        /// <returns></returns>
        public IconPathModel GetIcon()
        {
            var resultBuffer = CreateStringBuffer();
            int iconIndex;

            this._shellLink.Com.GetIconLocation(resultBuffer, resultBuffer.Capacity, out iconIndex);

            return new IconPathModel() {
                Path = resultBuffer.ToString(),
                Index = iconIndex
            };
        }

        /// <summary>
        /// アイコン設定。
        /// </summary>
        /// <param name="iconPath"></param>
        public void SetIcon(IconPathModel iconPath)
        {
            this._shellLink.Com.SetIconLocation(iconPath.Path, iconPath.Index);
        }

        /// <summary>
        /// ショートカット読み込み。
        /// 
        /// public だが Save との IF を合わせるためだけであり基本的には外から使用しない。
        /// 使っても問題ないけど。
        /// </summary>
        /// <param name="path">読み込むショートカットパス。</param>
        public void Load(string path)
        {
            if(this._shellLink != null) {
                Dispose(true);
            }

            this._shellLink = CreateShellLink();
            PersistFile.Com.Load(path, 0);
        }

        /// <summary>
        /// ショートカットを保存。
        /// </summary>
        /// <param name="path">保存先ショートカットパス。</param>
        public void Save(string path)
        {
            PersistFile.Com.Save(path, true);
        }

        #endregion

        #region DisposeFinalizeBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(this._persistFile != null) {
                    this._persistFile.Dispose();
                }
                this._persistFile = null;

                if(this._shellLink != null) {
                    this._shellLink.Dispose();
                }
                this._shellLink = null;
            }

            base.Dispose(disposing);
        }

        #endregion /////////////////////////////////
    }
}
