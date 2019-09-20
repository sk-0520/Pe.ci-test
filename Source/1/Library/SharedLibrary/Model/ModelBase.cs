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
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
    /// <summary>
    /// モデルの基盤。
    /// <para>データ保持を生きがいにする。</para>
    /// </summary>
    [Serializable]
    public abstract class ModelBase: IModel
    {
        #region variable
        #endregion

        #region IModel

        [IgnoreDataMember, XmlIgnore]
        public virtual string DisplayText
        {
            get { return GetType().FullName; }
        }

        public virtual void Correction()
        { }

        #endregion

        #region Object

        public override string ToString()
        {
            return ReflectionUtility.GetObjectString(this);
        }

        #endregion

    }
}
