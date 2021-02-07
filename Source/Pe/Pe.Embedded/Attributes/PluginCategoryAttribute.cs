using System;
using System.Collections.Generic;

namespace ContentTypeTextNet.Pe.Embedded.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class PluginCategoryAttribute: Attribute
    {
        public PluginCategoryAttribute(string primary)
        {
            Primary = primary;
            Secondaries = Array.Empty<string>();
        }

        public PluginCategoryAttribute(string primary, string secondary, params string[] secondaries)
        {
            Primary = primary;
            var list = new List<string>(1 + secondaries.Length);
            list.Add(secondary);
            list.AddRange(secondaries);
            Secondaries = list;
        }

        #region property

        public string Primary { get; }
        public IReadOnlyList<string> Secondaries { get; }

        #endregion
    }
}
