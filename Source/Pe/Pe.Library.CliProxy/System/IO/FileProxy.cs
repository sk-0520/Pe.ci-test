using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.CliProxy.System.IO
{
    public interface IFileProxy
    {
        #region function

        /// <inheritdoc cref="File.Copy(string, string, bool)"/>
        void Copy(string sourceFileName, string destFileName, bool overwrite);

        /// <inheritdoc cref="File.Move(string, string)"/>
        void Move(string sourceFileName, string destFileName);

        /// <inheritdoc cref="File.Delete(string)"/>
        void Delete(string path);

        /// <inheritdoc cref="File.Exists(string)"/>
        bool Exists(string path);

        #endregion
    }

    public static class IFileProxyExtensions
    {
        #region function

        /// <inheritdoc cref="File.Copy(string, string)"/>
        public static void Copy(this IFileProxy staticFile, string sourceFileName, string destFileName)
            => staticFile.Copy(sourceFileName, destFileName, false)
        ;

        #endregion
    }

    public class DirectFileProxy: IFileProxy
    {
        #region IStaticFile

        public void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            File.Copy(sourceFileName, destFileName, overwrite);
        }

        public void Move(string sourceFileName, string destFileName)
        {
            File.Move(sourceFileName, destFileName);
        }

        public void Delete(string path)
        {
            File.Delete(path);
        }

        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        #endregion
    }
}
