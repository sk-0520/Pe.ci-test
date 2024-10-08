using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.CliProxy.System.IO
{
    public interface IDirectoryProxy
    {
        #region function

        /// <inheritdoc cref="Directory.CreateDirectory(string)"/>
        DirectoryInfo CreateDirectory(string path);

        /// <inheritdoc cref="Directory.Exists(string)"/>
        bool Exists(string path);

        #endregion
    }

    public class DirectDirectoryProxy: IDirectoryProxy
    {
        #region IDirectoryProxy

        public DirectoryInfo CreateDirectory(string path)
        {
            return Directory.CreateDirectory(path);
        }

        public bool Exists(string path)
        {
            return Directory.Exists(path);
        }

        #endregion
    }
}
