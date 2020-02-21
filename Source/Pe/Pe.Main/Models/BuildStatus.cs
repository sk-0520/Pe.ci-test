using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models
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
        public static Version Version { get; } = Assembly.GetExecutingAssembly()!.GetName()!.Version!;
        public static string Revision { get; } = Assembly.GetExecutingAssembly()!.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;

        public static string Name { get; } = Assembly.GetExecutingAssembly()!.GetCustomAttribute<AssemblyProductAttribute>()!.Product;
        public static string Copyright { get; }= Assembly.GetExecutingAssembly()!.GetCustomAttribute<AssemblyCopyrightAttribute>()!.Copyright;

        #endregion
    }

}
