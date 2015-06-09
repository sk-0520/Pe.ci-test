namespace ContentTypeTextNet.Library.SharedLibrary.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Model;

	/// <summary>
	/// 単一モデルを取り込むVM。
	/// </summary>
	/// <typeparam name="TModel">モデル。</typeparam>
	public abstract class SingleModelWrapperViewModelBase<TModel> : ViewModelBase
		where TModel: ModelBase
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="model">取り込むモデル</param>
		public SingleModelWrapperViewModelBase(TModel model)
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
