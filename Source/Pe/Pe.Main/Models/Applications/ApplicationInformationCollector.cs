using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Main.Models.Platform;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    public class ApplicationInformationCollector: PlatformInformationCollector
    {
        public ApplicationInformationCollector(EnvironmentParameters environmentParameters)
        {
            EnvironmentParameters = environmentParameters;
        }

        #region property

        EnvironmentParameters EnvironmentParameters { get; }
        #endregion
    }
}
