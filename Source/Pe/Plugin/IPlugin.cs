using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Plugin
{
    public enum PluginType
    {
        Normal,
        Systyem,
    }

    public interface IPlugin0
    {
        #region property

        int SupportedVersion { get; }
        PluginType PluginType { get; }

        #endregion
    }

    public interface IPlugin1: IPlugin0
    {
        #region property
        #endregion
    }

}
