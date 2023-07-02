using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace ContentTypeTextNet.Pe.Standard.Base
{
    /// <summary>
    /// ファイル関連の共通処理。
    /// </summary>
    public static class IOUtility
    {
        /// <summary>
        /// ファイルパスを元にディレクトリを作成
        /// </summary>
        /// <param name="path">ファイルパス</param>
        /// <returns>ディレクトリパス。<paramref name="path"/>から親ディレクトリが判定できなかった場合はから文字列。</returns>
        public static string MakeFileParentDirectory(string path)
        {
            var dirPath = Path.GetDirectoryName(path);
            if(dirPath == null) {
                return string.Empty;
            }

            var dirInfo = Directory.CreateDirectory(dirPath);
            return dirInfo.FullName;
        }

        /// <inheritdoc cref="MakeFileParentDirectory(string)"/>
        public static string MakeFileParentDirectory(FileSystemInfo fileSystemInfo) => MakeFileParentDirectory(fileSystemInfo.FullName);


        /// <summary>
        /// ファイル・ディレクトリ問わずに存在するか
        /// </summary>
        /// <param name="path">対象パス。</param>
        /// <returns>存在状態。</returns>
        public static bool Exists(string? path)
        {
            if(path == null) {
                return false;
            }

            return File.Exists(path) || Directory.Exists(path);
        }

        /// <summary>
        /// <inheritdoc cref="File.Exists(string)"/>
        /// <para>非同期版。</para>
        /// </summary>
        /// <param name="path"><inheritdoc cref="File.Exists(string)"/></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<bool> ExistsAsync(string? path, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => Exists(path), cancellationToken);
        }

        /// <summary>
        /// <inheritdoc cref="Directory.Exists(string)"/>
        /// <para>非同期版。</para>
        /// </summary>
        /// <param name="path"><inheritdoc cref="Directory.Exists(string)"/></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<bool> ExistsFileAsync(string path, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => {
                return File.Exists(path);
            }, cancellationToken);
        }

        public static Task<bool> ExistsDirectoryAsync(string path, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => {
                return Directory.Exists(path);
            }, cancellationToken);
        }

        /// <summary>
        /// ファイル・ディレクトリ問わずに対象パスを削除。
        /// <para>ファイル・ディレクトリを問わない(ディレクトリの場合は再帰的削除)。</para>
        /// </summary>
        /// <param name="path">対象パス。</param>
        /// <exception cref="IOException">ファイルでもディレクトリでもなかった。</exception>
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

        public static Task DeleteAsync(string path, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => Delete(path), cancellationToken);
        }

        /// <summary>
        /// パスから名前取得。
        /// </summary>
        /// <param name="path">対象パス。</param>
        /// <returns></returns>
        public static string GetName(string path)
        {
            var plainName = Path.GetFileNameWithoutExtension(path);
            if(string.IsNullOrEmpty(plainName)) {
                // ドライブ名に一致するか
                var drive = DriveInfo.GetDrives().FirstOrDefault(d => d.Name == path);
                if(drive != null) {
                    if(drive.DriveType == DriveType.CDRom || drive.DriveType == DriveType.Removable) {
                        return drive.Name;
                    } else {
                        return drive.VolumeLabel;
                    }
                }

                // ネットワークフォルダ名か(.NET Framework と挙動が違う気がする)
                if(PathUtility.IsNetworkDirectoryPath(path)) {
                    var name = PathUtility.GetNetworkDirectoryName(path);
                    if(!string.IsNullOrEmpty(name)) {
                        return name;
                    }
                }
            }

            if(PathUtility.IsProgram(path)) {
                var verInfo = FileVersionInfo.GetVersionInfo(path);
                if(!string.IsNullOrEmpty(verInfo.ProductName)) {
                    return verInfo.ProductName;
                }
            }

            return plainName ?? Path.GetFileName(path) ?? string.Empty;
        }
    }
}
