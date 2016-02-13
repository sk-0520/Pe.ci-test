/**
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
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Pe.Library.PeData.Item;

namespace ContentTypeTextNet.Pe.Library.PeData.Setting
{
    public abstract class IndexSettingModelBase<TCollectionModel, TItemModel>: SettingModelBase, IDeepClone
        where TCollectionModel : IndexItemCollectionModel<TItemModel>, new()
        where TItemModel : IndexItemModelBase, IDeepClone
    {
        public IndexSettingModelBase()
            : base()
        {
            Items = new TCollectionModel();
        }

        #region property

        [DataMember]
        public TCollectionModel Items { get; set; }

        #endregion

        #region IDeepClone

        public virtual void DeepCloneTo(IDeepClone target)
        {
            var obj = (IndexSettingModelBase<TCollectionModel, TItemModel>)target;

            obj.Items.InitializeRange(Items.Select(i => (TItemModel)i.DeepClone()));
        }

        public abstract IDeepClone DeepClone();

        #endregion
    }
}
