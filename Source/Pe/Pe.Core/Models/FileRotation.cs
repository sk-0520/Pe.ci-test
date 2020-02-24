using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public class FileRotation
    {
        #region function

        /// <summary>
        ///
        /// </summary>
        /// <param name="parentDirectory"></param>
        /// <param name="regex"></param>
        /// <param name="leaveCount"></param>
        /// <param name="orderByDesc"></param>
        /// <param name="exceptionCacther">ファイル削除中に例外を受け取った場合の処理。trueを返すと継続、falseで処理終了。</param>
        /// <returns></returns>
        private int ExecuteCore(DirectoryInfo parentDirectory, Regex regex, int leaveCount, bool orderByDesc, Func<Exception, bool> exceptionCacther)
        {
            try {
                parentDirectory.Refresh();
                if(!parentDirectory.Exists) {
                    return -1;
                }
            } catch(Exception) {
                throw;
            }

            var matchFiles = parentDirectory
                .EnumerateFiles("*")
                .Where(i => regex.IsMatch(i.Name))
            ;
            var targetFiles =
                (
                    orderByDesc
                        ? matchFiles.OrderByDescending(i => i.Name)
                        : matchFiles.OrderBy(i => i.Name)
                )
                .Skip(leaveCount)
                .ToList()
            ;

            var removedCount = 0;
            for(var i = 0; i < targetFiles.Count; i++) {
                try {
                    File.Delete(targetFiles[i].FullName);
                    removedCount += 1;
                } catch(Exception ex) {
                    if(!exceptionCacther(ex)) {
                        return removedCount;
                    }
                }
            }

            return removedCount;
        }

        public int ExecuteRegex(DirectoryInfo parentDirectory, Regex regex, int leaveCount, Func<Exception, bool> exceptionCacther)
        {
            return ExecuteCore(parentDirectory, regex, leaveCount, true, exceptionCacther);
        }
        public int ExecuteRegex(DirectoryInfo parentDirectory, Regex regex, int leaveCount, bool orderByDesc, Func<Exception, bool> exceptionCacther)
        {
            return ExecuteCore(parentDirectory, regex, leaveCount, true, exceptionCacther);
        }

        public int ExecuteWildcard(DirectoryInfo parentDirectory, string wildCard, int leaveCount, Func<Exception, bool> exceptionCacther)
        {
            return ExecuteWildcard(parentDirectory, wildCard, leaveCount, true, exceptionCacther);
        }
        public int ExecuteWildcard(DirectoryInfo parentDirectory, string wildCard, int leaveCount, bool orderByDesc, Func<Exception, bool> exceptionCacther)
        {
            var wildcardPattern = "^" + Regex.Escape(wildCard).Replace("\\?", ".").Replace("\\*", ".*") + "$";
            var wildcardRegex = new Regex(wildcardPattern, RegexOptions.IgnoreCase);

            return ExecuteCore(parentDirectory, wildcardRegex, leaveCount, orderByDesc, exceptionCacther);
        }

        public int ExecuteExtensions(DirectoryInfo parentDirectory, IEnumerable<string> extensions, int leaveCount, Func<Exception, bool> exceptionCacther)
        {
            return ExecuteExtensions(parentDirectory, extensions, leaveCount, true, exceptionCacther);
        }
        public int ExecuteExtensions(DirectoryInfo parentDirectory, IEnumerable<string> extensions, int leaveCount, bool orderByDesc, Func<Exception, bool> exceptionCacther)
        {
            var extensionPatterns  = extensions
                .Select(i => Regex.Escape(i))
                .Select(i => "("+ i + ")")
            ;
            var extensionPattern = "(" + string.Join("|", extensionPatterns) + ")";
            var extensionRegex = new Regex(extensionPattern, RegexOptions.IgnoreCase);

            return ExecuteCore(parentDirectory, extensionRegex, leaveCount, orderByDesc, exceptionCacther);
        }

        #endregion
    }
}
