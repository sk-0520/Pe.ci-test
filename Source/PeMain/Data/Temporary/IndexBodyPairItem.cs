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
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.Library.PeData.Item;

namespace ContentTypeTextNet.Pe.PeMain.Data.Temporary
{
    public class IndexBodyPairItem<TIndexBody>: DisposeFinalizeBase
        where TIndexBody : IndexBodyItemModelBase
    {
        public IndexBodyPairItem(Guid id, TIndexBody body)
        {
            Id = id;
            Body = body;
        }

        #region property

        public Guid Id { get; private set; }
        public TIndexBody Body { get; private set; }

        #endregion

        #region DisposeFinalizeBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Body.Dispose();
                Body = null;
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
