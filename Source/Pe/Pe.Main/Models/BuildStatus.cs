using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models
{
    /// <summary>
    /// ビルド種別
    /// </summary>
    public enum BuildType
    {
        /// <summary>
        /// デバッグ版。
        /// </summary>
        Debug,
        /// <summary>
        /// β版。
        /// </summary>
        Beta,
        /// <summary>
        /// リリース版。
        /// </summary>
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

        /// <summary>
        /// ビルド種別。
        /// </summary>
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

        public static bool IsProduct { get; } =
#if PRODUCT
            true
#else
            false
#endif
        ;

        /// <summary>
        /// バージョン。
        /// </summary>
        public static Version Version { get; } = Assembly.GetExecutingAssembly()!.GetName()!.Version!;
        /// <summary>
        /// リビジョン。
        /// </summary>
        public static string Revision { get; } = Assembly.GetExecutingAssembly()!.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;

        /// <summary>
        /// アプリケーション名。
        /// </summary>
        public static string Name { get; } = Assembly.GetExecutingAssembly()!.GetCustomAttribute<AssemblyProductAttribute>()!.Product;
        /// <summary>
        /// 著作権。
        /// </summary>
        public static string Copyright { get; } = Assembly.GetExecutingAssembly()!.GetCustomAttribute<AssemblyCopyrightAttribute>()!.Copyright;

        #endregion
    }

}
