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

namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
    /// <summary>
    /// 標準入出力関連。
    /// </summary>
    [Serializable]
    public class LauncherStdStreamItemModel: ItemModelBase, IDeepClone
    {
        public LauncherStdStreamItemModel()
            : base()
        { }

        #region property

        /// <summary>
        /// 標準出力(とエラー)を取得するか。
        /// </summary>
        [DataMember, IsDeepClone]
        public bool OutputWatch { get; set; }
        /// <summary>
        /// 標準入力へ入力するか。
        /// </summary>
        [DataMember, IsDeepClone]
        public bool InputUsing { get; set; }

        #endregion

        #region IDeepClone

        //public virtual void DeepCloneTo(IDeepClone target)
        //{
        //    var obj = (LauncherStdStreamItemModel)target;

        //    obj.OutputWatch = OutputWatch;
        //    obj.InputUsing = InputUsing;
        //}

        public IDeepClone DeepClone()
        {
            //var result = new LauncherStdStreamItemModel();

            //DeepCloneTo(result);

            //return result;
            return DeepCloneUtility.Copy(this);
        }

        #endregion
    }
}
