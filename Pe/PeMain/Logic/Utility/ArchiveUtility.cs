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
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
    using Data.Temporary;
    internal static class ArchiveUtility
    {
        struct FilePathAndBuffer
        {
            public FilePathAndBuffer(string path, byte[] buffer)
            {
                Path = path;
                Buffer = buffer;
            }

            public string Path { get; private set; }
            public byte[] Buffer { get; private set; }
        }

        static FilePathAndBuffer GetFilePathAndBuffer(string path)
        {
            var buffer = FileUtility.ToBinary(path);
            return new FilePathAndBuffer(path, buffer);
        }

        static ZipArchiveEntry WriteArchive(ZipArchive archive, string entryPath, byte[] buffer, CompressionLevel compressionLevel)
        {
            var entry = archive.CreateEntry(entryPath, compressionLevel);
            using(var entryStream = new BinaryWriter(entry.Open())) {
                entryStream.Write(buffer);
            }
            return entry;
        }

        static ZipArchiveEntry WriteArchive(ZipArchive archive, string relativePath, string baseDirectoryPath, CompressionLevel compressionLevel)
        {
            var path = Path.Combine(baseDirectoryPath, relativePath);
            var buffer = FileUtility.ToBinary(path);
            return WriteArchive(archive, relativePath, buffer, compressionLevel);
        }

        public static string GetArchiveEntryPath(string path, string baseDirectoryPath)
        {
            var entryPath = path.Substring(baseDirectoryPath.Length);
            while(entryPath.First() == Path.DirectorySeparatorChar) {
                entryPath = entryPath.Substring(1);
            }

            return entryPath;
        }

        /// <summary>
        /// 指定パスにZIP形式でアーカイブを作成。
        /// </summary>
        /// <param name="saveFilePath">保存先パス。</param>
        /// <param name="baseDirectoryPath">基準とするディレクトリパス。</param>
        /// <param name="archiveParameters">取り込み対象パス。ディレクトリを指定した場合は、以下のファイル・ディレクトリをすべてその対象とする</param>
        public static void CreateZipFile(string saveFilePath, string baseDirectoryPath, IEnumerable<ArchiveParameter> archiveParameters)
        {
            using(var zip = new ZipArchive(new FileStream(saveFilePath, FileMode.Create), ZipArchiveMode.Create)) {
                foreach(var archiveParameter in archiveParameters) {
                    var fullPath = archiveParameter.GetFullPath(baseDirectoryPath);
                    if(File.Exists(fullPath)) {
                        WriteArchive(zip, archiveParameter.RelativePath, baseDirectoryPath, archiveParameter.CompressionLevel);
                    } else if(Directory.Exists(fullPath)) {
                        var list = Directory.EnumerateFiles(fullPath, archiveParameter.SearchPattern, archiveParameter.SearchOption)
                            .AsParallel()
                            .Select(f => new FilePathAndBuffer(GetArchiveEntryPath(f, baseDirectoryPath), FileUtility.ToBinary(f)))
                        ;
                        foreach(var pb in list) {
                            WriteArchive(zip, pb.Path, pb.Buffer, archiveParameter.CompressionLevel);
                        }
                    }
                }
            }
        }

    }
}
