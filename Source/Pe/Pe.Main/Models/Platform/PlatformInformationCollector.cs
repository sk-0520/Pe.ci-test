using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Main.Models.Platform
{
    public class PlatformInformationCollector : DisposerBase
    {
        #region property
        #endregion

        #region function

        protected Dictionary<string, object> GetInfo(ManagementClass managementClass)
        {
            var result = new Dictionary<string, object>();

            using(var mc = managementClass.GetInstances()) {
                foreach(var mo in mc) {
                    var collection = mo.Properties
                        .OfType<PropertyData>()
                    ;
                    foreach(var property in collection) {
                        result[property.Name] = property.Value;
                    }
                }
            }

            return result;
        }

        public virtual IReadOnlyDictionary<string, object> GetCPU()
        {
            using(var managementCpu = new ManagementClass("Win32_Processor")) {
                return GetInfo(managementCpu);
            }
        }

        public virtual IReadOnlyDictionary<string, object> GetOS()
        {
            using(var managementOs = new ManagementClass("Win32_OperatingSystem")) {
                return GetInfo(managementOs);
            }
        }

        #endregion

        #region DisposerBase

        #endregion
    }
}
