using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Main.Models.Launcher
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

#pragma warning disable CS8603 // Null 参照戻り値である可能性があります。
        public override string Name => System.IO.Path.GetFileName(Path);
#pragma warning restore CS8603 // Null 参照戻り値である可能性があります。

        public override bool Exists => FileUtility.Exists(Path);

        public override void Delete()
        {
            throw new NotSupportedException();
        }

        #endregion

    }
}
