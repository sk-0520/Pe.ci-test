using System;

namespace ContentTypeTextNet.Pe.Main.Models
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    internal class SettingChangedTargetAttribute: Attribute
    { }
}
