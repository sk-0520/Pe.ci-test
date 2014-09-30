/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/09/07
 * 時刻: 17:53
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using PeUtility;

namespace PeMain.UI
{
	public partial class AboutForm
	{
		void Initialize()
		{
			PointingUtility.AppendEventFormLoad(this);
			
			var iconSize = IconScale.Big.ToSize();
			using(var icon = new Icon(global::PeMain.Properties.Images.Pe, iconSize)) {
				var image = new Bitmap(iconSize.Width, iconSize.Height);
				using(var g = Graphics.FromImage(image)) {
					g.DrawIcon(icon, new Rectangle(Point.Empty, iconSize));
				}
				this.imageIcon.Image = image;
			}
			
			this.labelAppName.Text = Literal.programName;
			this.labelAppVersion.Text = Literal.PeVersion;
			this.labelConfiguration.Text = string.Format(
				"{0}: {1}bit",
				#if DEBUG
				"DEBUG",
				#else
				"RELEASE",
				#endif
				Environment.Is64BitProcess ? "64": "32"
			);
			
			this.linkWeb.Text = Literal.AboutWebPage;
			this.linkMail.Text = Literal.AboutMailAddress;
			this.linkDev.Text = Literal.AboutDevelopPage;
			
			var xml = XElement.Load(Path.Combine(Literal.PeDocumentDirPath, "components.xml"));
			var components = xml
				.Elements()
				.Select(e => new { Name = e.Attribute("name").Value, URI = e.Attribute("uri").Value })
			;
			this.gridComponents_columnName.DataPropertyName = "Name";
			this.gridComponents_columnURI.DataPropertyName = "URI";
			this.gridComponents.AutoGenerateColumns = false;
			this.gridComponents.DataSource = new BindingSource(components, string.Empty);
		}
	}
}
