using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.About
{
    public class AboutElement : ElementBase
    {
        public AboutElement(EnvironmentParameters environmentParameters, IClipboardManager clipboardManager, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            EnvironmentParameters = environmentParameters;
            ApplicationConfiguration = EnvironmentParameters.ApplicationConfiguration;
            ClipboardManager = clipboardManager;
        }

        #region property

        EnvironmentParameters EnvironmentParameters { get; }
        ApplicationConfiguration ApplicationConfiguration { get; }
        IClipboardManager ClipboardManager { get; }

        private List<AboutComponentItem> ComponentsImpl { get; } = new List<AboutComponentItem>();
        public IReadOnlyList<AboutComponentItem> Components => ComponentsImpl;

        public UninstallTarget UninstallTargets { get; internal set; }

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
            var versionConverter = new VersionConverter();

            var data = new[] {
                new AboutComponentData() {
                    Name = BuildStatus.Name,
                    Uri = ApplicationConfiguration.General.ProjectWebSiteUri.ToString(),
                    License = new AboutLicenseData() {
                        Name = ApplicationConfiguration.General.LicenseName,
                        Uri = ApplicationConfiguration.General.LicenseUri.ToString(),
                    },
                    Comment = $"{BuildStatus.BuildType}: {versionConverter.ConvertNormalVersion(BuildStatus.Version)} - {BuildStatus.Revision}"
                },
            };

            return ToItems(AboutComponentKind.Application, data);
        }

        public void OpenUri(Uri uri)
        {
            var systemExecutor = new SystemExecutor();
            try {
                systemExecutor.OpenUri(uri);
            } catch(Exception ex) {
                Logger.LogWarning(ex, ex.Message);
            }
        }

        public void OpenForumUri()
        {
            OpenUri(ApplicationConfiguration.General.ProjectForumUri);
        }
        public void OpenProjectUri()
        {
            OpenUri(ApplicationConfiguration.General.ProjectRepositoryUri);
        }

        public void CopyShortInformation()
        {
            var infoCollector = new ApplicationInformationCollector(EnvironmentParameters);
            var s = infoCollector.GetShortInformation();
            ClipboardManager.CopyText(s, ClipboardNotify.User);
        }
        public void CopyLongInformation()
        {
            var infoCollector = new ApplicationInformationCollector(EnvironmentParameters);
            var s = infoCollector.GetLongInformation();
            ClipboardManager.CopyText(s, ClipboardNotify.User);
        }

        private void OpenDirectory(DirectoryInfo directory)
        {
            try {
                var systemExecutor = new SystemExecutor();
                systemExecutor.ExecuteFile(directory.FullName);
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
