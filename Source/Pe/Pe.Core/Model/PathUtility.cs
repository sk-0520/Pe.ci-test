using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ContentTypeTextNet.Pe.Core.Model
{
    public static class PathUtility
    {
        #region define

        const string formatTimestampFileName = "yyyy-MM-dd_HH-mm-ss";
        const string extensionTemporaryFile = "tmp";

        #endregion

        /// <summary>
        /// パスに拡張子を追加する。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="ext"></param>
        /// <returns></returns>
        public static string AppendExtension(string path, string ext)
        {
            return path + "." + ext;
        }

        /// <summary>
        /// ファイル名をそれとなく安全な名称に変更する。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fn"></param>
        /// <returns></returns>
        public static string ToSafeName(string name, Func<char, string> fn)
        {
            Debug.Assert(fn != null);

            var pattern = Regex.Escape(string.Concat(Path.GetInvalidPathChars().Concat(Path.GetInvalidFileNameChars())));
            var reg = new Regex("([" + pattern + "])");
            return reg.Replace(name.Trim(), m => fn(m.Groups[0].Value[0]));
        }
        /// <summary>
        /// ファイル名のシステムで使用できない文字を '_' に置き換える。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ToSafeNameDefault(string name)
        {
            return ToSafeName(name, c => "_");
        }

        [Obsolete("use: " + nameof(HasExtensions))]
        public static bool HasExtension(string path, params string[] extList)
        {
            return HasExtensions(path, extList);
        }

        public static bool HasExtensions(string path, params string[] extList)
        {
            var dotExt = Path.GetExtension(path);
            if(string.IsNullOrEmpty(dotExt)) {
                return false;
            }

            var ext = dotExt.Substring(1);
            return extList
                .Select(s => s.ToLower())
                .Any(s => s == ext)
            ;
        }

        /// <summary>
        /// アイコンを持つパスか。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool HasIconPath(string path)
        {
            return HasExtensions(path, "exe", "dll");
        }
        [Obsolete("use:" + nameof(HasIconPath))]
        public static bool IsIconPath(string path)
        {
            return HasIconPath(path);
        }

        /// <summary>
        /// ショートカットか。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsShortcut(string path)
        {
            return HasExtensions(path, "lnk");
        }

        /// <summary>
        /// 実行形式か。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsProgram(string path)
        {
            return HasExtensions(path, "exe", "dll");
        }

        public static string GetTimestampFileName(DateTime dateTime)
        {
            return dateTime.ToString(formatTimestampFileName);
        }

        /// <summary>
        /// ファイル名に使用可能なタイムスタンプを取得。
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentTimestampFileName()
        {
            return GetTimestampFileName(DateTime.Now);
        }

        /// <summary>
        /// 一時ファイル用拡張子の作成。
        /// <para>現在時間を用いる。</para>
        /// </summary>
        /// <returns></returns>
        public static string GetTemporaryExtension(string role)
        {
            return "." + GetCurrentTimestampFileName() + "." + role + "." + extensionTemporaryFile;
        }

        /// <summary>
        /// 一時ファイル用拡張子の作成。
        /// </summary>
        /// <param name="name">ファイル名。</param>
        /// <param name="role">役割。</param>
        /// <param name="extension">拡張子。</param>
        /// <returns></returns>
        static string CreateFileNameCore(string name, string? role, string extension)
        {
            return $"{name}{(role == null ? string.Empty : "-" + role)}.{extension}";
        }
        /// <summary>
        /// ファイル名を生成。
        /// </summary>
        /// <param name="name">ファイル名。</param>
        /// <param name="role">役割。</param>
        /// <param name="extension">拡張子。</param>
        /// <returns></returns>
        public static string CreateFileName(string name, string role, string extension)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(role));
            return CreateFileNameCore(name, role, extension);
        }
        /// <summary>
        /// ファイル名を生成。
        /// </summary>
        /// <param name="name">ファイル名。</param>
        /// <param name="extension">拡張子。</param>
        /// <returns></returns>
        public static string CreateFileName(string name, string extension)
        {
            return CreateFileNameCore(name, null, extension);
        }

        public static string ExpandFilePath(string path)
        {
            return Environment.ExpandEnvironmentVariables(path ?? string.Empty);
        }
    }
}
