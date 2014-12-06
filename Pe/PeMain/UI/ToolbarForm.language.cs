/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/04
 * 時刻: 23:53
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using ContentTypeTextNet.Pe.PeMain.Logic;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	/// <summary>
	/// Description of ToolbarForm_language.
	/// </summary>
	partial class ToolbarForm
	{
		void ApplyLanguage()
		{
			Debug.Assert(CommonData != null);
			Debug.Assert(CommonData.Language != null);
			
			UIUtility.SetDefaultText(this, CommonData.Language);
		}
	}
}
