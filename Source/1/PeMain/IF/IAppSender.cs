/*
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.IF;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.Define;

namespace ContentTypeTextNet.Pe.PeMain.IF
{
    public interface IAppSender
    {
        /// <summary>
        /// ウィンドウを追加。
        /// </summary>
        /// <param name="window"></param>
        void SendAppendWindow(Window window);
        /// <summary>
        /// ウィンドウを生成。
        /// </summary>
        /// <param name="windowKind"></param>
        /// <param name="extensionData"></param>
        /// <param name="parent"></param>
        Window SendCreateWindow(WindowKind windowKind, object extensionData, Window parent);
        /// <summary>
        /// ウィンドウ状態を更新。
        /// </summary>
        /// <param name="windowKind"></param>
        /// <param name="fromView"></param>
        void SendRefreshView(WindowKind windowKind, Window fromView);
        /// <summary>
        /// 対象インデックスから指定IDを削除。
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="indexKind"></param>
        void SendRemoveIndex(IndexKind indexKind, Guid guid, Timing timing);
        /// <summary>
        /// 対象インデックスを保存。
        /// </summary>
        /// <param name="indexKind"></param>
        /// <param name="timing"></param>
        void SendSaveIndex(IndexKind indexKind, Timing timing);
        /// <summary>
        /// 対象インデックスのボディ部を取得。
        /// </summary>
        /// <param name="indexKind"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        IndexBodyItemModelBase SendLoadIndexBody(IndexKind indexKind, Guid guid);
        /// <summary>
        /// 対象インデックスのボディ部を保存。
        /// </summary>
        /// <param name="indexBody"></param>
        /// <param name="guid"></param>
        void SendSaveIndexBody(IndexBodyItemModelBase indexBody, Guid guid, Timing timing);
        /// <summary>
        /// デバイスが変更されたことを通知。
        /// </summary>
        /// <param name="changedDevice"></param>
        void SendDeviceChanged(ChangedDevice changedDevice);
        /// <summary>
        /// クリップボードが変更された際に通知。
        /// </summary>
        void SendClipboardChanged();
        /// <summary>
        /// ホットキー。
        /// </summary>
        /// <param name="hotKeyId"></param>
        /// <param name="hotKeyModel"></param>
        void SendInputHotKey(HotKeyId hotKeyId, HotKeyModel hotKeyModel);
        /// <summary>
        /// バルーン通知。
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="logKind">諸事情によりHardcodet.Wpf.TaskbarNotification.BalloonIconじゃなくてLogKind の None, Information, Warning, Errorをそれぞれ指定</param>
        void SendInformationTips(string title, string message, LogKind logKind);
        /// <summary>
        /// アプリケーション内コマンド実行。
        /// </summary>
        /// <param name="applicationCommand"></param>
        /// <param name="arg"></param>
        void SendApplicationCommand(ApplicationCommand applicationCommand, object sender, ApplicationCommandArg arg);
        /// <summary>
        /// ユーザー情報収集開始。
        /// </summary>
        void SendUserInformation();
    }
}
