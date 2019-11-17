using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Bridge.Models.Data
{
    public interface IAuthor
    {
        #region property

        /// <summary>
        /// 作者名。
        /// </summary>
        string Name { get; }

        IReadOnlyCollection<IContact> Contacts { get; }

        #endregion
    }
}
