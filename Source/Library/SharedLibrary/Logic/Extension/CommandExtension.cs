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
using System.Windows.Input;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Extension
{
    public static class CommandExtension
    {
        /// <summary>
        /// コマンドを実行する。
        /// <para>コマンド実行が可能な場合に実行する。</para>
        /// </summary>
        /// <param name="command">コマンド。</param>
        /// <param name="parameter">パラメータ。</param>
        /// <returns>コマンド実行時に真、実行しなかった場合は偽を返す。</returns>
        public static bool TryExecute(this ICommand command, object parameter)
        {
            if(command.CanExecute(parameter)) {
                command.Execute(parameter);
                return true;
            }

            return false;
        }
    }
}
