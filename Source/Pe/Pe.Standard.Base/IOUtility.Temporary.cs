using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ContentTypeTextNet.Pe.Standard.Base
{
    /// <summary>
    /// 一時ディレクトリ処理。
    /// <para>プログラム生成による一時ディレクトリを扱う。</para>
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

        public DirectoryInfo Directory { get;}
        private DirectoryCleaner? Cleaner { get; }

        #endregion

        #region function

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(Cleaner is null) {
                    Directory.Delete(true);
                } else {
                    Cleaner.Clear(false);
                    Directory.Delete(true);
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
