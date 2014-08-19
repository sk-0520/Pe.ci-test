/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/03/12
 * 時刻: 4:30
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using PeMain.Data;
using PeMain.Logic;

namespace PeMain.UI
{
	partial class NoteForm
	{
		void Initialize()
		{
			this._commandStateMap = new Dictionary<NoteCommand, ButtonState>() {
				{ NoteCommand.Close, ButtonState.Normal },
				{ NoteCommand.Compact, ButtonState.Normal },
				{ NoteCommand.Topmost, ButtonState.Normal },
			};
			
			var colorControls = new [] {
				new { Control = contextMenu_fore, Title = "note/style/color-fore", Default = Literal.noteFore },
				new { Control = contextMenu_back, Title = "note/style/color-back", Default = Literal.noteBack },
			};
			foreach(var control in colorControls) {
				var isFore = control.Control == contextMenu_fore;
				var colorList = new [] {
					new ColorData(isFore ? Literal.noteForeColorBlack:  Literal.noteBackColorBlack , control.Title, "note/style/color/black"),
					new ColorData(isFore ? Literal.noteForeColorWhite:  Literal.noteBackColorWhite , control.Title, "note/style/color/white"),
					new ColorData(isFore ? Literal.noteForeColorRed:    Literal.noteBackColorRed   , control.Title, "note/style/color/red"),
					new ColorData(isFore ? Literal.noteForeColorGreen:  Literal.noteBackColorGreen , control.Title, "note/style/color/green"),
					new ColorData(isFore ? Literal.noteForeColorBlue:   Literal.noteBackColorBlue  , control.Title, "note/style/color/blue"),
					new ColorData(isFore ? Literal.noteForeColorYellow: Literal.noteBackColorYellow, control.Title, "note/style/color/yellow"),
					new ColorData(isFore ? Literal.noteForeColorOrange: Literal.noteBackColorOrange, control.Title, "note/style/color/orange"),
					new ColorData(isFore ? Literal.noteForeColorPurple: Literal.noteBackColorPurple, control.Title, "note/style/color/purple"),
				};
				control.Control.ComboBox.BindingContext = BindingContext;
				control.Control.Attachment(colorList, control.Default);
			}
			this.inputTitle.AutoSize = false;
		}
	}
}
