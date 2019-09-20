using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Launcher
{
    public sealed class EmptyFileSystemInfo : FileSystemInfo
    {
        public EmptyFileSystemInfo(string path)
        {
            Path = path;
        }

        #region property

        string Path { get; }

        #endregion

        #region function
        #endregion

        #region FileSystemInfo

        public override string Name => System.IO.Path.GetFileName(Path);

        public override bool Exists => FileUtility.Exists(Path);

        public override void Delete()
        {
            throw new NotSupportedException();
        }

        #endregion

    }
}
