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
using ContentTypeTextNet.Pe.Library.PeData.Define;

namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
    /// <summary>
    /// テンプレートインデックスのヘッダ部。
    /// </summary>
    public class TemplateIndexItemModel: IndexItemModelBase
    {
        public TemplateIndexItemModel()
            : base()
        { }

        #region property

        /// <summary>
        /// 置換処理を行うか。
        /// </summary>
        [DataMember]
        public TemplateReplaceMode TemplateReplaceMode { get; set; }

        #endregion

        #region IndexItemModelBase

        public override void DeepCloneTo(IDeepClone target)
        {
            base.DeepCloneTo(target);

            var obj = (TemplateIndexItemModel)target;

            obj.TemplateReplaceMode = TemplateReplaceMode;
        }

        public override IDeepClone DeepClone()
        {
            var result = new TemplateIndexItemModel();

            DeepCloneTo(result);

            return result;
        }

        #endregion
    }
}
