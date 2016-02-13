/**
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ContentTypeTextNet.Pe.Library.PeData.Converter
{
    public class ByteArrayConverter: JsonConverter
    {
        #region function

        static byte[] ToArray(string text)
        {
            if(string.IsNullOrWhiteSpace(text) || string.Compare(text.Trim(), "null", true) == 0) {
                return null;
            } else {
                return Convert.FromBase64String(text);
            }
        }

        #endregion

        #region JsonConverter

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(byte[]);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if(reader.Value == null) {
                // 0.74.0以下
                var jObject = JObject.Load(reader);
                var property = jObject.Properties().FirstOrDefault(p => p.Name == "$value");
                return ToArray(property.Value.ToString());
            } else {
                // 0.75.0以上
                var text = reader.Value as string;
                return ToArray(text);
            }
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var array = value as byte[];
            string output = null;
            if(array == null) {
                output = "null";
            } else {
                output = Convert.ToBase64String(array, Base64FormattingOptions.None);
            }
            serializer.Serialize(writer, output);
        }

        #endregion
    }
}
