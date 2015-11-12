/**
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
namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;
    using Attribute;
    using ContentTypeTextNet.Library.SharedLibrary.IF;
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

    [Serializable, DataContract]
    public class FontModel: ModelBase, IDeepClone
    {
        #region property

        [DataMember, IsDeepClone]
        public string Family { get; set; }
        [DataMember, IsDeepClone]
        public double Size { get; set; }
        [DataMember, IsDeepClone]
        public bool Bold { get; set; }
        [DataMember, IsDeepClone]
        public bool Italic { get; set; }

        #endregion

        #region IDeepClone

        //public void DeepCloneTo(IDeepClone target)
        //{
        //    var obj = (FontModel)target;

        //    obj.Family = Family;
        //    obj.Size = Size;
        //    obj.Bold = Bold;
        //    obj.Italic = Italic;
        //}

        public IDeepClone DeepClone()
        {
            //var result = new FontModel();

            //DeepCloneTo(result);

            //return result;
            return DeepCloneUtility.Copy(this);
        }

        #endregion
    }
}
