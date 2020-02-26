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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
    /// <summary>
    /// 最低限度の機能を保持したログ出力処理。
    /// </summary>
    internal class Logger: LoggerBase, IIsDisposed
    {
        public Logger()
            : base()
        {
            Writer = new HashSet<TextWriter>();
            ManagingWriter = new HashSet<TextWriter>();
        }

        #region property

        protected HashSet<TextWriter> Writer { get; private set; }
        protected HashSet<TextWriter> ManagingWriter { get; private set; }

        #endregion

        #region function

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
                    foreach(var writer in ManagingWriter) {
                        writer.Dispose();
                    }
                }
                Writer.Clear();
                ManagingWriter.Clear();
            }

            base.Dispose(disposing);
        }

        protected override void PutsStream(LogItemModel item)
        {
            var s = LogUtility.MakeLogDetailText(item);
            lock (Writer) {
                foreach(var writer in Writer) {
                    writer.WriteLine(s);
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="cedeManage">本クラスに所有権を譲るか</param>
        public void AttachStream(TextWriter writer, bool cedeManage)
        {
            lock(Writer) {
                Writer.Add(writer);
                if(cedeManage) {
                    ManagingWriter.Add(writer);
                }
            }
        }

        public void DetachStream(TextWriter writer)
        {
            lock (Writer) {
                Writer.Remove(writer);
                ManagingWriter.Remove(writer);
            }
        }

        #endregion
    }
}
