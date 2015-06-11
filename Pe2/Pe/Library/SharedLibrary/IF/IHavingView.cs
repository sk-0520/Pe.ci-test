namespace ContentTypeTextNet.Library.SharedLibrary.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;

	public interface IHavingView<out TView>
		where TView: UIElement
	{
		TView View { get; }
		/// <summary>
		/// Viewが存在するか。
		/// <para>View != null じゃなくて本メソッドでView存在確認を行う。</para>
		/// <para>テストの際にView が存在しない可能性がある。</para>
		/// </summary>
		bool HasView { get; }
	}
}
