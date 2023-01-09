using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Setupper
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DatabaseSetupVersionAttribute: Attribute
    {
        public DatabaseSetupVersionAttribute(Version version)
        {
            Version = version;
        }

        public DatabaseSetupVersionAttribute(int major, int minor, int builds)
            : this(new Version(major, minor, builds))
        { }

        #region property

        public Version Version { get; }

        #endregion
    }
}
