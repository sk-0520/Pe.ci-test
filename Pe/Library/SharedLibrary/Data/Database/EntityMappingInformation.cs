/**
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
namespace ContentTypeTextNet.Library.SharedLibrary.Data.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using ContentTypeTextNet.Library.SharedLibrary.Attribute;

    /// <summary>
    /// 物理名・プロパティ紐付。
    /// 
    /// TargetNameAttributeに紐付く物理名とプロパティ情報。
    /// </summary>
    public sealed class EntityMappingInformation
    {
        public EntityMappingInformation(EntityMappingAttribute attribute, PropertyInfo propertyInfo)
        {
            EntityMappingAttribute = attribute;
            PropertyInfo = propertyInfo;
        }

        #region property

        public EntityMappingAttribute EntityMappingAttribute { get; private set; }
        /// <summary>
        /// TargetNameAttributeで紐付くプロパティ。
        /// </summary>
        public PropertyInfo PropertyInfo { get; private set; }

        #endregion
    }
}
