/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/09/07
 * 時刻: 17:53
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using PeSkin;
using PeUtility;

namespace PeMain.UI
{
	partial class AboutForm
	{
		void Initialize()
		{
			PointingUtility.AttachmentDefaultButton(this);
			
			var iconSize = IconScale.Big.ToSize();
			using(var icon = new Icon(global::PeMain.Properties.Images.App, iconSize)) {
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
				Environment.Is64BitProcess ? "64": "32"
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
	}
}
