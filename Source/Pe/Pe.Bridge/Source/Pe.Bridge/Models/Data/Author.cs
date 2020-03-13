using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Bridge.Models.Data
{
    /// <summary>
    /// 著者。
    /// </summary>
    public interface IAuthor
    {
        #region property

        /// <summary>
        /// 作者名。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 連絡先一覧。
        /// </summary>
        IReadOnlyCollection<IContact> Contacts { get; }

        #endregion
    }

    /// <summary>
    /// <inheritdoc cref="IAuthor" />
    /// </summary>
    public class Author : IAuthor
    {
        public Author(string name)
        {
            Name = name;
        }


        #region IAuthor

        /// <summary>
        /// <inheritdoc cref="IAuthor.Name"/>
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// <inheritdoc cref="IAuthor.Contacts"/>
        /// </summary>
        public List<IContact> Contacts { get; } = new List<IContact>();
        /// <summary>
        /// <see cref="Contacts"/>
        /// </summary>
        IReadOnlyCollection<IContact> IAuthor.Contacts => Contacts;

        #endregion
    }

}
