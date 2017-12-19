/*
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ContentTypeTextNet.Library.SharedLibrary.View.Window
{
    /// <summary>
    /// 読み込みを Loaded イベントに依存しない順序ありで逐次実行する Window。
    /// <para>本クラスを継承する場合、Loaded イベントではなく OnLoaded を使用すること。</para>
    /// <para>順序については自身の読み込み処理を考慮して base.OnLoaded を呼び出すこと。 </para>
    /// </summary>
    public abstract class OnLoadedWindowBase: System.Windows.Window
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
