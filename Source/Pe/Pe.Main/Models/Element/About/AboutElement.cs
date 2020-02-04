using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.About
{
    public class AboutElement : ElementBase
    {
        public AboutElement(EnvironmentParameters environmentParameters, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            EnvironmentParameters = environmentParameters;
            CustomConfiguration = EnvironmentParameters.Configuration;
        }

        #region property

        EnvironmentParameters EnvironmentParameters { get; }
        CustomConfiguration CustomConfiguration { get; }

        private List<AboutComponentItem> ComponentsImpl { get; } = new List<AboutComponentItem>();
        public IReadOnlyList<AboutComponentItem> Components => ComponentsImpl;

        #endregion

        #region function

        private AboutComponentsData LoadComponents()
        {
            var serializer = new JsonDataSerializer();
            using(var stream = EnvironmentParameters.ComponentsFile.OpenRead()) {
                return serializer.Load<AboutComponentsData>(stream);
            }
        }

        private IEnumerable<AboutComponentItem> ToItems(AboutComponentKind kind, IEnumerable<AboutComponentData> items)
        {
            return items
                .Counting()
                .Select(i => new AboutComponentItem(kind, i.Value, i.Number))
            ;
        }

        private IEnumerable<AboutComponentItem> GetApplicationItems()
        {
            var data = new[] {
                new AboutComponentData() {
                    Name = BuildStatus.Name,
                    Uri = CustomConfiguration.General.ProjectWebSiteUri.ToString(),
                    License = new AboutLicenseData() {
                        Name = CustomConfiguration.General.LicenseName,
                        Uri = CustomConfiguration.General.LicenseUri.ToString(),
                    },
                    Comment = $"{BuildStatus.BuildType}: {BuildStatus.Version} - {BuildStatus.Revision}"
                },
            };

            return ToItems(AboutComponentKind.Application, data);
        }

        public void OpenUri(Uri uri)
        {
            var systemExecutor = new SystemExecutor(LoggerFactory);
            try {
                systemExecutor.OpenUri(uri);
            } catch(Exception ex) {
                Logger.LogWarning(ex, ex.Message);
            }
        }

        public void OpenForumUri()
        {
            OpenUri(CustomConfiguration.General.ProjectForumUri);
        }
        public void OpenProjectUri()
        {
            OpenUri(CustomConfiguration.General.ProjectRepositoryUri);
        }

        private void Copt(string s)
        {
            Logger.LogWarning("TODO");
        }

        public void CopyShortInformation()
        {
            var infoCollector = new ApplicationInformationCollector(EnvironmentParameters);
            var s = infoCollector.GetShortInformation();
            Copt(s);
        }
        public void CopyLongInformation()
        {
            var infoCollector = new ApplicationInformationCollector(EnvironmentParameters);
            var s = infoCollector.GetLongInformation();
            Copt(s);
        }

        private void OpenDirectory(DirectoryInfo directory)
        {
            try {
                var process = Process.Start(new ProcessStartInfo(directory.FullName) {
                    UseShellExecute = true,
                });
            } catch(Exception ex) {
                Logger.LogWarning(ex, ex.Message);
            }
        }

        public void OpenApplicationDirectory()
        {
            OpenDirectory(EnvironmentParameters.RootDirectory);
        }
        public void OpenUserDirectory()
        {
            OpenDirectory(EnvironmentParameters.UserRoamingDirectory);
        }
        public void OpenMachineDirectory()
        {
            OpenDirectory(EnvironmentParameters.MachineDirectory);
        }
        public void OpenTemporaryDirectory()
        {
            OpenDirectory(EnvironmentParameters.TemporaryDirectory);
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            var components = LoadComponents();

            ComponentsImpl.AddRange(GetApplicationItems());
            ComponentsImpl.AddRange(ToItems(AboutComponentKind.Library, components.Library));
            ComponentsImpl.AddRange(ToItems(AboutComponentKind.Resource, components.Resource));
        }

        #endregion
    }
}
