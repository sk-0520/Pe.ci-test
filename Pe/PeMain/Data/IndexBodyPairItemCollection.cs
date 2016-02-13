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
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
using ContentTypeTextNet.Pe.PeMain.Logic;
using Library.PeData.Define;

namespace ContentTypeTextNet.Pe.PeMain.Data
{
    public class IndexBodyPairItemCollection<TIndexBody>: FixedSizeCollectionModel<IndexBodyPairItem<TIndexBody>>
        where TIndexBody : IndexBodyItemModelBase
    {
        public IndexBodyPairItemCollection(int limitSize, IndexKind indexKind, VariableConstants variableConstants)
            : base(limitSize)
        {
            IndexKind = indexKind;
            Archive.OpenIfExists(indexKind, variableConstants);
        }

        #region property

        public IndexKind IndexKind { get; private set; }
        public IndexBodyArchive Archive { get; } = new IndexBodyArchive();

        #endregion

        #region function

        /// <summary>
        /// 指定IDのデータを取得。
        /// </summary>
        /// <param name="id"></param>
        /// <returns>なければnull。</returns>
        public TIndexBody GetFromId(Guid id)
        {
            var result = this.FirstOrDefault(pair => pair.Id == id);
            if(result != null) {
                return result.Body;
            }

            return null;
        }

        public int IndexOf(Guid id)
        {
            var result = this
                .Select((item, i) => new { Item = item, Index = i })
                .FirstOrDefault(pair => pair.Item.Id == id)
            ;
            if(result == null) {
                return -1;
            }

            return result.Index;
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Archive.Dispose();
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
