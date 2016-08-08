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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.PeMain.Data.Temporary
{
    public class UpdateInformation
    {
        IEnumerable<string> _log;

        public UpdateInformation(IEnumerable<string> log)
        {
            this._log = log;
        }

        public string Version { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsRcVersion { get; set; }
        public bool IsError { get; set; }
        public int ErrorCode { get; set; }

        public string Log
        {
            get
            {
                return string.Join(Environment.NewLine, this._log);
            }
        }
    }
}
