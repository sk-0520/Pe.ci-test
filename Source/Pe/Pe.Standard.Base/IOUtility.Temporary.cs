using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ContentTypeTextNet.Pe.Standard.Base
{

    [Serializable]
    public class TemporaryException: Exception
    {
        public TemporaryException()
        { }

        public TemporaryException(string message)
            : base(message)
        { }

        public TemporaryException(string message, Exception inner)
            : base(message, inner)
        { }

        protected TemporaryException(
          System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        { }
    }

    /// <summary>
    /// 一時ディレクトリ・ファイル生成オプション。
    /// </summary>
    public class TemporaryOptions
    {
        #region property

        /// <summary>
        /// プレフィックス。
        /// </summary>
        public string Prefix { get; set; } = string.Empty;

        /// <summary>
        /// ランダム生成文字列の文字数。
        /// </summary>
        public int RandomNameLength { get; set; } = 6;

        /// <summary>
        /// ランダム生成文字列の文字一覧。
        /// </summary>
        public ISet<char> RandomNameCharacters { get; set; } = new HashSet<char>("abcdefghijklmnopqrstuvwxyz0123456789");

        /// <summary>
        /// 再試行回数。
        /// </summary>
        public int RetryCount { get; set; } = 100;

        /// <summary>
        /// ディレクトリ破棄処理。
        /// </summary>
        public DirectoryCleaner? Cleaner { get; set; }

        #endregion
    }

    /// <summary>
    /// 一時ディレクトリ処理。
    /// <para>渡されたディレクトリを破棄時に削除する。</para>
    /// <para><see cref="IOUtility.CreateTemporaryDirectory(DirectoryInfo, TemporaryOptions?)"/>から使用する前提。</para>
    /// </summary>
    public class TemporaryDirectory: DisposerBase
    {
        public TemporaryDirectory(DirectoryInfo directory)
        {
            Directory = directory;
            Cleaner = null;
        }

        public TemporaryDirectory(DirectoryInfo directory, DirectoryCleaner cleaner)
        {
            Directory = directory;
            Cleaner = cleaner;
        }

        #region property

        /// <summary>
        /// 一時ディレクトリ。
        /// </summary>
        public DirectoryInfo Directory { get; }
        /// <summary>
        /// ディレクトリ破棄処理。
        /// </summary>
        private DirectoryCleaner? Cleaner { get; }

        #endregion

        #region function

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Directory.Refresh();
                if(Directory.Exists) {
                    if(Cleaner is null) {
                        Directory.Delete(true);
                    } else {
                        Cleaner.Clear(false);
                        Directory.Delete(true);
                    }
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
