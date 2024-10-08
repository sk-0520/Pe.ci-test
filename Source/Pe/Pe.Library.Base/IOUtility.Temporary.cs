using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Base
{
    [Serializable]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
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

        [Obsolete]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Info Code Smell", "S1133:Deprecated code should be removed")]
        protected TemporaryException(
          System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        { }
    }

    /// <summary>
    /// 一時ディレクトリ・ファイル生成オプション。
    /// </summary>
    public abstract class TemporaryOptions
    {
        #region property

        /// <summary>
        /// 一時ファイルシステム名プレフィックス。
        /// </summary>
        public string Prefix { get; set; } = string.Empty;
        /// <summary>
        /// 一時ファイルシステム名サフィックス。
        /// </summary>
        public string Suffix { get; set; } = string.Empty;

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

        #endregion
    }

    public class TemporaryDirectoryOptions: TemporaryOptions
    {
        #region property

        /// <summary>
        /// ディレクトリ破棄処理。
        /// </summary>
        public DirectoryCleaner? Cleaner { get; set; }

        #endregion
    }

    public class TemporaryFileOptions: TemporaryOptions
    {
        #region property

        #endregion
    }

    /// <summary>
    /// 一時ディレクトリ処理。
    /// </summary>
    /// <remarks>
    /// <para>渡されたディレクトリを破棄時に削除する。</para>
    /// <para><see cref="IOUtility.CreateTemporaryDirectory"/>から使用する前提。</para>
    /// </remarks>
    public class TemporaryDirectory: DisposerBase
    {
        internal TemporaryDirectory(DirectoryInfo directory)
        {
            Directory = directory;
            Cleaner = null;
        }

        internal TemporaryDirectory(DirectoryInfo directory, DirectoryCleaner cleaner)
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

    public class TemporaryFile: DisposerBase
    {
        internal TemporaryFile(FileStream stream)
        {
            Stream = stream;
            Path = stream.Name;
            File = new FileInfo(Path);
            File.Refresh();
        }

        #region proeprty

        /// <summary>
        /// ファイルストリーム。
        /// </summary>
        private FileStream Stream { get; }

        /// <summary>
        /// ファイル情報。
        /// </summary>
        /// <remarks>
        /// <para>ストリーム処理の解放とか諸々の管理をしたくない場合は<see cref="CreateStream"/>を用いること。</para>
        /// </remarks>
        public FileInfo File { get; }

        /// <summary>
        /// 内部で開いたストリーム一式。
        /// </summary>
        private Stack<Stream> Streams { get; } = new Stack<Stream>();

        /// <summary>
        /// 対象ファイルパス。
        /// </summary>
        public string Path { get; private set; }

        #endregion

        #region function

        /// <summary>
        /// ファイル操作用 <see cref="System.IO.Stream"/> を生成。
        /// </summary>
        /// <returns></returns>
        public Stream CreateStream()
        {
            var stream = new FileStream(Path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            Streams.Push(stream);
            return new KeepStream(stream);
        }

        /// <summary>
        /// 現在ストリームに対する処理。
        /// </summary>
        /// <param name="action"></param>
        public void DoStream(Action<Stream> action)
        {
            action(Stream);
        }

        /// <summary>
        /// 非同期版。
        /// <inheritdoc cref="DoStream"/>
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public Task DoStreamAsync(Func<Stream, Task> func)
        {
            return func(Stream);
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    foreach(var stream in Streams) {
                        stream.Dispose();
                    }
                    Streams.Clear();
                    Stream.Dispose();
                }

                if(Path.Length != 0 && System.IO.File.Exists(Path)) {
                    System.IO.File.Delete(Path);
                    Path = string.Empty;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
