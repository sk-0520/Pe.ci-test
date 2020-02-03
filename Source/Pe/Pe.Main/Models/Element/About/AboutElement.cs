using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
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
            var data = new [] {
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
