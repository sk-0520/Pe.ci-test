using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ContentTypeTextNet.Pe.PeMain.Converter
{
    public class JsonVersionConverter : JsonConverter
    {
        #region JsonConverter

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Version);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var map = serializer.Deserialize<Dictionary<string, int>>(reader);
            if(map == null) {
                return null;
            }
            return new Version(map["Major"], map["Minor"], map["Build"], map["Revision"]);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
