namespace ContentTypeTextNet.Pe.PeMain.Data.Temporary
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	public class LauncherExecuteItem : ItemWithScreen<LauncherItemModel>
	{
		public LauncherExecuteItem(LauncherItemModel model, ScreenModel screen, IEnumerable<string> options)
			:base(model, screen)
		{
			Options = options;
		}

		#region property

		public IEnumerable<string> Options { get; private set; }

		#endregion
	}
}
