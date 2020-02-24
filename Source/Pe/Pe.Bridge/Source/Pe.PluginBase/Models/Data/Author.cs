using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.PluginBase.Models.Data
{
    public class Author : IAuthor
    {
        public Author(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public List<IContact> Contacts { get; } = new List<IContact>();
        IReadOnlyCollection<IContact> IAuthor.Contacts => Contacts;

        #region IAuthor
        #endregion
    }
}
