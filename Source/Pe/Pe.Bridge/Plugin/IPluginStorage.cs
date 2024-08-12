using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace ContentTypeTextNet.Pe.Bridge.Plugin
{
    /// <summary>
    /// 永続データのデータフォーマット。
    /// </summary>
    public enum PluginPersistenceFormat
    {
        /// <summary>
        /// ただの文字列。
        /// </summary>
        /// <remarks>
        /// <para>プラグイン側で解釈すること。</para>
        /// </remarks>
        Text,
        /// <summary>
        /// JSON形式。
        /// </summary>
        Json,
        /// <summary>
        /// XML形式。
        /// </summary>
        /// <seealso cref="System.Xml.Serialization.XmlSerializer"/>
        SimpleXml,
        /// <summary>
        /// XML形式。
        /// </summary>
        /// <seealso cref="System.Runtime.Serialization.DataContractSerializer"/>
        DataXml,
    }

    /// <summary>
    /// ファイル操作処理。
    /// </summary>
    /// <remarks>
    /// <para>Pe の管理下で処理する。</para>
    /// <para>Pe から提供される。</para>
    /// </remarks>
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
    /// </summary>
    /// <remarks>
    /// <para>Pe の管理下で処理する。</para>
    /// <para>Pe から提供される。</para>
    /// </remarks>
    public interface IPluginPersistenceStorage
    {
        #region property

        /// <summary>
        /// 永続データアクセスは読み取り専用か。
        /// </summary>
        /// <remarks>
        /// <para>読み取り専用の場合、書き込み処理実行で例外発生。</para>
        /// </remarks>
        bool IsReadOnly { get; }

        #endregion

        #region function

        /// <summary>
        /// キー一覧を取得。
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetKeys();

        /// <summary>
        /// 指定データは存在するか。
        /// </summary>
        /// <param name="key">キー</param>
        /// <returns></returns>
        bool Exists(string key);

        /// <summary>
        /// 指定データを取得する。
        /// </summary>
        /// <typeparam name="TValue">格納データ型。</typeparam>
        /// <param name="key">キー</param>
        /// <param name="value">取得・変換できた場合に格納。</param>
        /// <returns>取得・変換できたか。</returns>
        bool TryGet<TValue>(string key, [MaybeNullWhen(returnValue: false)] out TValue value);

        /// <summary>
        /// 指定データを保存する。
        /// </summary>
        /// <remarks>
        /// <para><see cref="PluginPersistenceFormat.Text"/>を使用する以外は原則使用せず<see cref="Set{TValue}(string, TValue)"/>を用いること。</para>
        /// </remarks>
        /// <typeparam name="TValue">保存データ。</typeparam>
        /// <param name="key">キー。</param>
        /// <param name="value">値。</param>
        /// <param name="format">変換種別。</param>
        /// <returns>保存成功・失敗。</returns>
        bool Set<TValue>(string key, TValue value, PluginPersistenceFormat format)
            where TValue : notnull
        ;
        /// <summary>
        /// 現行バージョンにおける最適な型を使用して指定データを保存する。
        /// </summary>
        /// <typeparam name="TValue"><inheritdoc cref="SetValue{TValue}(string, TValue, PluginPersistenceFormat)"/></typeparam>
        /// <param name="key">キー。</param>
        /// <param name="value">値。</param>
        /// <returns>保存成功・失敗。</returns>
        bool Set<TValue>(string key, TValue value)
            where TValue : notnull
        ;

        /// <summary>
        /// 指定データを破棄する。
        /// </summary>
        /// <param name="key"></param>
        /// <returns>破棄成功。</returns>
        bool Delete(string key);

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
        /// </summary>
        /// <remarks>
        /// <para>次回起動時に存在しない。</para>
        /// </remarks>
        IPluginFileStorage Temporary { get; }

        #endregion
    }

    /// <summary>
    /// 永続データ操作処理グループ。
    /// </summary>
    /// <remarks>
    /// <para>Pe から提供される。</para>
    /// </remarks>
    public interface IPluginPersistence
    {
        #region property

        /// <summary>
        /// 通常データ。
        /// </summary>
        IPluginPersistenceStorage Normal { get; }
        /// <summary>
        /// 大きめのデータ。
        /// </summary>
        IPluginPersistenceStorage Large { get; }
        /// <summary>
        /// お好きに。
        /// </summary>
        IPluginPersistenceStorage Temporary { get; }

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
        IPluginPersistence Persistence { get; }

        #endregion
    }
}
