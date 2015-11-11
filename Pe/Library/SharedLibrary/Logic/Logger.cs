﻿/**
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
namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using ContentTypeTextNet.Library.SharedLibrary.IF;
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
    using ContentTypeTextNet.Library.SharedLibrary.Model;

    /// <summary>
    /// 最低限度の機能を保持したログ出力処理。
    /// </summary>
    public class Logger: LoggerBase, IIsDisposed
    {
        #region varable

        string _filePath = null;
        TextWriter _fileWriter = null;

        #endregion

        public Logger()
            : base()
        {
            Writer = new HashSet<TextWriter>();
        }

        #region property

        protected HashSet<TextWriter> Writer { get; private set; }

        /// <summary>
        /// ファイルログに使用するファイルパス。
        /// <para>値設定が有効なものであれば既存ファイルを閉じて指定されたファイルに追記していく。</para>
        /// </summary>
        public string FilePath
        {
            get { return this._filePath; }
            set
            {
                if(this._filePath != value) {
                    ClearFileWriter();
                }

                this._filePath = value;
            }
        }

        /// <summary>
        /// FilePathで設定されたパスのファイルストリーム。
        /// </summary>
        protected TextWriter FileWriter
        {
            get
            {
                if(this._fileWriter == null && CanFilePuts) {
                    this._fileWriter = new StreamWriter(new FileStream(this._filePath, FileMode.Append, FileAccess.Write, FileShare.Read), Encoding.UTF8);
                }

                return this._fileWriter;
            }
        }
        public bool CanFilePuts
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this._filePath) && LoggerConfig.PutsStream;
            }
        }

        #endregion

        #region function

        void ClearFileWriter()
        {
            if(this._fileWriter != null) {
                this._fileWriter.Dispose();
                this._fileWriter = null;
            }
        }

        protected string PutsOutput(LogItemModel item, char c)
        {
            return string.Format(
                "{0:s}{1}[{2}] {3}({4}): {5}{6}",
                item.Timestamp,
                c,
                item.LogKind.ToString().ToUpper()[0],
                item.CallerMember,
                item.CallerLine,
                item.Message,
                item.HasDetail ? ", " + item.DetailText.SplitLines().First() : string.Empty
            );
        }

        #endregion

        #region LoggerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    ClearFileWriter();
                }
            }

            base.Dispose(disposing);
        }

        protected override void PutsStream(LogItemModel item)
        {
            if(FileWriter != null) {
                FileWriter.WriteLine(LogUtility.MakeLogDetailText(item));
                FileWriter.Flush();
            }
            lock(Writer) {
                foreach(var writer in Writer) {
                    writer.WriteLine(LogUtility.MakeLogDetailText(item));
                    writer.Flush();
                }
            }
        }

        protected override void PutsConsole(LogItemModel item)
        {
            Console.WriteLine(PutsOutput(item, 'C'));
        }

        protected override void PutsDebug(LogItemModel item)
        {
            System.Diagnostics.Debug.WriteLine(PutsOutput(item, 'D'));
        }

        /// <summary>
        /// このクラスでは何もしない。
        /// <para>サブクラスで適当にどうぞ。</para>
        /// </summary>
        /// <param name="item"></param>
        protected override void PutsCustom(LogItemModel item)
        { }

        #endregion
    }
}
