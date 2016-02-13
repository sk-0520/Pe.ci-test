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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.Item;

namespace ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings
{
    [Serializable, DataContract]
    public class StreamSettingModel: SettingModelBase, IDeepClone
    {
        public StreamSettingModel()
            : base()
        {
            OutputColor = new ColorPairItemModel();
            ErrorColor = new ColorPairItemModel();

            Font = new FontModel();
        }

        #region property

        [DataMember, IsDeepClone]
        public ColorPairItemModel OutputColor { get; set; }
        [DataMember, IsDeepClone]
        public ColorPairItemModel ErrorColor { get; set; }

        [DataMember, IsDeepClone]
        public FontModel Font { get; set; }

        #endregion

        #region IDeepClone

        //public void DeepCloneTo(IDeepClone target)
        //{
        //    var obj = (StreamSettingModel)target;

        //    OutputColor.DeepCloneTo(obj.OutputColor);
        //    ErrorColor.DeepCloneTo(obj.ErrorColor);

        //    Font.DeepCloneTo(obj.Font);
        //}

        public IDeepClone DeepClone()
        {
            //var result = new StreamSettingModel();

            //DeepCloneTo(result);

            //return result;
            return DeepCloneUtility.Copy(this);
        }

        #endregion
    }
}
