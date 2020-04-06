using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Bridge.Plugin
{
    /// <summary>
    /// 永続データのデータフォーマット。
    /// </summary>
    public enum PluginPersistentFormat
    {
        /// <summary>
        /// JSON形式。
        /// </summary>
        Json,
        /// <summary>
        /// XML形式。
        /// <seealso cref="System.Xml.Serialization.XmlSerializer"/>
        /// </summary>
        SimleXml,
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
    public interface IPluginFile
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
    public interface IPluginPersistent
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
        IPluginFile File { get; }
        /// <summary>
        /// 永続データ処理。
        /// </summary>
        IPluginPersistent Persistent { get; }

        #endregion
    }
}
