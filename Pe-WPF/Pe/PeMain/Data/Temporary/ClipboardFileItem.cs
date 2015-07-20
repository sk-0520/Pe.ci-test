namespace ContentTypeTextNet.Pe.PeMain.Data.Temporary
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Media;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public class ClipboardFileItem
	{
		public string Name { get; set; }
		public string Path { get; set; }
		public ImageSource Image { 
			get
			{
				var iconPath = new IconPathModel() {
					Path = this.Path,
					Index = 0,
				};
				if(FileUtility.Exists(iconPath.Path)) {
					return AppUtility.LoadIconDefault(iconPath, IconScale.Small);
				} else {
					return AppResource.GetNotFoundIcon(IconScale.Small);
				}
			}
		} 
	}
}
