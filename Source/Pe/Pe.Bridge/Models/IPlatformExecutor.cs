using System.Collections.Generic;

namespace ContentTypeTextNet.Pe.Bridge.Models
{
    /// <summary>
    /// ファイル的な何かを実行する。
    /// </summary>
    public interface IAddonExecutor
    {
        #region function

        void Execute(string path);
        void Execute(string path, IEnumerable<string> options);
        void Execute(string path, IEnumerable<string> options, string workDirectoryPath);

        void ExtendsExecute(string path);
        void ExtendsExecute(string path, IEnumerable<string> options);
        void ExtendsExecute(string path, IEnumerable<string> options, string workDirectoryPath);

        #endregion
    }
}
