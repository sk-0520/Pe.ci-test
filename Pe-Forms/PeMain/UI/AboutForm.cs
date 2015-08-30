namespace ContentTypeTextNet.Pe.PeMain.UI
{
	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using System.IO;
	using System.Linq;
	using System.Windows.Forms;
	using System.Xml.Linq;
	using ContentTypeTextNet.Pe.Library.Skin;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Kind;
	using ContentTypeTextNet.Pe.PeMain.Logic;
	using ContentTypeTextNet.Pe.PeMain.UI.Ex;

	/// <summary>
	/// 情報。
	/// </summary>
	public partial class AboutForm : CommonForm
	{
		#region define
		class ComponentInfo
		{
			public ComponentInfo(XElement x)
			{
				Name = x.Attribute("name").Value;
				Type = x.Attribute("type").Value;
				URI = x.Attribute("uri").Value;
				License = x.Attribute("license").Value;
			}

			public string Name { get; private set; }
			public string Type { get; private set; }
			public string URI { get; private set; }
			public string License { get; private set; }
		}
		#endregion ////////////////////////////////////

		#region variable
		#endregion ////////////////////////////////////

		public AboutForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			Initialize();
		}

		#region property
		//CommonData CommonData { get; set; }
		public bool CheckUpdate { get; private set; }

		List<ComponentInfo> ComponentInfoList { get; set; }

		string Separator { get { return "____________"; } }
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
			PointingUtility.AttachmentDefaultButton(this);

			this.imageIcon.Image = IconUtility.ImageFromIcon(global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Icon_App, IconScale.Big);

			this.labelAppName.Text = Literal.programName;
			this.labelAppVersion.Text = Literal.ApplicationVersion;
			this.labelConfiguration.Text = string.Format(
				"{0}: {1}bit",
				Literal.BuildType,
				Literal.BuildProcess
			);

			Action<LinkLabel, string> setLink = (linkLabel, path) => {
				linkLabel.Links.Add(new LinkLabel.Link(0, path.Length, path));
			};
			setLink(this.linkAbout, Literal.AboutWebURL);
			setLink(this.linkMail, "mailto:" + Literal.AboutMailAddress);
			setLink(this.linkDevelopment, Literal.AboutDevelopmentURL);
			setLink(this.linkDiscussion, Literal.DiscussionURL);
			setLink(this.linkFeedback, Literal.FeedbackURL);

			var xml = XElement.Load(Path.Combine(Literal.ApplicationDocumentDirPath, "components.xml"));
			ComponentInfoList = xml
				.Elements()
				.Select(e => new ComponentInfo(e))
				.OrderBy(ci => ci.Type.ToLower() != "Library".ToLower())
				.ThenBy(ci => ci.Name)
				.ToList()
			;
			this.gridComponents_columnName.DataPropertyName = "Name";
			this.gridComponents_columnType.DataPropertyName = "Type";
			this.gridComponents_columnLicense.DataPropertyName = "License";
			this.gridComponents.AutoGenerateColumns = false;
			this.gridComponents.DataSource = new BindingSource(ComponentInfoList, string.Empty);
		}
		#endregion ////////////////////////////////////

		#region language
		protected override void ApplyLanguage()
		{
			base.ApplyLanguage();

			UIUtility.SetDefaultText(this, CommonData.Language);

			this.commandExecuteDir.SetLanguage(CommonData.Language);
			this.commandDataDir.SetLanguage(CommonData.Language);
			this.commandChangelog.SetLanguage(CommonData.Language);
			this.commandUpdate.SetLanguage(CommonData.Language);

			this.linkAbout.SetLanguage(CommonData.Language);
			this.linkMail.SetLanguage(CommonData.Language);
			this.linkDevelopment.SetLanguage(CommonData.Language);
			this.linkDiscussion.SetLanguage(CommonData.Language);
			this.linkFeedback.SetLanguage(CommonData.Language);

			this.labelUserenv.SetLanguage(CommonData.Language);
			this.linkCopyShort.SetLanguage(CommonData.Language);
			this.linkCopyLong.SetLanguage(CommonData.Language);

			this.gridComponents_columnName.SetLanguage(CommonData.Language);
			this.gridComponents_columnType.SetLanguage(CommonData.Language);
			this.gridComponents_columnLicense.SetLanguage(CommonData.Language);

			this.tabAbout_pageApp.SetLanguage(CommonData.Language);
			this.tabAbout_pageComponent.SetLanguage(CommonData.Language);
		}
		#endregion ////////////////////////////////////

		#region skin
		protected override void ApplySkin()
		{
			base.ApplySkin();

			this.commandExecuteDir.Image = CommonData.Skin.GetImage(SkinImage.Dir);
			this.commandDataDir.Image = CommonData.Skin.GetImage(SkinImage.Dir);

			this.commandChangelog.Image = CommonData.Skin.GetImage(SkinImage.Changelog);
			this.commandUpdate.Image = CommonData.Skin.GetImage(SkinImage.Update);
		}
		#endregion ////////////////////////////////////

		#region function

		protected override void ApplySetting()
		{
			base.ApplySetting();
			foreach(var command in this.panelCommand.Controls.OfType<Button>()) {
				command.Size = Size.Empty;
				command.AutoSize = true;
				command.Size = new Size(command.Width, this.commandOk.Height);
			}
		}

		void OpenDirectory(string path)
		{
			Executor.OpenDirectory(path, CommonData, null);
		}

		#endregion ////////////////////////////////////

		void CommandOk_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}
		
		void Link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			var linkLabel = (LinkLabel)sender;
			linkLabel.LinkVisited = true;
			var link = (string)linkLabel.Links[0].LinkData;
			if(string.IsNullOrWhiteSpace(link)) {
				return;
			}
			
			try {
				Executor.RunCommand(link, CommonData);
			} catch(Exception ex) {
				CommonData.Logger.Puts(LogType.Error, link, ex);
			}
		}
		
		void CommandExecuteDir_Click(object sender, EventArgs e)
		{
			OpenDirectory(Literal.ApplicationRootDirectoryPath);
		}
		
		void CommandDataDir_Click(object sender, EventArgs e)
		{
			OpenDirectory(Literal.UserSettingDirectoryPath);
		}
		
		void CommandUpdate_Click(object sender, EventArgs e)
		{
			var caption = CommonData.Language["about/update/check/dialog/caption"];
			var message = CommonData.Language["about/update/check/dialog/message"];
			var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
			if(result == DialogResult.Yes) {
				CheckUpdate = true;
				Close();
			}
		}
		
		void CommandChangelog_Click(object sender, EventArgs e)
		{
			var path = Path.Combine(Literal.ApplicationDocumentDirPath, "changelog.xml");
			Executor.OpenFile(path, CommonData);
		}
		
		void GridComponents_CellContentClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
		{
			if(e.ColumnIndex == this.gridComponents_columnName.Index && e.RowIndex != -1) {
				var cell = this.gridComponents.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewLinkCell;
				if(cell != null) {
					var rowIndex = cell.RowIndex;
					if(0 <= rowIndex && rowIndex < ComponentInfoList.Count) {
						var componentInfo = ComponentInfoList[rowIndex];
						var link = componentInfo.URI;
						Executor.RunCommand(link, CommonData);
						cell.LinkVisited = true;
					}
				}
			}
		}
		
		void linkCopyShort_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			var list = new List<string>();
			list.Add("Software: " + Literal.Version.ProductName);
			list.Add("Version: " + Literal.ApplicationVersion);
			list.Add("Type: " + Literal.BuildType);
			list.Add("Process: " + Literal.BuildProcess);
			list.Add("Platform: " + (Environment.Is64BitOperatingSystem ? "64": "32"));
			list.Add("OS: " + System.Environment.OSVersion);
			list.Add("CLI: " + System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion());

			ClipboardUtility.CopyText(Environment.NewLine + Separator + Environment.NewLine + string.Join(Environment.NewLine, list.Select(s => "    " + s)) + Environment.NewLine + Environment.NewLine, CommonData);
		}
		
		void linkCopyLong_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			ClipboardUtility.CopyText(Environment.NewLine + Separator + Environment.NewLine + string.Join(Environment.NewLine, new ContentTypeTextNet.Pe.PeMain.Logic.AppInformation().ToString().SplitLines().Select(s => "    " + s)) + Environment.NewLine + Environment.NewLine, CommonData);
		}
	}
}
