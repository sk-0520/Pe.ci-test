namespace ContentTypeTextNet.Library.SharedLibrary.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Model;

	/// <summary>
	/// 単一モデルを取り込むVM。
	/// </summary>
	/// <typeparam name="TModel">モデル。</typeparam>
	public abstract class SingleModelWrapperViewModelBase<TModel> : ViewModelBase
		where TModel: IModel
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="model">取り込むモデル</param>
		public SingleModelWrapperViewModelBase(TModel model)
			: base()
		{
			Model = model;
			InitializeModel();
		}

		#region property

		/// <summary>
		/// モデル。
		/// </summary>
		public TModel Model { get; private set; }

		#endregion

		#region function

		protected virtual void InitializeModel()
		{ }

		/// <summary>
		/// モデル変更用ヘルパ。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">変更データ。</param>
		/// <param name="memberName">対象オブジェクトのメンバ名。</param>
		/// <param name="propertyName"></param>
		/// <returns>変更があった場合は真を返す。</returns>
		protected bool SetModelValue<T>(T value, [CallerMemberName] string memberName = "", [CallerMemberName] string propertyName = "")
		{
			return SetPropertyValue(Model, value, memberName, propertyName);
		}

		#endregion

		#region ViewModelBase

		protected override void Dispose(bool disposing)
		{
			if(!IsDisposed) {
				Model = default(TModel);
			}

			base.Dispose(disposing);
		}

		#endregion
	}
}
