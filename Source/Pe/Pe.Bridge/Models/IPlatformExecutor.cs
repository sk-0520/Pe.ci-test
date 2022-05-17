using System.Collections.Generic;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Bridge.Models
{
    /// <summary>
    /// ファイル的な何かを実行する。
    /// </summary>
    public interface IAddonExecutor
    {
        #region function

        /// <inheritdoc cref="Execute(string, IEnumerable{string}, string, ShowMode)"/>
        void Execute(string path);
        /// <inheritdoc cref="Execute(string, IEnumerable{string}, string, ShowMode)"/>
        void Execute(string path, IEnumerable<string> options);
        /// <summary>
        /// 通常実行。
        /// </summary>
        /// <param name="path">対象パス。</param>
        /// <param name="options">オプション。</param>
        /// <param name="workDirectoryPath">作業ディレクトリ</param>
        /// <param name="showMode"><inheritdoc cref="ShowMode"/></param>
        void Execute(string path, IEnumerable<string> options, string workDirectoryPath, ShowMode showMode);

        /// <inheritdoc cref="ExtendsExecute(string, IEnumerable{string}, ShowMode)"/>
        void ExtendsExecute(string path);
        /// <inheritdoc cref="ExtendsExecute(string, IEnumerable{string}, string)"/>
        void ExtendsExecute(string path, IEnumerable<string> options);
        /// <summary>
        /// 指定して実行。
        /// </summary>
        /// <param name="path">対象パス。</param>
        /// <param name="options">オプション。</param>
        /// <param name="workDirectoryPath">作業ディレクトリ</param>
        /// <param name="showMode"><inheritdoc cref="ShowMode"/></param>
        void ExtendsExecute(string path, IEnumerable<string> options, string workDirectoryPath, ShowMode showMode);

        #endregion
    }
}
