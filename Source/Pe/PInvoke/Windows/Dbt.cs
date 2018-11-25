/*
This file is part of PInvoke.

PInvoke is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

PInvoke is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with PInvoke.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;

namespace ContentTypeTextNet.Library.PInvoke.Windows
{
	public enum DBT
	{
		/// <summary>
		/// デバイスが使用可能
		/// </summary>
		DBT_DEVICEARRIVAL = 0x8000,
		/// <summary>
		/// 設定変更要求キャンセル
		/// </summary>
		DBT_CONFIGCHANGECANCELED = 0x0019,
		/// <summary>
		/// 設定が変更された
		/// </summary>
		DBT_CONFIGCHANGED = 0x0018,
		/// <summary>
		/// ドライバー定義のカスタムイベント
		/// </summary>
		DBT_CUSTOMEVENT = 0x8006,
		/// <summary>
		/// デバイス停止要求
		/// </summary>
		DBT_DEVICEQUERYREMOVE = 0x8001,
		/// <summary>
		/// デバイス停止要求失敗
		/// </summary>
		DBT_DEVICEQUERYREMOVEFAILED = 0x8002,
		/// <summary>
		/// デバイスが停止
		/// </summary>
		DBT_DEVICEREMOVECOMPLETE = 0x8004,
		/// <summary>
		/// デバイス停止中
		/// </summary>
		DBT_DEVICEREMOVEPENDING = 0x8003,
		/// <summary>
		/// 独自イベント発行
		/// </summary>
		DBT_DEVICETYPESPECIFIC = 0x8005,
		/// <summary>
		/// デバイス状態変更
		/// </summary>
		DBT_DEVNODES_CHANGED = 0x0007,
		/// <summary>
		/// 設定変更要求
		/// </summary>
		DBT_QUERYCHANGECONFIG = 0x0017,
		/// <summary>
		/// なんだろう。ユーザー定義？
		/// </summary>
		DBT_USERDEFINED = 0xFFFF,
	}
}
