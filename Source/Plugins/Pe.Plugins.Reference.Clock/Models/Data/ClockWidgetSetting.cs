using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace ContentTypeTextNet.Pe.Plugins.Reference.Clock.Models.Data
{
    public enum ClockWidgetKind
    {

        SimpleAnalog,
        JaggyAnalog,
    }

    public class ClockWidgetSetting
    {
        #region property

        public ClockWidgetKind ClockWidgetKind { get; set; }

        [XmlIgnore]
        [IgnoreDataMember]
        [JsonIgnore]
        public TimeZoneInfo TimeZone { get; set; } = TimeZoneInfo.Local;

        public string TimeZoneId
        {
            get
            {
                return TimeZone.ToSerializedString();
            }
            set
            {
                TimeZone = TimeZoneInfo.FromSerializedString(value);
            }
        }

        #endregion
    }
}
