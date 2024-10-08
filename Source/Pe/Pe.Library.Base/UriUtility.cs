using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.Base
{
    public static class UriUtility
    {
        #region function

        private static Uri CombinePathCore(Uri uri, bool appendLastSeparator, string[] paths)
        {
            Debug.Assert(paths.Any());

            var noLastSegment = string.IsNullOrEmpty(uri.UserInfo)
                ? $"{uri.Scheme}://{uri.Authority}"
                : $"{uri.Scheme}://{uri.UserInfo}@{uri.Authority}"
            ;
            var builder = new StringBuilder(uri.OriginalString.Length);
            foreach(var segment in uri.Segments) {
                builder.Append(segment);
            }
            foreach(var path in paths) {
                if(builder[builder.Length - 1] != '/') {
                    builder.Append('/');
                }
                builder.Append(path.Trim('/'));
            }
            if(appendLastSeparator) {
                if(builder[builder.Length - 1] != '/') {
                    builder.Append('/');
                }
            }

            if(!string.IsNullOrEmpty(uri.Query)) {
                builder.Append(uri.Query);
            }

            return new Uri(noLastSegment + builder.ToString());
        }

        /// <summary>
        /// <inheritdoc cref="CombinePath(Uri, bool, string, string[])"/>
        /// </summary>
        /// <remarks>
        /// <para>パス末尾に / は付与されない。</para>
        /// </remarks>
        /// <inheritdoc cref="CombinePath(Uri, bool, string, string[])"/>
        public static Uri CombinePath(Uri uri, string path, params string[] paths) => CombinePath(uri, false, path, paths);

        /// <summary>
        /// 指定URLのパスを順に結合。
        /// </summary>
        /// <param name="uri">対象URI。</param>
        /// <param name="appendLastSeparator">パス末尾に / を付与するか。</param>
        /// <param name="path">結合するパス。</param>
        /// <param name="paths">さらに結合するパス。</param>
        /// <returns>結合されたURI。クエリは最後に付与される。</returns>
        public static Uri CombinePath(Uri uri, bool appendLastSeparator, string path, params string[] paths)
        {
            if(uri == null) {
                throw new ArgumentNullException(nameof(uri));
            }
            if(path == null) {
                throw new ArgumentNullException(nameof(path));
            }
            if(paths == null) {
                throw new ArgumentNullException(nameof(paths));
            }
            foreach(var p in paths) {
                if(p == null) {
                    throw new ArgumentNullException(nameof(paths));
                }
            }

            var allPaths = new string[1 + paths.Length];
            allPaths[0] = path;
            for(var i = 0; i < paths.Length; i++) {
                allPaths[i + 1] = paths[i];
            }
            return CombinePathCore(uri, appendLastSeparator, allPaths);
        }

        #endregion
    }
}
