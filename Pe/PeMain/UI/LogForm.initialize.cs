/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 01/11/2014
 * 時刻: 00:16
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Linq;
using System.Windows.Forms;
using PeMain.Data;
using PeSkin;
using PeUtility;

namespace PeMain.UI
{
	/// <summary>
	/// Description of LogForm.
	/// </summary>
	partial class LogForm
	{
		void Initialize()
		{
			// イメージリストをリソースから構築
			this._imageLogType = new ImageList();
			this._imageLogType.ColorDepth = ColorDepth.Depth32Bit;
			this._imageLogType.ImageSize = IconScale.Small.ToSize();
			this._imageLogType.Images.Add(LogType.Information.ToString(), global::PeMain.Properties.Images.Information);
			this._imageLogType.Images.Add(LogType.Warning.ToString(), global::PeMain.Properties.Images.Warning);
			this._imageLogType.Images.Add(LogType.Error.ToString(), global::PeMain.Properties.Images.Error);
			this.listLog.SmallImageList = this._imageLogType;
			//this.listLog.LargeImageList = this._imageLogType;
			
		}
	}
}
