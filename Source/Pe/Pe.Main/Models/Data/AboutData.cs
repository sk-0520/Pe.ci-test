using System;
using System.Runtime.Serialization;
using ContentTypeTextNet.Pe.Core.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    [Serializable, DataContract]
    public class AboutComponentsData: DataBase
    {
        #region property
        [DataMember(Name = "library")]
        public AboutComponentData[] Library { get; set; } = new AboutComponentData[0];

        [DataMember(Name = "resource")]
        public AboutComponentData[] Resource { get; set; } = new AboutComponentData[0];
        #endregion
    }

    [Serializable, DataContract]
    public class AboutComponentData: DataBase
    {
        #region property

        [DataMember(Name = "name")]
        public string Name { get; set; } = string.Empty;

        [DataMember(Name = "uri")]
        public string Uri { get; set; } = string.Empty;

        [DataMember(Name = "license")]
        public AboutLicenseData License { get; set; } = new AboutLicenseData();

        [DataMember(Name = "comment")]
        public string Comment { get; set; } = string.Empty;
        #endregion
    }

    [Serializable, DataContract]
    public class AboutLicenseData: DataBase
    {
        #region property

        [DataMember(Name = "name")]
        public string Name { get; set; } = string.Empty;

        [DataMember(Name = "uri")]
        public string Uri { get; set; } = string.Empty;

        #endregion
    }

    /// <summary>
    /// コンポーネント種別。
    /// </summary>
    public enum AboutComponentKind
    {
        /// <summary>
        /// Pe。
        /// </summary>
        [EnumResource]
        Application,
        /// <summary>
        /// ライブラリ。
        /// </summary>
        [EnumResource]
        Library,
        /// <summary>
        /// ソフトウェア。
        /// </summary>
        [EnumResource]
        Software,
        /// <summary>
        /// リソース。
        /// </summary>
        [EnumResource]
        Resource,
    }

    /// <summary>
    /// コンポーネントアイテム。
    /// </summary>
    public class AboutComponentItem
    {
        public AboutComponentItem(AboutComponentKind kind, AboutComponentData data, int sort)
        {
            Kind = kind;
            Data = data;
            Sort = sort;
        }

        #region property

        /// <inheritdoc cref="AboutComponentKind"/>
        public AboutComponentKind Kind { get; }

        /// <inheritdoc cref="AboutComponentData"/>
        public AboutComponentData Data { get; }

        /// <summary>
        /// 順序。
        /// </summary>
        public int Sort { get; }

        #endregion
    }
}
