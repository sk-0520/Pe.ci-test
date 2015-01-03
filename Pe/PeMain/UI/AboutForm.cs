/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/09/07
 * 時刻: 17:52
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using ContentTypeTextNet.Pe.Library.Skin;
using ContentTypeTextNet.Pe.Library.Utility;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	/// <summary>
	/// 情報。
	/// </summary>
	public partial class AboutForm : Form, ISetCommonData
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
		CommonData CommonData { get; set; }
		public bool CheckUpdate { get; private set; }

		List<ComponentInfo> ComponentInfoList { get; set; }

		string Separator { get { return "____________"; } }
		#endregion ////////////////////////////////////

		#region ISetCommonData

		public void SetCommonData(CommonData commonData)
		{
			CommonData = commonData;

			ApplySetting();
		}

		#endregion ////////////////////////////////////

		#region initialize
		void Initialize()
		{
			PointingUtility.AttachmentDefaultButton(this);

			var iconSize = IconScale.Big.ToSize();
			using(var icon = new Icon(global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Icon_App, iconSize)) {
				var image = new Bitmap(iconSize.Width, iconSize.Height);
				using(var g = Graphics.FromImage(image)) {
					g.DrawIcon(icon, new Rectangle(Point.Empty, iconSize));
				}
				this.imageIcon.Image = image;
			}

			this.labelAppName.Text = Literal.programName;
			this.labelAppVersion.Text = Literal.ApplicationVersion;
			this.labelConfiguration.Text = string.Format(
				"{0}: {1}bit",
#if DEBUG
				"DEBUG",
#else
				"RELEASE",
#endif
 Environment.Is64BitProcess ? "64" : "32"
			);

			this.linkAbout.Text = Literal.AboutWebURL;
			this.linkMail.Text = "mailto:" + Literal.AboutMailAddress;
			this.linkDevelopment.Text = Literal.AboutDevelopmentURL;
			this.linkDiscussion.Text = Literal.DiscussionURL;

			var xml = XElement.Load(Path.Combine(Literal.ApplicationDocumentDirPath, "components.xml"));
			ComponentInfoList = xml
				.Elements()
				.Select(e => new ComponentInfo(e))
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
		void ApplyLanguage()
		{
			UIUtility.SetDefaultText(this, CommonData.Language);

			this.commandExecuteDir.SetLanguage(CommonData.Language);
			this.commandDataDir.SetLanguage(CommonData.Language);
			this.commandBackupDir.SetLanguage(CommonData.Language);
			this.commandChangelog.SetLanguage(CommonData.Language);
			this.commandUpdate.SetLanguage(CommonData.Language);

			this.labelUserenv.SetLanguage(CommonData.Language);
			this.linkCopyShort.SetLanguage(CommonData.Language);
			this.linkCopyLong.SetLanguage(CommonData.Language);

			this.gridComponents_columnName.SetLanguage(CommonData.Language);
			this.gridComponents_columnType.SetLanguage(CommonData.Language);
			this.gridComponents_columnLicense.SetLanguage(CommonData.Language);
		}
		#endregion ////////////////////////////////////

		#region function

		void ApplySetting()
		{
			ApplyLanguage();
		}

		void OpenDirectory(string path)
		{
			Executer.OpenDirectory(path, CommonData, null);
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
			var link = linkLabel.Text;
			if(string.IsNullOrWhiteSpace(link)) {
				return;
			}
			
			try {
				Executer.RunCommand(link, CommonData);
			} catch(Exception ex) {
				CommonData.Logger.Puts(LogType.Error, link, ex);
			}
		}
		
		void CommandExecuteDir_Click(object sender, EventArgs e)
		{
			OpenDirectory(Literal.ApplicationRootDirPath);
		}
		
		void CommandDataDir_Click(object sender, EventArgs e)
		{
			OpenDirectory(Literal.UserSettingDirPath);
		}
		
		void CommandBackupDir_Click(object sender, EventArgs e)
		{
			OpenDirectory(Literal.UserBackupDirPath);
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
			Executer.OpenFile(path, CommonData);
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
						Executer.RunCommand(link, CommonData);
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
			list.Add("Type: " +
				#if DEBUG
				"DEBUG"
				#else
				"RELEASE"
				#endif
			);
			list.Add("Process: " + (Environment.Is64BitProcess ? "64": "32"));
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
