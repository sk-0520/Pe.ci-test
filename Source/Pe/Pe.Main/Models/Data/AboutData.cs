using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    [Serializable, DataContract]
    public class AboutComponentsData : DataBase
    {
        #region property
        [DataMember(Name = "library")]
        public AboutComponentData[] Library { get; set; } = new AboutComponentData[0];

        [DataMember(Name = "resource")]
        public AboutComponentData[] Resource { get; set; } = new AboutComponentData[0];
        #endregion
    }

    [Serializable, DataContract]
    public class AboutComponentData : DataBase
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
    public class AboutLicenseData : DataBase
    {
        #region property

        [DataMember(Name = "name")]
        public string Name { get; set; } = string.Empty;

        [DataMember(Name = "uri")]
        public string Uri { get; set; } = string.Empty;

        #endregion
    }

    public enum AboutComponentKind
    {
        [EnumResource]
        Application,
        [EnumResource]
        Library,
        [EnumResource]
        Software,
        [EnumResource]
        Resource,
    }

    public class AboutComponentItem
    {
        public AboutComponentItem(AboutComponentKind kind, AboutComponentData data, int sort)
        {
            Kind = kind;
            Data = data;
            Sort = sort;
        }

        #region property

        public AboutComponentKind Kind { get; }

        public AboutComponentData Data { get; }

        public int Sort { get; }
        #endregion
    }
}
