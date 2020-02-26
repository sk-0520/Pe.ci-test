/*
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
    /// <summary>
    /// <see cref="ISupportInitialize"/>の初期化から初期化終了までを using で実施できるようにする。
    /// </summary>
    internal class Initializer: DisposeFinalizeBase
    {
        public Initializer(ISupportInitialize target)
        {
            Target = target;
        }

        #region property

        public ISupportInitialize Target { get; private set; }

        #endregion

        #region function

        /// <summary>
        /// 初期化用処理を簡略化。
        /// <para>多分こいつしか使わない。</para>
        /// </summary>
        /// <example>
        /// using(Initializer.BeginInitialize(obj)) {
        ///     obj.Property = xxx;
        /// }
        /// </example>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Initializer BeginInitialize(ISupportInitialize target)
        {
            var result = new Initializer(target);
            result.Target.BeginInit();

            return result;
        }

        #endregion

        #region DisposeFinalizeBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(Target != null) {
                    Target.EndInit();
                    Target = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
