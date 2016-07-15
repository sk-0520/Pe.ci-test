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
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
    /// <summary>
    /// 環境変数設定データ。
    /// </summary>
    [DataContract, Serializable]
    public class EnvironmentVariablesItemModel: ItemModelBase, IDeepClone
    {
        public EnvironmentVariablesItemModel()
            : base()
        { }

        #region property

        /// <summary>
        /// 環境変数を変更するか。
        /// </summary>
        [DataMember, IsDeepClone]
        public bool Edit { get; set; }

        /// <summary>
        /// 追加・変更対象。
        /// </summary>
        [DataMember, IsDeepClone]
        public EnvironmentVariableUpdateItemCollectionModel Update { get; set; } = new EnvironmentVariableUpdateItemCollectionModel();

        /// <summary>
        /// 削除対象。
        /// </summary>
        [DataMember, IsDeepClone]
        public CollectionModel<string> Remove { get; set; } = new CollectionModel<string>();

        #endregion

        #region IDeepClone

        //public void DeepCloneTo(IDeepClone target)
        //{
        //    var obj = (EnvironmentVariablesItemModel)target;

        //    obj.Edit = Edit;
        //    obj.Update.InitializeRange(Update.Select(u => (EnvironmentVariableUpdateItemModel)u.DeepClone()));
        //    obj.Remove.InitializeRange(Remove);
        //}

        public IDeepClone DeepClone()
        {
            //var result = new EnvironmentVariablesItemModel();

            //DeepCloneTo(result);

            //return result;
            var result = DeepCloneUtility.Copy(this);

            result.Update = new EnvironmentVariableUpdateItemCollectionModel();
            result.Update.InitializeRange(Update.Select(u => u.DeepClone()).Cast<EnvironmentVariableUpdateItemModel>());

            result.Remove = new CollectionModel<string>(Remove);

            return result;
        }

        #endregion
    }
}
