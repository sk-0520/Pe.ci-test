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
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.IF;

namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
    /// <summary>
    /// アイコンのパスを保持。
    /// </summary>
    [DataContract, Serializable]
    public sealed class IconItemModel: IconPathModel, IItemModel, IDeepClone
    {
        public IconItemModel()
            : base()
        {
            IsDisposed = false;
        }

        ~IconItemModel()
        {
            Dispose();
        }

        #region IItemModel

        [field: NonSerialized]
        public event EventHandler Disposing;

        [IgnoreDataMember, XmlIgnore]
        public bool IsDisposed { get; set; }

        public void Dispose()
        {
            // IItemModelのIFに合わせるためだけの実装

            if(IsDisposed) {
                return;
            }

            if(Disposing != null) {
                Disposing(this, EventArgs.Empty);
            }

            IsDisposed = true;
            GC.SuppressFinalize(this);
        }

        #endregion

        #region IDeepClone

        //public void DeepCloneTo(IDeepClone target)
        //{
        //    var obj = (IconItemModel)target;

        //    obj.Path = Path;
        //    obj.Index = Index;
        //}

        public override IDeepClone DeepClone()
        {
            //var result = new IconItemModel();

            //DeepCloneTo(result);

            //return result;
            return DeepCloneUtility.Copy(this);
        }

        #endregion
    }
}
