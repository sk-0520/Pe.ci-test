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
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
    /// <summary>
    /// 環境変数設定データ。
    /// </summary>
    [Serializable]
    public class EnvironmentVariablesItemModel: ItemModelBase, IDeepClone
    {
        public EnvironmentVariablesItemModel()
            : base()
        {
            Update = new EnvironmentVariableUpdateItemCollectionModel();
            Remove = new CollectionModel<string>();
        }

        /// <summary>
        /// 環境変数を変更するか。
        /// </summary>
        [DataMember]
        public bool Edit { get; set; }

        /// <summary>
        /// 追加・変更対象。
        /// </summary>
        [DataMember]
        public EnvironmentVariableUpdateItemCollectionModel Update { get; set; }

        /// <summary>
        /// 削除対象。
        /// </summary>
        [DataMember]
        public CollectionModel<string> Remove { get; set; }

        #region IDeepClone

        public void DeepCloneTo(IDeepClone target)
        {
            var obj = (EnvironmentVariablesItemModel)target;

            obj.Edit = Edit;
            obj.Update.InitializeRange(Update.Select(u => (EnvironmentVariableUpdateItemModel)u.DeepClone()));
            obj.Remove.InitializeRange(Remove);
        }

        public IDeepClone DeepClone()
        {
            var result = new EnvironmentVariablesItemModel();

            DeepCloneTo(result);

            return result;
        }

        #endregion
    }
}
