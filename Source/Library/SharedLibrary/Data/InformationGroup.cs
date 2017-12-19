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

namespace ContentTypeTextNet.Library.SharedLibrary.Data
{
    public class InformationGroup
    {
        public InformationGroup(string title)
        {
            Title = title;
            Items = new Dictionary<string, object>();
        }

        public string Title { get; private set; }
        public Dictionary<string, object> Items { get; private set; }

        public void WriteInformation(TextWriter writer)
        {
            writer.WriteLine("{0} =================", Title);
            foreach(var pair in Items) {
                writer.WriteLine("{0}: {1}", pair.Key, pair.Value);
            }
        }

        public override string ToString()
        {
            using(var writer = new StringWriter()) {
                WriteInformation(writer);
                return writer.ToString();
            }
        }
    }
}
