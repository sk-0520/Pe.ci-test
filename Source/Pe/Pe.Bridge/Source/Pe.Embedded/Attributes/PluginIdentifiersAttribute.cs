using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Embedded.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class PluginIdentifiersAttribute: Attribute
    {
        public PluginIdentifiersAttribute(string pluginName, string pluginId)
        {
            if(string.IsNullOrWhiteSpace(pluginName)) {
                throw new ArgumentException(nameof(pluginName));
            }
            PluginName = pluginName.Trim();
            if(PluginName.Length == 0) {
                throw new ArgumentException(nameof(pluginName));
            }

            if(Guid.TryParse(pluginId, out var guid)) {
                PluginId = guid;
            } else {
                throw new ArgumentException(nameof(pluginId));
            }
        }

        #region property

        public string PluginName { get; }
        public Guid PluginId { get; }

        #endregion
    }
}
