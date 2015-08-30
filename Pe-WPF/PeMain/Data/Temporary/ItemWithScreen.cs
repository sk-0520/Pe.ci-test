namespace ContentTypeTextNet.Pe.PeMain.Data.Temporary
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Model;

	public class ItemWithScreen<TModel>
		where TModel: IModel
	{
		public ItemWithScreen(TModel model, ScreenModel screen)
		{
			Model = model;
			Screen = screen;
		}

		public TModel Model { get; private set; }
		public ScreenModel Screen { get; private set; }
	}
}
