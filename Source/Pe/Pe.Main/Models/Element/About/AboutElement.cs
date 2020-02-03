using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
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


        #endregion

        #region function

        private AboutComponentsData LoadComponents()
        {
            var serializer = new JsonDataSerializer();
            using(var stream = EnvironmentParameters.ComponentsFile.OpenRead()) {
                return serializer.Load<AboutComponentsData>(stream);
            }
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            var component = LoadComponents();
        }

        #endregion
    }
}
