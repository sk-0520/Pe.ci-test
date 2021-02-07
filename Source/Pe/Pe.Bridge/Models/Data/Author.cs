using System.Collections.Generic;

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

    /// <inheritdoc cref="IAuthor" />
    public class Author: IAuthor
    {
        public Author(string name)
        {
            Name = name;
        }


        #region IAuthor

        /// <inheritdoc cref="IAuthor.Name"/>
        public string Name { get; }

        /// <inheritdoc cref="IAuthor.Contacts"/>
        public List<IContact> Contacts { get; } = new List<IContact>();
        /// <see cref="Contacts"/>
        IReadOnlyCollection<IContact> IAuthor.Contacts => Contacts;

        #endregion
    }

}
