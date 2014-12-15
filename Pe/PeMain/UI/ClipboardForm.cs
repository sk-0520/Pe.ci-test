using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.Library.Utility;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	public partial class ClipboardForm: Form, ISetCommonData
	{
		#region Define

		const string imageText = "image_text";
		const string imageRtf = "image_rtf";
		const string imageImage = "image_image";
		const string imageFile = "image_file";

		#endregion

		#region Variable
		#endregion

		public ClipboardForm()
		{
			InitializeComponent();

			Initialize();
		}

		#region Property

		CommonData CommonData { get; set; }

		#endregion

		#region Initialize
		
		void InitializeUI()
		{
			this.tabPreview_pageText.ImageKey = imageText;
			this.tabPreview_pageRichTextFormat.ImageKey = imageRtf;
			this.tabPreview_pageImage.ImageKey = imageImage;
			this.tabPreview_pageFile.ImageKey = imageFile;
		}

		void Initialize()
		{
			this.imageTab.Images.Add(imageText, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ClipboardText);
			this.imageTab.Images.Add(imageRtf, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ClipboardRichTextFormat);
			this.imageTab.Images.Add(imageImage, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ClipboardImage);
			this.imageTab.Images.Add(imageFile, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ClipboardFile);

			InitializeUI();
		}

		#endregion

		#region Language

		private void ApplyLanguage()
		{
		}
		
		#endregion

		#region Function

		public void SetCommonData(CommonData commonData)
		{
			CommonData = commonData;

			ApplySetting();
		}

		void ApplySetting()
		{
			ApplyLanguage();
		}


		#endregion
	}
}
