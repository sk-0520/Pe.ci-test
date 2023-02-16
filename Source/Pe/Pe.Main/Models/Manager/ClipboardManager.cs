using System;
using System.Windows;
using ContentTypeTextNet.Pe.Standard.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
    public enum ClipboardNotify
    {
        None,
        User,
    }

    /// <summary>
    /// クリップボード操作用。
    /// <para>もっかしたら<see cref="IOrderManager"/>で完結するかも。</para>
    /// </summary>
    public interface IClipboardManager
    {
        #region function

        /// <summary>
        /// <see cref="Clipboard.SetDataObject(object)"/> を用いたコピー処理。
        /// <para>本処理にて例外は握りつぶされる。</para>
        /// </summary>
        /// <param name="data">クリップボードにコピーするデータ。</param>
        /// <param name="clipboardNotify">通知方法。</param>
        /// <returns>クリップボードへコピーできたか。</returns>
        bool CopyData(IDataObject data, ClipboardNotify clipboardNotify);

        /// <summary>
        /// <see cref="Clipboard.SetText(string, TextDataFormat)"/>(<see cref="TextDataFormat.UnicodeText"/>) を用いたコピー処理。
        /// <para>本処理にて例外は握りつぶされる。</para>
        /// </summary>
        /// <param name="data">クリップボードにコピーする文字列。</param>
        /// <param name="clipboardNotify">通知方法。</param>
        /// <returns>クリップボードへコピーできたか。</returns>
        bool CopyText(string data, ClipboardNotify clipboardNotify);

        /// <summary>
        /// <see cref="Clipboard.Clear"/> を用いたクリップボードクリア処理。
        /// <para>まったくもっていらんけど <see cref="IClipboardManager"/> の名前なので一応ね。一応。</para>
        /// </summary>
        void Clear();

        #endregion
    }

    public class ClipboardManager: ManagerBase, IClipboardManager
    {
        public ClipboardManager(IDiContainer diContainer, ILoggerFactory loggerFactory)
            : base(diContainer, loggerFactory)
        { }

        #region property
        #endregion

        #region function

        #endregion

        #region IClipboardManager

        /// <inheritdoc cref="IClipboardManager.CopyData(IDataObject, ClipboardNotify)"/>
        public bool CopyData(IDataObject data, ClipboardNotify clipboardNotify)
        {
            if(data == null) {
                throw new ArgumentNullException(nameof(data));
            }

            try {
                Clipboard.SetDataObject(data);
                return true;
            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
                return false;
            }
        }

        /// <inheritdoc cref="IClipboardManager.CopyText(string, ClipboardNotify)"/>
        public bool CopyText(string data, ClipboardNotify clipboardNotify)
        {
            if(data == null) {
                throw new ArgumentNullException(nameof(data));
            }

            try {
                Clipboard.SetText(data, TextDataFormat.UnicodeText);
                return true;
            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
                return false;
            }
        }

        /// <inheritdoc cref="IClipboardManager.Clear"/>
        public void Clear()
        {
            Clipboard.Clear();
        }

        #endregion
    }
}
