using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.Base;

namespace ContentTypeTextNet.Pe.Main.Models.Platform
{
    public struct PlatformInformationItem
    {
        public PlatformInformationItem(string key, object? value)
        {
            Key = key;
            Value = value;
        }

        #region property

        public string Key { get; }
        public object? Value { get; }

        #endregion
    }

    /// <summary>
    /// システム情報収集。
    /// </summary>
    public class PlatformInformationCollector: DisposerBase
    {
        #region property
        #endregion

        #region function

        protected List<PlatformInformationItem> GetInfo(ManagementClass managementClass)
        {
            var result = new List<PlatformInformationItem>();

            using(var mc = managementClass.GetInstances()) {
                foreach(var mo in mc) {
                    var collection = mo.Properties
                        .OfType<PropertyData>()
                    ;
                    foreach(var property in collection) {
                        result.Add(new PlatformInformationItem(property.Name, property.Value));
                    }
                }
            }

            return result;
        }

        public virtual IList<PlatformInformationItem> GetCPU()
        {
            using(var managementCpu = new ManagementClass("Win32_Processor")) {
                return GetInfo(managementCpu);
            }
        }

        public virtual IList<PlatformInformationItem> GetOS()
        {
            using(var managementOs = new ManagementClass("Win32_OperatingSystem")) {
                return GetInfo(managementOs);
            }
        }

        public virtual IList<PlatformInformationItem> GetEnvironment()
        {
            var type = typeof(Environment);
            var props = type.GetProperties(BindingFlags.Static | BindingFlags.Public);

            var result = new List<PlatformInformationItem>(props.Length);

            var ignoreProperties = new HashSet<string>() {
                nameof(Environment.StackTrace),
            };

            foreach(var prop in props) {
                if(ignoreProperties.Contains(prop.Name)) {
                    continue;
                }
                var value = prop.GetValue(type, null);
                switch(prop.Name) {
                    case nameof(Environment.NewLine):
                        result.Add(new PlatformInformationItem(prop.Name, BitConverter.ToString(Encoding.UTF8.GetBytes((string)value!))));
                        break;

                    default:
                        result.Add(new PlatformInformationItem(prop.Name, value));
                        break;
                }
            }

            return result;
        }

        public virtual IList<PlatformInformationItem> GetEnvironmentVariables()
        {
            var envVars = Environment.GetEnvironmentVariables();

            var result = new List<PlatformInformationItem>(envVars.Count);

            foreach(var entry in envVars.OfType<DictionaryEntry>()) {
                result.Add(new PlatformInformationItem((string)entry.Key, entry.Value));
            }

            return result;
        }

        public virtual IList<PlatformInformationItem> GetRuntimeInformation()
        {
            var type = typeof(System.Runtime.InteropServices.RuntimeInformation);
            var props = type.GetProperties(BindingFlags.Static | BindingFlags.Public);

            var result = new List<PlatformInformationItem>(props.Length);

            foreach(var prop in props) {
                var value = prop.GetValue(type, null);
                switch(prop.Name) {
                    default:
                        result.Add(new PlatformInformationItem(prop.Name, value));
                        break;
                }
            }
            return result;
        }

        public virtual IList<PlatformInformationItem> GetScreen()
        {
            var screens = Screen.AllScreens;

            var result = new List<PlatformInformationItem>(screens.Length * 5);
            result.Add(new PlatformInformationItem("screen", screens.Length));

            for(var i = 0; i < screens.Length; i++) {
                var screen = screens[i];
                var head = $"screen[{i}].";
                result.Add(new PlatformInformationItem(head + nameof(screen.BitsPerPixel), screen.BitsPerPixel));
                result.Add(new PlatformInformationItem(head + nameof(screen.Primary), screen.Primary));
                result.Add(new PlatformInformationItem(head + nameof(screen.DeviceName), screen.DeviceName));
                result.Add(new PlatformInformationItem(head + nameof(screen.DeviceBounds), screen.DeviceBounds));
                result.Add(new PlatformInformationItem(head + nameof(screen.DeviceWorkingArea), screen.DeviceWorkingArea));
            }

            return result;
        }

        #endregion

        #region DisposerBase

        #endregion
    }
}
