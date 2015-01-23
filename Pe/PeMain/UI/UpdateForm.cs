namespace ContentTypeTextNet.Pe.PeMain.UI
{
	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using System.IO;
	using System.Net;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic;

	/// <summary>
	/// アップデートチェック。
	/// 
	/// 実行は PeUpdater にお任せ。
	/// </summary>
	public partial class UpdateForm : CommonForm
	{
		#region define
		#endregion ////////////////////////////////////

		#region static
		#endregion ////////////////////////////////////

		#region variable
		#endregion ////////////////////////////////////

		public UpdateForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			Initialize();
		}


		#region property
		//CommonData CommonData { get; set; }
		public UpdateData UpdateData { get; set; }
		#endregion ////////////////////////////////////

		#region ISetCommonData
		//public void SetCommonData(CommonData commonData)
		//{
		//	CommonData = commonData;

		//	ApplyLanguage();
		//	ApplySetting();

		//	ApplyUpdate();
		//}
		#endregion ////////////////////////////////////

		#region override
		#endregion ////////////////////////////////////

		#region initialize
		void Initialize()
		{
			PointingUtility.AttachmentDefaultButton(this);
			WebBrowserUtility.AttachmentNewWindow(this.webUpdate);
		}
		#endregion ////////////////////////////////////

		#region language
		protected override void ApplyLanguage()
		{
			base.ApplyLanguage();

			UIUtility.SetDefaultText(this, CommonData.Language);
			this.AcceptButton = null;

			var map = new Dictionary<string, string>() {
				{ AppLanguageName.versionNow,  Literal.ApplicationVersion },
				{ AppLanguageName.versionNext, UpdateData.Info.Version },
				{ AppLanguageName.versionType, UpdateData.Info.IsRcVersion ? "${version-rc}": "${version-release}" },
			};
			var version = CommonData.Language[this.labelVersion.Text.Substring(1)];
			version = CommonData.Language[version, map];
			version = CommonData.Language[version, map];
			version = CommonData.Language[version, map];
			this.labelVersion.Text = version;
		}
		#endregion ////////////////////////////////////

		#region function
		protected override void ApplySetting()
		{
			base.ApplySetting();

			if(UpdateData.Info.IsRcVersion) {
				this.labelVersion.BorderStyle = BorderStyle.FixedSingle;
				this.labelVersion.ForeColor = Color.Red;
				this.labelVersion.BackColor = Color.Black;
			}

			ApplyUpdate();
		}
		
		void ApplyUpdate()
		{
			byte[] httpData = null;
			using(var web = new WebClient()) {
				var url = UpdateData.Info.IsRcVersion ? Literal.ChangeLogRcURL: Literal.ChangeLogURL; 
				httpData = web.DownloadData(url);
			}
			this.webUpdate.DocumentStream = new MemoryStream(httpData);
//			WebBrowserUtility.AttachmentNavigating(this.webUpdate);
		}
		#endregion ////////////////////////////////////

		void CommandOk_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}
		
		void UpdateForm_Shown(object sender, EventArgs e)
		{
			UIUtility.ShowFrontActive(this);
		}
	}
}
