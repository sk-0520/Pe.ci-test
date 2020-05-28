using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;

namespace ContentTypeTextNet.Pe.Embedded.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class SupportVersionsAttibute: Attribute
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="minimumVersion"><see cref="IPluginVersions.MinimumSupportVersion"/></param>
        /// <param name="maximumVersion"><see cref="IPluginVersions.MaximumSupportVersion"/></param>
        public SupportVersionsAttibute(string minimumVersion = "0.0.0", string maximumVersion = "0.0.0")
        {
            MinimumVersion = Version.Parse(minimumVersion);
            MaximumVersion = Version.Parse(maximumVersion);
        }


        #region property

        /// <inheritdoc cref="IPluginVersions.MinimumSupportVersion"/>
        public Version MinimumVersion { get; }
        /// <inheritdoc cref="IPluginVersions.MaximumSupportVersion"/>
        public Version MaximumVersion { get; }

        #endregion
    }
}
