using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Main.Models
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    internal class SettingChangedTargetAttribute: Attribute
    { }
}
