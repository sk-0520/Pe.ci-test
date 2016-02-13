/*
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Data;
using ContentTypeTextNet.Library.SharedLibrary.Logic;

namespace ContentTypeTextNet.Pe.PeMain.Logic
{
    public class AppInformationCollection: InformationCollection
    {
        public override FileVersionInfo GetVersionInfo
        {
            get { return FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location); }
        }

        public override InformationGroup GetApplication()
        {
            var result = base.GetApplication();

            result.Items.Add("BuildType", Constants.BuildType);

            return result;
        }
    }
}

