using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    public interface IApplicationInformation
    {
        #region property

        /// <summary>
        /// アプリケーションバージョン。
        /// </summary>
        Version Version { get; }
        /// <summary>
        /// アプリケーションアーキテクチャ(プラットフォーム)。
        /// </summary>
        string Architecture { get; }

        #endregion
    }

    internal class ApplicationInformation: IApplicationInformation
    {
        public ApplicationInformation(Version version, string architecture)
        {
            Version = version;
            Architecture = architecture;
        }
    
        #region IApplicationProcessInformation

        public Version Version { get; }
        public string Architecture { get; }

        #endregion
    }
}
