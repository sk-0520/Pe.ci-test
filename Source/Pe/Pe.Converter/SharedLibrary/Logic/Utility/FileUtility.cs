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
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
    /// <summary>
    /// ファイル関連の共通処理。
    /// </summary>
    internal static class FileUtility
    {
        /// <summary>
        /// バイナリファイル読み込み用ストリーム生成。
        /// <para>排他処理はしない。</para>
        /// </summary>
        /// <param name="filePath">読み込むファイルパス。</param>
        /// <returns>ストリーム。</returns>
        public static BinaryReader CreateBinaryReader(string filePath)
        {
            var stream = new BinaryReader(new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            return stream;
        }
        /// <summary>
        /// ファイルをバイナリとして読み込む。
        /// <para><see cref="File.ReadAllBytes"/> は開いているファイルを読めないのでこちらを使用する。</para>
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <param name="startIndex">読み出し位置</param>
        /// <param name="readLength">読み出しサイズ</param>
        /// <returns></returns>
        public static byte[] ToBinary(string filePath, int startIndex, int readLength)
        {
            byte[] buffer;

            using(var stream = CreateBinaryReader(filePath)) {
                buffer = new byte[readLength];
                stream.Read(buffer, startIndex, readLength);
            }

            return buffer;
        }
        /// <summary>
        /// ファイルをバイナリとして読み込む。
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <returns></returns>
        public static byte[] ToBinary(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            return ToBinary(filePath, 0, (int)fileInfo.Length);
        }

        /// <summary>
        /// ファイルパスを元にディレクトリを作成
        /// </summary>
        /// <param name="path">ファイルパス</param>
        /// <returns>ディレクトリパス</returns>
        public static string MakeFileParentDirectory(string path)
        {
            var dirPath = Path.GetDirectoryName(path);
            var dirInfo = Directory.CreateDirectory(dirPath);
            return dirInfo.FullName;
        }

        /// <summary>
        /// ファイル・ディレクトリ問わずに存在するか
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool Exists(string path)
        {
            return File.Exists(path) || Directory.Exists(path);
        }

        public static void Delete(string path)
        {
            if(File.Exists(path)) {
                File.Delete(path);
            } else if(Directory.Exists(path)) {
                Directory.Delete(path, true);
            } else {
                throw new IOException($"not found: {path}");
            }
        }

        /// <summary>
        /// 指定条件で不要ファイルを削除。
        /// </summary>
        /// <param name="baseDirectoryPath">対象ファイル群のディレクトリ。</param>
        /// <param name="targetWildcard">対象ファイル群をワイルドカードで指定。</param>
        /// <param name="orderBy">リストアップしたファイル群のソート順。真で昇順。</param>
        /// <param name="enableCount">リストアップしたファイル群の上位から残すファイル数。</param>
        /// <param name="catchException">ファイル削除中に例外を受け取った場合の処理。trueを返すと継続、falseで処理終了。</param>
        /// <returns>削除ファイル数。baseDirPathが存在しない場合は -1。</returns>
        //public static int RotateFiles(string baseDirectoryPath, string targetWildcard, OrderBy orderBy, int enableCount, Func<Exception, bool> catchException)
        //{
        //    if(Directory.Exists(baseDirectoryPath)) {
        //        var archiveList = Directory.EnumerateFiles(baseDirectoryPath, targetWildcard)
        //            .Where(File.Exists)
        //            .IfOrderByAsc(p => Path.GetFileName(p), orderBy == OrderBy.Ascending)
        //            .Skip(enableCount - 1)
        //        ;

        //        var removeCount = 0;
        //        foreach(var path in archiveList) {
        //            try {
        //                File.Delete(path);
        //                removeCount += 1;
        //            } catch(Exception ex) {
        //                if(!catchException(ex)) {
        //                    break;
        //                }
        //            }
        //        }
        //        return removeCount;
        //    } else {
        //        return -1;
        //    }
        //}

        /// <summary>
        /// パスから名前取得。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetName(string path)
        {
            var plainName = Path.GetFileNameWithoutExtension(path);
            if(string.IsNullOrEmpty(plainName)) {
                // ドライブ名
                var drive = DriveInfo.GetDrives().FirstOrDefault(d => d.Name == path);
                if(drive != null) {
                    return drive.VolumeLabel;
                }
            }

            if(PathUtility.IsProgram(path)) {
                var verInfo = FileVersionInfo.GetVersionInfo(path);
                if(!string.IsNullOrEmpty(verInfo.ProductName)) {
                    return verInfo.ProductName;
                }
            }

            return plainName;
        }
    }
}
