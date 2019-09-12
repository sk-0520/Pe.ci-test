using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Model.Logic
{
    public enum BuildType
    {
        Debug,
        Beta,
        Release,
    }

    public interface IBuildStatus
    {
        #region property

        BuildType BuildType { get; }

        Version Version { get; }
        string Revision { get; }

        #endregion
    }

    public static class BuildStatus
    {
        #region property

        public static BuildType BuildType
        {
            get
            {
#if DEBUG
                return BuildType.Debug;
#elif BETA
                return BuildType.Beta;
#else
                return BuildType.Release;
#endif
            }
        }

#pragma warning disable CS8601 // Null 参照割り当ての可能性があります。
        public static Version Version { get; } = Assembly.GetExecutingAssembly().GetName().Version;
#pragma warning restore CS8601 // Null 参照割り当ての可能性があります。
        public static string Revision { get; } = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;

        #endregion
    }

}
