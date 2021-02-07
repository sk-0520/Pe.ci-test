using System;
using System.Collections.Generic;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public class VersionConverter
    {
        #region function

        public string ConvertDisplayVersion(Version version, string separator)
        {
            return $"{version.Major}{separator}{version.Minor:00}{separator}{version.Build:000}";
        }

        public string ConvertNormalVersion(Version version)
        {
            return ConvertDisplayVersion(version, ".");
        }

        public string ConvertFileName(string head, Version version, string tail, string extension)
        {
            if(string.IsNullOrWhiteSpace(head)) {
                throw new ArgumentException(nameof(head));
            }
            var values = new List<string>() {
                head,
                "_",
                ConvertDisplayVersion(version, "-")
            };
            if(!string.IsNullOrWhiteSpace(tail)) {
                values.Add("_");
                values.Add(tail);
            }
            if(!string.IsNullOrEmpty(extension)) {
                values.Add(".");
                values.Add(extension);
            }

            return string.Join(string.Empty, values);
        }

        #endregion
    }
}
