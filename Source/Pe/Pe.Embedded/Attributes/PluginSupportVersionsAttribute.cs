using System;
using System.Collections.Generic;
using ContentTypeTextNet.Pe.Bridge.Plugin;

namespace ContentTypeTextNet.Pe.Embedded.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class PluginSupportVersionsAttribute: Attribute
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="minimumVersion"><see cref="IPluginVersions.MinimumSupportVersion"/></param>
        /// <param name="maximumVersion"><see cref="IPluginVersions.MaximumSupportVersion"/></param>
        /// <param name="checkUrls"></param>
        public PluginSupportVersionsAttribute(string minimumVersion = "0.0.0", string maximumVersion = "0.0.0", params string[] checkUrls)
        {
            MinimumVersion = Version.Parse(minimumVersion);
            MaximumVersion = Version.Parse(maximumVersion);
            CheckUrls = checkUrls;
        }


        #region property

        /// <inheritdoc cref="IPluginVersions.MinimumSupportVersion"/>
        public Version MinimumVersion { get; }
        /// <inheritdoc cref="IPluginVersions.MaximumSupportVersion"/>
        public Version MaximumVersion { get; }
        /// <inheritdoc cref="IPluginVersions.CheckUrls"/>
        public IReadOnlyList<string> CheckUrls { get; }

        #endregion
    }
}
