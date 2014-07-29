/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/03/12
 * 時刻: 4:30
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Windows.Forms;
using PeMain.Logic;

namespace PeMain.UI
{
	partial class NoteForm
	{
		public void SetCommonData(CommonData commonData)
		{
			CommonData = commonData;
			
			ApplySetting();
		}
		
		void ApplySetting()
		{
			
		}
		
		SkinNoteStatus GetNoteStatus()
		{
			return new SkinNoteStatus();
		}
		
		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			//base.OnPaintBackground(pevent);
		}
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			this.Invalidate();
		}
	}
}
