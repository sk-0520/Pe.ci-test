namespace ContentTypeTextNet.Library.SharedLibrary.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Model;

	/// <summary>
	/// これといってViewは関係ないと思う。
	/// </summary>
	public class LanguageViewModel: ModelWrapperViewModelBase<LanguageModel>
	{
		public LanguageViewModel(LanguageModel model)
			: base(model)
		{ }

		#region function

		static LanguageItemModel GetItem(IEnumerable<LanguageItemModel> list, string key)
		{
			var result = list.SingleOrDefault(item => item.Name == key);
			return result ?? new LanguageItemModel() {
				Name = key,
				Word = key,
			};
		}

		/// <summary>
		/// キーからテキスト取得。
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public string GetPlainText(string key)
		{
			var item = GetItem(Model.Words, key);
			return item.Word;
		}


		#endregion

		#region indexer
		#endregion
	}
}
