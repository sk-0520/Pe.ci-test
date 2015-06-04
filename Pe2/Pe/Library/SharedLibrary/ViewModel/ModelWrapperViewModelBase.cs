namespace ContentTypeTextNet.Library.SharedLibrary.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Model;

	public class ModelWrapperViewModelBase<TModel> : ViewModelBase
		where TModel: ModelBase
	{
		public ModelWrapperViewModelBase(TModel model)
		{
			Model = model;
		}

		#region property

		/// <summary>
		/// モデル。
		/// </summary>
		protected TModel Model { get; private set; }

		#endregion
	}
}
