using System;
using System.Reflection;

namespace ContentTypeTextNet.Pe.Main.Models
{
    /// <summary>
    /// ビルド種別。
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

    /// <summary>
    /// ビルド状態。
    /// </summary>
    public interface IBuildStatus
    {
        #region property

        /// <summary>
        /// ビルド種別。
        /// </summary>
        BuildType BuildType { get; }

        /// <summary>
        /// アプリケーションバージョン。
        /// </summary>
        Version Version { get; }

        /// <summary>
        /// リビジョン。
        /// </summary>
        /// <remarks>
        /// <para>ビルド時の git コミット。</para>
        /// </remarks>
        string Revision { get; }

        #endregion
    }

    /// <summary>
    /// ビルド状態ヘルパー。
    /// </summary>
    public static class BuildStatus
    {
        #region property

        /// <inheritdoc cref="IBuildStatus.BuildType"/>
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

        /// <summary>
        /// 本番バージョンか。
        /// </summary>
        /// <remarks>
        /// <para>本番バージョンはそれ以外と違って稼働ディレクトリが一段上になる。</para>
        /// </remarks>
        public static bool IsProduct { get; } =
#if PRODUCT
            true
#else
            false
#endif
        ;

        /// <inheritdoc cref="IBuildStatus.Version"/>
        public static Version Version { get; } = GetAssembly().GetName()!.Version!;

        /// <inheritdoc cref="IBuildStatus.Revision"/>
        public static string Revision { get; } = GetAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;

        /// <summary>
        /// アプリケーション名。
        /// </summary>
        public static string Name { get; } = GetAssembly().GetCustomAttribute<AssemblyProductAttribute>()!.Product;

        /// <summary>
        /// 著作権。
        /// </summary>
        public static string Copyright { get; } = GetAssembly().GetCustomAttribute<AssemblyCopyrightAttribute>()!.Copyright;

        #endregion

        #region function

        private static Assembly GetAssembly()
        {
            var assembly = Assembly.GetExecutingAssembly();
            if(assembly is null) {
                throw new InvalidOperationException();
            }

            return assembly;
        }

        #endregion
    }
}
