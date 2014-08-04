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
using System.Drawing;
using System.Windows.Forms;
using PeMain.Data;
using PeMain.Logic;

namespace PeMain.UI
{
	partial class NoteForm
	{
		public void SetCommonData(CommonData commonData)
		{
			this._initialized = false;
			
			CommonData = commonData;
			
			ApplySetting();
			

			this._changed = false;
			this._initialized = true;
		}
		
		void ApplySetting()
		{
			
		}
		
		IEnumerable<NoteCommand> GetCommandList()
		{
			return new [] {
				NoteCommand.Topmost,
				NoteCommand.Compact,
				NoteCommand.Close,
			};
		}
		
		SkinNoteStatus GetNoteStatus()
		{
			return new SkinNoteStatus();
		}
		
		void ClickCommand(NoteCommand noteCommand)
		{
			switch(noteCommand) {
				case NoteCommand.Topmost:
					{
						NoteItem.Topmost = !NoteItem.Topmost;
						TopMost = NoteItem.Topmost;
						Changed = true;
						Invalidate();
						break;
					}
					
				case NoteCommand.Compact:
					{
						NoteItem.Compact = !NoteItem.Compact;
						Changed = true;
						
						ChangeCompact(NoteItem.Compact, NoteItem.Size);
						
						break;
					}
					
				case NoteCommand.Close:
					{
						if(true) {
							NoteItem.Visibled = false;
							Changed = true;
							SaveItem();
							Close();
						} else {
							// TODO: 削除
						}
						break;
					}
					
				default:
					Debug.Assert(false, noteCommand.ToString());
					break;
			}
		}
		
		void ChangeCompact(bool compact, Size size)
		{
			if(compact) {
				var edge = this.CommonData.Skin.GetNoteWindowEdgePadding();
				var titleArea = GetTitleArea();
				Size = new Size(titleArea.Width + edge.Horizontal, titleArea.Height + edge.Vertical);
			} else {
				Size = size;
			}

		}
		
		
		void DrawCommand(Point point, Func<bool, ButtonState, ButtonState> inFirstDg, Action<NoteCommand> prevDrawDg, Action lastInDg, bool elseProcess)
		{
			var captionArea = CommonData.Skin.GetNoteCaptionArea(ClientSize);
			if(!captionArea.Size.IsEmpty) {
				var active = this == Form.ActiveForm;
				var noteStatus = GetNoteStatus();
				if(captionArea.Contains(point)) {
					using(var g = CreateGraphics()) {
						foreach(var command in GetCommandList()) {
							var commandArea = CommonData.Skin.GetNoteCommandArea(captionArea, command);
							var nowState = ButtonState.None;
							var prevState = this._commandStateMap[command];
							nowState = inFirstDg(commandArea.Contains(point), prevState);
							
							if(nowState != ButtonState.None) {
								if(prevDrawDg != null) {
									prevDrawDg(command);
								}
								if(nowState != prevState && this.Created) {
									CommonData.Skin.DrawNoteCommand(g, commandArea, active, noteStatus, NoteItem.Style.ForeColor, NoteItem.Style.BackColor, command, nowState);
									this._commandStateMap[command] = nowState;
								}
							}
						}
					}
					if(lastInDg != null) {
						lastInDg();
					}
				} else {
					if(elseProcess) {
						using(var g = CreateGraphics()) {
							foreach(var pair in this._commandStateMap) {
								if(pair.Value != ButtonState.Normal) {
									var commandArea = CommonData.Skin.GetNoteCommandArea(captionArea, pair.Key);
									CommonData.Skin.DrawNoteCommand(g, commandArea, active, noteStatus, NoteItem.Style.ForeColor, NoteItem.Style.BackColor, pair.Key, ButtonState.Normal);
								}
							}
						}
					}
				}
			}
		}
		
		Rectangle GetTitleArea()
		{
			return this.CommonData.Skin.GetNoteCaptionArea(ClientSize);
		}
		
		Rectangle GetBodyArea()
		{
			return GetBodyArea(
				this.CommonData.Skin.GetNoteWindowEdgePadding(),
				this.CommonData.Skin.GetNoteCaptionArea(ClientSize)
			);
		}
		Rectangle GetBodyArea(Padding edge, Rectangle captionArea)
		{
			return new Rectangle(
				new Point(edge.Left, captionArea.Bottom),
				new Size(ClientSize.Width - edge.Horizontal, ClientSize.Height - (edge.Vertical + captionArea.Height))
			);
		}

		void ResizeInputTitleArea()
		{
			var titleArea = GetTitleArea();
			this.inputTitle.Location = titleArea.Location;
			this.inputTitle.Size = titleArea.Size;
		}
		
		void ResizeInputBodyArea()
		{
			var bodyArea = GetBodyArea();
			this.inputBody.Location = bodyArea.Location;
			this.inputBody.Size = bodyArea.Size;
		}
		
		void ShowInputTitleArea()
		{
			this.inputTitle.Text = NoteItem.Title;
			this.inputTitle.Font = CommonData.MainSetting.Note.CaptionFontSetting.Font;
			
			if(!this.inputTitle.Visible) {
				ResizeInputTitleArea();
				this.inputTitle.Visible = true;
				this.inputTitle.Focus();
			}
		}
		
		void ShowInputBodyArea()
		{
			this.inputBody.Text = NoteItem.Body;
			this.inputBody.Font = NoteItem.Style.FontSetting.Font;
			
			if(!this.inputBody.Visible) {
				ResizeInputBodyArea();
				this.inputBody.Visible = true;
				this.inputBody.Focus();
			}
		}
		
		void HiddenInputTitleArea()
		{
			var value = this.inputTitle.Text.Trim();
			var change = NoteItem.Title != value;
			if(change) {
				NoteItem.Title = value;
				this._changed |= true;
			}
			this.inputTitle.Visible = false;
		}
		
		void HiddenInputBodyArea()
		{
			var value = this.inputBody.Text.Trim();
			var change = NoteItem.Title != value;
			if(change) {
				NoteItem.Body = value;
				this._changed |= true;
			}
			this.inputBody.Visible = false;
		}
		
		void ShowContextMenu(Point point)
		{
			this.contextMenu.Show(this, point);
		}
		
		void SaveItem()
		{
			if(this._changed) {
				CommonData.MainSetting.Note.ResistItem(NoteItem);
				this._changed = false;
				//*
				var map = new Dictionary<string, string>() {
					{"title", NoteItem.Title},
					{"body", NoteItem.Body},
				};
				CommonData.RootSender.ShowBalloon(
					ToolTipIcon.Info,
					CommonData.Language["memo/save"],
					CommonData.Language["memo/content", map]
				);
				//*/
			}
		}

	}
}