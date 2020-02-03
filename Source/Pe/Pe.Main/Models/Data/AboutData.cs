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
    public class AboutComponentData:DataBase
    {
        #region property

        #endregion
    }
}
