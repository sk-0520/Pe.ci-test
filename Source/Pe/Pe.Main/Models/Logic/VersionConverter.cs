using System;
using System.Collections.Generic;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public sealed class VersionConverter
    {
        #region function

        public string ConvertDisplayVersion(Version version, string separator)
        {
            return $"{version.Major}{separator}{version.Minor:00}{separator}{version.Build:000}";
        }

        public string ConvertNormalVersion(Version version)
        {
            return ConvertDisplayVersion(version, ".");
        }

        public string ConvertFileName(string head, Version version, string tail, string extension)
        {
            if(string.IsNullOrWhiteSpace(head)) {
                throw new ArgumentException(nameof(string.IsNullOrWhiteSpace), nameof(head));
            }
            var values = new List<string>() {
                head,
                "_",
                ToFileString(version)
            };
            if(!string.IsNullOrWhiteSpace(tail)) {
                values.Add("_");
                values.Add(tail);
            }
            if(!string.IsNullOrEmpty(extension)) {
                values.Add(".");
                values.Add(extension);
            }

            return string.Join(string.Empty, values);
        }

        /// <summary>
        /// バージョンをファイルとして使用できる形にする。
        /// <para>ファイル名として扱う共通処理。</para>
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public string ToFileString(Version version) => ConvertDisplayVersion(version, "-");

        #endregion
    }
}
