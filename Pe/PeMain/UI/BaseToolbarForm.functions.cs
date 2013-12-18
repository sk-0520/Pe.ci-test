/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/18
 * 時刻: 14:14
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using Windows;

namespace PeMain.UI
{
	/// <summary>
	/// Description of BaseToolbarForm_function.
	/// </summary>
	public partial class BaseToolbarForm
	{
		private bool ResistAppBar()
		{
			Debug.Assert(!this.DesignMode);

			APPBARDATA appData = new APPBARDATA();
			appData.hWnd = Handle;
			
			this.callbackMessage = Windows.API.RegisterWindowMessage(MessageString);
			var registResult = Windows.API.SHAppBarMessage(ABM.ABM_NEW, ref appData);
			IsDocking = registResult.ToInt32() != 0;
			
			return IsDocking;
		}
		
		private bool UnResistAppBar()
		{
			Debug.Assert(!this.DesignMode);

			APPBARDATA appData = new APPBARDATA();
			appData.hWnd = Handle;

			var unregistResult = Windows.API.SHAppBarMessage(ABM.ABM_REMOVE, ref appData);
			
			IsDocking = false;
			this.callbackMessage = 0;
			
			return unregistResult.ToInt32() != 0;
		}
		
		/// <summary>
		/// ドッキングの実行
		/// 
		/// すでにドッキングされている場合はドッキングを再度実行する
		/// </summary>
		public void Docking()
		{
			if(DesignMode) {
				return;
			}
			if(DockType == DockType.None) {
				return;
			}
			
			ResistAppBar();
		}
	}
}
