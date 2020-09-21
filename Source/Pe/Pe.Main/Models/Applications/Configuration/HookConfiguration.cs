using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    public class HookConfiguration: ConfigurationBase
    {
        public HookConfiguration(IConfigurationSection section)
            : base(section)
        {
            //Keyboard = section.GetValue<bool>("keyboard");
            //Mouse = section.GetValue<bool>("mouse");
        }

        #region property

        /// <summary>
        /// キーボードを有効にするか。
        /// </summary>
        [Configuration]
        public bool Keyboard { get; }
        /// <summary>
        /// マウスフックを有効にするか。
        /// </summary>
        [Configuration]
        public bool Mouse { get; }

        #endregion
    }
}
