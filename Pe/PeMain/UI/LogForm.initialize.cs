/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 01/11/2014
 * 時刻: 00:16
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PeMain.Data;
using PeUtility;

namespace PeMain.UI
{
	/// <summary>
	/// Description of LogForm.
	/// </summary>
	public partial class LogForm
	{
		void Initialize(IEnumerable<LogItem> initLog)
		{
			// イメージリストをリソースから構築
			this._imageLogType = new ImageList();
			this._imageLogType.ColorDepth = ColorDepth.Depth32Bit;
			this._imageLogType.ImageSize = IconSize.Small.ToSize();
			this._imageLogType.Images.Add(LogType.Information.ToString(), global::PeMain.Properties.Images.Information);
			this._imageLogType.Images.Add(LogType.Warning.ToString(), global::PeMain.Properties.Images.Warning);
			this._imageLogType.Images.Add(LogType.Error.ToString(), global::PeMain.Properties.Images.Error);
			this.listLog.SmallImageList = this._imageLogType;
			//this.listLog.LargeImageList = this._imageLogType;
			
			// 
			this._logs.AddRange(initLog);
			this.listLog.VirtualListSize = this._logs.Count;
			this.listLog.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			
		}
	}
}
