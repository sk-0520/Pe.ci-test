using System;

namespace ContentTypeTextNet.Pe.Embedded.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class PluginAuthorsAttribute: Attribute
    {
        public PluginAuthorsAttribute(string name, string license, string website = "", string projectsite = "", string email = "")
        {
            if(string.IsNullOrWhiteSpace(name)) {
                throw new ArgumentException(nameof(name));
            }
            Name = name;

            if(string.IsNullOrWhiteSpace(license)) {
                throw new ArgumentException(nameof(license));
            }
            License = license;

            Website = website;
            Projectsite = projectsite;
            Email = email;
        }

        #region property

        public string Name { get; }
        public string License { get; }

        public string Website { get; }
        public string Projectsite { get; }
        public string Email { get; }

        #endregion
    }
}
