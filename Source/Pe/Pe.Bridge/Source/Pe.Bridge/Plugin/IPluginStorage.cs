using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ContentTypeTextNet.Pe.Bridge.Plugin
{
    /// <summary>
    /// 永続データのデータフォーマット。
    /// </summary>
    public enum PluginPersistentFormat
    {
        /// <summary>
        /// ただの文字列。
        /// <para>プラグイン側で解釈すること。</para>
        /// </summary>
        Text,
        /// <summary>
        /// JSON形式。
        /// </summary>
        Json,
        /// <summary>
        /// XML形式。
        /// <seealso cref="System.Xml.Serialization.XmlSerializer"/>
        /// </summary>
        SimpleXml,
        /// <summary>
        /// XML形式。
        /// <seealso cref="System.Runtime.Serialization.DataContractSerializer"/>
        /// </summary>
        DataXml,
    }

    /// <summary>
    /// ファイル操作処理。
    /// <para>Pe の管理下で処理する。</para>
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface IPluginFileStorage
    {
        #region function

        /// <summary>
        /// 指定ファイル名が存在するか。
        /// </summary>
        /// <param name="name">ファイル名として有効な名前。</param>
        /// <returns>存在すれば真。</returns>
        bool Exists(string name);

        /// <summary>
        /// 指定ファイル名を変更する。
        /// </summary>
        /// <param name="sourceName">元ファイル名。</param>
        /// <param name="destinationName">変更ファイル名。</param>
        /// <param name="overwrite">上書きを行うか。</param>
        void Rename(string sourceName, string destinationName, bool overwrite);

        /// <summary>
        /// 指定ファイル名を複製する。
        /// </summary>
        /// <param name="sourceName">元ファイル名。</param>
        /// <param name="destinationName">コピー先ファイル名。</param>
        /// <param name="overwrite">上書きを行うか。</param>
        void Copy(string sourceName, string destinationName, bool overwrite);

        /// <summary>
        /// 指定ファイル名を削除する。
        /// </summary>
        /// <param name="name">ファイル名として有効な名前。</param>
        void Delete(string name);

        /// <summary>
        /// 指定ファイル名を開く。
        /// </summary>
        /// <param name="name">ファイル名として有効な名前。</param>
        /// <param name="fileMode"><see cref="System.IO.FileMode"/></param>
        /// <returns>ストリーム。</returns>
        Stream Open(string name, FileMode fileMode);

        #endregion
    }

    /// <summary>
    /// 永続データ操作処理。
    /// <para>Pe の管理下で処理する。</para>
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface IPluginPersistentStorage
    {
        #region function

        #endregion
    }

    /// <summary>
    /// ファイル操作処理グループ。
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface IPluginFiles
    {
        #region property

        /// <summary>
        /// バックアップ対象。
        /// </summary>
        IPluginFileStorage User { get; }
        /// <summary>
        /// 非バックアップ対象。
        /// <para>次回起動時に存在する可能性あり。</para>
        /// </summary>
        IPluginFileStorage Machine { get; }
        /// <summary>
        /// 非バックアップ対象。
        /// <para>次回起動時に存在しない。</para>
        /// </summary>
        IPluginFileStorage Temporary { get; }

        #endregion
    }

    /// <summary>
    /// 永続データ操作処理グループ。
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface IPluginPersistents
    {
        #region property

        /// <summary>
        /// 通常データ。
        /// </summary>
        IPluginPersistentStorage Normal { get; }
        /// <summary>
        /// 大きめのデータ。
        /// </summary>
        IPluginPersistentStorage Large { get; }
        /// <summary>
        /// お好きに。
        /// </summary>
        IPluginPersistentStorage Temporary { get; }

        #endregion
    }

    /// <summary>
    /// プラグインからのストレージ操作処理。
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface IPluginStorage
    {
        #region property

        /// <summary>
        /// ファイル処理。
        /// </summary>
        IPluginFiles File { get; }
        /// <summary>
        /// 永続データ処理。
        /// </summary>
        IPluginPersistents Persistent { get; }

        #endregion
    }
}
