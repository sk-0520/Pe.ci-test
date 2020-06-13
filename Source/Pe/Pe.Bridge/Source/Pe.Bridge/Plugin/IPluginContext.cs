using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Bridge.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Bridge.Plugin
{
    /// <summary>
    /// 各種コンテキストの共通項目。
    /// <para>基本的にコンテキストは持ち歩いて使用ることはできない。</para>
    /// </summary>
    public interface IPluginCommonContext
    {
        #region property

        /// <summary>
        /// このコンテキストが使用可能か。
        /// </summary>
        bool IsAvailable { get; }

        #endregion
    }

    /// <summary>
    /// プラグインのコンストラクタ時の Pe との架け橋。
    /// <para>持ち歩かないこと。</para>
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface IPluginConstructorContext: IPluginCommonContext
    {
        #region property

        ILoggerFactory LoggerFactory { get; }

        #endregion
    }

    /// <summary>
    /// プラグイン初期化時の Pe との架け橋。
    /// <para>持ち歩かないこと。</para>
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface IPluginInitializeContext: IPluginCommonContext
    {
        #region property

        /// <summary>
        /// ストレージ操作。
        /// </summary>
        IPluginStorage Storage { get; }

        #endregion
    }

    /// <summary>
    /// プラグイン終了時の Pe との架け橋。
    /// <para>持ち歩かないこと。</para>
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface IPluginUninitializeContext: IPluginCommonContext
    {
        #region property

        /// <summary>
        /// ストレージ操作。
        /// </summary>
        IPluginStorage Storage { get; }

        #endregion
    }

    /// <summary>
    /// プラグイン機能読み込み時の Pe との架け橋。
    /// <para>持ち歩かないこと。</para>
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface IPluginLoadContext: IPluginCommonContext
    {
        #region property

        /// <summary>
        /// ストレージ操作。
        /// </summary>
        IPluginStorage Storage { get; }

        #endregion
    }

    /// <summary>
    /// プラグイン機能終了時の Pe との架け橋。
    /// <para>持ち歩かないこと。</para>
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface IPluginUnloadContext: IPluginCommonContext
    {
        #region property

        /// <summary>
        /// ストレージ操作。
        /// </summary>
        IPluginStorage Storage { get; }

        #endregion
    }

    /// <summary>
    /// プラグインと Pe の架け橋。
    /// <para>持ち歩かないこと(必要箇所で都度渡すので勘弁して)。</para>
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface IPluginContext: IPluginCommonContext
    {
        #region property

        /// <summary>
        /// ストレージ操作。
        /// </summary>
        IPluginStorage Storage { get; }

        #endregion
    }
}
