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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.PInvoke.Windows;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
    /// <summary>
    /// <para>http://stackoverflow.com/questions/248603/natural-sort-order-in-c-sharp</para>
    /// </summary>
    public class NaturalStringComparer: IComparer<string>
    {
        public int Compare(string a, string b)
        {
            return NativeMethods.StrCmpLogicalW(a, b);
        }
    }

    /// <summary>
    /// <para>http://stackoverflow.com/questions/248603/natural-sort-order-in-c-sharp</para>
    /// </summary>
    public sealed class NaturalFileInfoNameComparer: IComparer<FileInfo>
    {
        public int Compare(FileInfo a, FileInfo b)
        {
            return NativeMethods.StrCmpLogicalW(a.Name, b.Name);
        }
    }
}
