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
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.IF;

namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
    /// <summary>
    /// ノートインデックスのボディ部データ。
    /// </summary>
    [DataContract, Serializable]
    public class NoteBodyItemModel: IndexBodyItemModelBase
    {
        public NoteBodyItemModel()
            : base()
        { }

        #region property

        /// <summary>
        /// テキストデータ。
        /// </summary>
        [DataMember]
        public string Text { get; set; }

        #endregion

        #region IndexBodyItemModelBase

        public override IndexKind IndexKind { get { return IndexKind.Note; } }

        public override void DeepCloneTo(IDeepClone target)
        {
            base.DeepCloneTo(target);

            var obj = (NoteBodyItemModel)target;
            obj.Text = Text;
        }

        public override IDeepClone DeepClone()
        {
            var result = new NoteBodyItemModel();

            DeepCloneTo(result);

            return result;
        }

        #endregion
    }
}
