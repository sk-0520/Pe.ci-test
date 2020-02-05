using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Platform;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    public class ApplicationInformationCollector : PlatformInformationCollector
    {
        public ApplicationInformationCollector(EnvironmentParameters environmentParameters)
        {
            EnvironmentParameters = environmentParameters;
        }

        #region property

        EnvironmentParameters EnvironmentParameters { get; }

        #endregion

        #region function

        public virtual IList<PlatformInformationItem> GetApplication()
        {
            return new[] {
                new PlatformInformationItem(nameof(BuildStatus.Name), BuildStatus.Name),
                new PlatformInformationItem(nameof(BuildStatus.BuildType), BuildStatus.BuildType),
                new PlatformInformationItem(nameof(BuildStatus.Version), BuildStatus.Version),
                new PlatformInformationItem(nameof(BuildStatus.Revision), BuildStatus.Revision),
                new PlatformInformationItem(nameof(BuildStatus.Copyright), BuildStatus.Copyright),
            }.ToList();
        }

        public string GetShortInformation()
        {
            //TODO
            return string.Empty;
        }

        public string GetLongInformation()
        {
            //TODO
            return string.Empty;
        }

        #endregion
    }
}
