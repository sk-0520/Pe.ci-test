/*
This file is part of PInvoke.

PInvoke is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

PInvoke is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with PInvoke.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Management;

// TODO: リフレクション実装すべき
namespace ContentTypeTextNet.Library.PInvoke.Windows.root.CIMV2
{
    public class CIM_UserDevice: IImportWMI
    {
        public ushort? Availability { get; set; }
        public string Caption { get; set; }
        public uint? ConfigManagerErrorCode { get; set; }
        public bool? ConfigManagerUserConfig { get; set; }
        public string CreationClassName { get; set; }
        public string Description { get; set; }
        public string DeviceID { get; set; }
        public bool? ErrorCleared { get; set; }
        public string ErrorDescription { get; set; }
        public DateTime? InstallDate { get; set; }
        public bool? IsLocked { get; set; }
        public uint? LastErrorCode { get; set; }
        public string Name { get; set; }
        public string PNPDeviceID { get; set; }
        public ushort[] PowerManagementCapabilities { get; set; }
        public bool? PowerManagementSupported { get; set; }
        public string Status { get; set; }
        public ushort? StatusInfo { get; set; }
        public string SystemCreationClassName { get; set; }
        public string SystemName { get; set; }

        public virtual void Import(ManagementBaseObject obj)
        {
            Availability = (ushort?)obj["Availability"];
            Caption = (string)obj["Caption"];
            ConfigManagerErrorCode = (uint?)obj["ConfigManagerErrorCode"];
            ConfigManagerUserConfig = (bool?)obj["ConfigManagerUserConfig"];
            CreationClassName = (string)obj["CreationClassName"];
            Description = (string)obj["Description"];
            DeviceID = (string)obj["DeviceID"];
            ErrorCleared = (bool?)obj["ErrorCleared"];
            ErrorDescription = (string)obj["ErrorDescription"];
            InstallDate = (DateTime?)obj["InstallDate"];
            IsLocked = (bool?)obj["IsLocked"];
            LastErrorCode = (uint?)obj["LastErrorCode"];
            Name = (string)obj["Name"];
            PNPDeviceID = (string)obj["PNPDeviceID"];
            PowerManagementCapabilities = (ushort[])obj["PowerManagementCapabilities"];
            PowerManagementSupported = (bool?)obj["PowerManagementSupported"];
            Status = (string)obj["Status"];
            StatusInfo = (ushort?)obj["StatusInfo"];
            SystemCreationClassName = (string)obj["SystemCreationClassName"];
            SystemName = (string)obj["SystemName"];
        }

    }

    public class CIM_Display: CIM_UserDevice, IImportWMI
    {
        public override void Import(ManagementBaseObject obj)
        {
            base.Import(obj);
        }
    }

    public class Win32_DesktopMonitor: CIM_Display, IImportWMI
    {
        public uint? Bandwidth { get; set; }
        public ushort? DisplayType { get; set; }
        public string MonitorManufacturer { get; set; }
        public string MonitorType { get; set; }
        public uint? PixelsPerXLogicalInch { get; set; }
        public uint? PixelsPerYLogicalInch { get; set; }
        public uint? ScreenHeight { get; set; }
        public uint? ScreenWidth { get; set; }

        public override void Import(ManagementBaseObject obj)
        {
            base.Import(obj);

            Bandwidth = (uint?)obj["Bandwidth"];
            DisplayType = (ushort?)obj["DisplayType"];
            MonitorManufacturer = (string)obj["MonitorManufacturer"];
            MonitorType = (string)obj["MonitorType"];
            PixelsPerXLogicalInch = (uint?)obj["PixelsPerXLogicalInch"];
            PixelsPerYLogicalInch = (uint?)obj["PixelsPerYLogicalInch"];
            ScreenHeight = (uint?)obj["ScreenHeight"];
            ScreenWidth = (uint?)obj["ScreenWidth"];
        }

    }
}
