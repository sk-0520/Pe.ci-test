/*
This file is part of PInvoke.

PInvoke is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

PInvoke is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with PInvoke.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Library.PInvoke.Windows
{
	[ComImport, Guid("0000010c-0000-0000-c000-000000000046"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IPersist
	{
		[PreserveSig]
		void GetClassID(out Guid pClassID);
	}

	[ComImport, Guid("0000010b-0000-0000-C000-000000000046"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IPersistFile: IPersist
	{
		new void GetClassID(out Guid pClassID);
		[PreserveSig]
		int IsDirty();

		[PreserveSig]
		void Load(
			[In, MarshalAs(UnmanagedType.LPWStr)] string pszFileName, 
			uint dwMode
		);

		[PreserveSig]
		void Save(
			[In, MarshalAs(UnmanagedType.LPWStr)] string pszFileName,
			[In, MarshalAs(UnmanagedType.Bool)] bool fRemember
		);

		[PreserveSig]
		void SaveCompleted([In, MarshalAs(UnmanagedType.LPWStr)] string pszFileName);

		[PreserveSig]
		void GetCurFile([In, MarshalAs(UnmanagedType.LPWStr)] string ppszFileName);
	}

}
