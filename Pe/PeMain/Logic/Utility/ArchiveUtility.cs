/*
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see<http://www.gnu.org/licenses/>.
*/
namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

    internal static class ArchiveUtility
    {
        static ZipArchiveEntry WriteArchive(ZipArchive archive, string path, string baseDirPath)
        {
            var entryPath = path.Substring(baseDirPath.Length);
            while(entryPath.First() == Path.DirectorySeparatorChar) {
                entryPath = entryPath.Substring(1);
            }

            var entry = archive.CreateEntry(entryPath);

            using(var entryStream = new BinaryWriter(entry.Open())) {
                var buffer = FileUtility.ToBinary(path);
                entryStream.Write(buffer);
            }

            return entry;
        }

        /// <summary>
        /// 指定パスにZIP形式でアーカイブを作成。
        /// </summary>
        /// <param name="saveFilePath">保存先パス。</param>
        /// <param name="basePath">基準とするディレクトリパス。</param>
        /// <param name="targetFiles">取り込み対象パス。ディレクトリを指定した場合は、以下のファイル・ディレクトリをすべてその対象とする</param>
        public static void CreateZipFile(string saveFilePath, string basePath, IEnumerable<string> targetFiles)
        {
            using(var zip = new ZipArchive(new FileStream(saveFilePath, FileMode.Create), ZipArchiveMode.Create)) {
                foreach(var filePath in targetFiles) {
                    if(File.Exists(filePath)) {
                        WriteArchive(zip, filePath, basePath);
                    } else if(Directory.Exists(filePath)) {
                        var list = Directory.EnumerateFiles(filePath, "*", SearchOption.AllDirectories);
                        foreach(var f in list) {
                            WriteArchive(zip, f, basePath);
                        }
                    }
                }
            }
        }

    }
}
