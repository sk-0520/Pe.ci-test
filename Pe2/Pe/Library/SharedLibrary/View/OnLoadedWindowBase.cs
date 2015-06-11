namespace ContentTypeTextNet.Library.SharedLibrary.View
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;

	/// <summary>
	/// 読み込みを Loaded イベントに依存しない順序ありで逐次実行する Window。
	/// <para>本クラスを継承する場合、Loaded イベントではなく OnLoaded を使用すること。</para>
	/// <para>順序については自身の読み込み処理を考慮して base.OnLoaded を呼び出すこと。 </para>
	/// </summary>
	public abstract class OnLoadedWindowBase: Window
	{
		public OnLoadedWindowBase() :
			base()
		{
			Loaded += OnLoadedWindow_Loaded;
		}

		#region function

		protected virtual void OnLoaded(object sender, RoutedEventArgs e)
		{ }

		#endregion

		void OnLoadedWindow_Loaded(object sender, RoutedEventArgs e)
		{
			Loaded -= OnLoadedWindow_Loaded;
			OnLoaded(sender, e);
		}
	}
}
