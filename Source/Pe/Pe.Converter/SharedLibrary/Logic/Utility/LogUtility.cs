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
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using System.Reflection;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
    public static class LogUtility
    {
        #region define

        const string indent = "    ";
        const string messageFormat = " [MSG] {0}";
        const string detailPadding = "\t";

        #endregion

        static string ToShowText(MethodBase method)
        {
            var parameters = string.Join(
                ", ",
                method.GetParameters()
                    .OrderBy(p => p.Position)
                    .Select(p => p.ToString())
            );

            return (method?.ReflectedType?.Name ?? "(null)") + "." + (method?.Name ?? "null") + "(" + parameters + ")";
        }

        public static string MakeLogDetailText(LogItemModel item)
        {
            var header = string.Format(
                "{0}[{1}] {2} <{3}({4})> , Thread: {5}/{6}, Assembly: {7}",
                item.Timestamp.ToString("yyyy-MM-ddTHH:mm:ss.fff"),
                item.LogKind.ToString().ToUpper().Substring(0, 1),
                item.CallerMember,
                item.CallerFile,
                item.CallerLine,
                item.CallerThread.ManagedThreadId,
                item.CallerThread.ThreadState,
                item.CallerAssembly.GetName()
            );
            var message = string.Format(messageFormat, item.Message);
            var detail = item.DetailText;
            if(!string.IsNullOrEmpty(detail)) {
                var detailIndent = new string(' ', message.Length);
                var lines = detail.SplitLines();
                var nexts = lines.Skip(1).Select(s => detailIndent + detailPadding + s);
                if(nexts.Any()) {
                    var first = detailPadding + lines.First();
                    detail = first + Environment.NewLine + string.Join(Environment.NewLine, nexts);
                } else {
                    detail = detailPadding + detail;
                }
            }
            var stack = string.Join(
                Environment.NewLine,
                item.StackTrace.GetFrames()
                    .Select(sf => string.Format(
                        indent + "-[{0:x8}][{1:x8}] {2}[{3}]",
                        sf.GetNativeOffset(),
                        sf.GetILOffset(),
                        ToShowText(sf.GetMethod()),
                        sf.GetFileLineNumber()
                    )
                )
            );

            var result
                = header
                + Environment.NewLine
                + message
                + detail
                + Environment.NewLine
                + " [STK]"
                + Environment.NewLine
                + indent + "+[ Native ][   IL   ] Method[line]"
                + Environment.NewLine
                + stack
                + Environment.NewLine
            ;
            return result;
        }

        public static string ConvertProgramFileName(string filePath, string startPath)
        {
            var startIndex = filePath.IndexOf(startPath);
            if(startIndex != -1) {
                return filePath.Substring(startIndex);
            }

            return filePath;
        }
    }
}
