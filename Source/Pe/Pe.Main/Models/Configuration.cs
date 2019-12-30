using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models
{
    public abstract class ConfigurationBase
    {
        public ConfigurationBase(IConfigurationSection section)
        { }

        #region property

        protected static string GetString(IConfigurationSection section, string key) => section.GetValue<string>(key);
        protected static int GetInteger(IConfigurationSection section, string key) => section.GetValue<int>(key);

        #endregion
    }

    public class GeneralConfiguration : ConfigurationBase
    {
        public GeneralConfiguration(IConfigurationSection section) : base(section)
        {
            ProjectName = section.GetValue<string>("project-name");
            MutexName = section.GetValue<string>("mutex-name");
            LoggingConfigFileName = section.GetValue<string>("log-conf-file-name");
        }

        #region IGeneralConfiguration

        public string ProjectName { get; }
        public string MutexName { get; }
        public string LoggingConfigFileName { get; }

        #endregion
    }


    public class Configuration
    {
        public Configuration(IConfigurationRoot configurationRoot)
        {
            General = new GeneralConfiguration(configurationRoot.GetSection("general"));
        }

        #region property

        public GeneralConfiguration General { get; }

        #endregion
    }
}
