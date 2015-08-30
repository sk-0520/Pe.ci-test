using System;
using System.Collections.Generic;
using System.IO;
namespace ContentTypeTextNet.Pe.PeMain.UI
{
	using System.Text;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Logic;
	using ContentTypeTextNet.Pe.PeMain.UI.Ex;

	/// <summary>
	/// 使用許諾。
	/// </summary>
	partial class AcceptForm : CommonForm
	{
		#region define
		#endregion ////////////////////////////////////

		#region variable
		#endregion ////////////////////////////////////

		public AcceptForm()
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

		#endregion ////////////////////////////////////

		#region ISetCommonData
		//public void SetCommonData(CommonData commonData)
		//{
		//	CommonData = commonData;

		//	ApplySetting();
		//}
		#endregion ////////////////////////////////////

		#region initialize
		void Initialize()
		{
			WebBrowserUtility.AttachmentNewWindow(this.webDocument);
		}
		#endregion ////////////////////////////////////

		#region language
		protected override void ApplyLanguage()
		{
			base.ApplyLanguage();

			UIUtility.SetDefaultText(this, CommonData.Language);
#if RELEASE
			this.AcceptButton = null;
#endif

			this.selectUpdateCheck.SetLanguage(CommonData.Language);
			this.selectUpdateCheckRC.SetLanguage(CommonData.Language);

			var acceptFilePath = Path.Combine(Literal.ApplicationLanguageDirPath, CommonData.Language.AcceptFileName);
			var acceptFileSource = File.ReadAllText(acceptFilePath);
			var acceptMap = new Dictionary<string, string>() {
				{"WEB", Literal.AboutWebURL },
				{"DEVELOPMENT", Literal.AboutDevelopmentURL },
				{"MAIL", Literal.AboutMailAddress },
				{"DISCUSSION", Literal.DiscussionURL },
				{"HELP", Literal.HelpDocumentURI },
				{"STYLE", File.ReadAllText(Path.Combine(Literal.ApplicationStyleDirPath, "common.css"), Encoding.UTF8) },
			};
			var acceptFileReplaced = acceptFileSource.ReplaceRangeFromDictionary("${", "}", acceptMap);
			this.webDocument.DocumentStream = new MemoryStream(Encoding.UTF8.GetBytes(acceptFileReplaced));
		}
		#endregion ////////////////////////////////////

		#region skin
		protected override void ApplySkin()
		{
			// この時点でスキンは未設定
			//base.ApplySkin();
		}
		#endregion ////////////////////////////////////

		#region function
		protected override void ApplySetting()
		{
			base.ApplySetting();

			this.selectUpdateCheck.Checked = CommonData.MainSetting.Running.CheckUpdate;
			this.selectUpdateCheckRC.Checked = CommonData.MainSetting.Running.CheckUpdateRC;
		}
		#endregion ////////////////////////////////////

		void CommandAccept_Click(object sender, EventArgs e)
		{
			CommonData.MainSetting.Running.CheckUpdate = this.selectUpdateCheck.Checked;
			CommonData.MainSetting.Running.CheckUpdateRC = this.selectUpdateCheckRC.Checked;
			
			DialogResult = DialogResult.OK;
		}
		
		void AcceptForm_Shown(object sender, EventArgs e)
		{
			UIUtility.ShowFrontActive(this);
		}
	}
}
