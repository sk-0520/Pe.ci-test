using System;
using System.Management;

namespace ContentTypeTextNet.Pe.PInvoke.Windows.root.CIMV2
{
    public class CIM_UserDevice: IImportWMI
    {
        public ushort? Availability { get; set; }
        public string? Caption { get; set; }
        public uint? ConfigManagerErrorCode { get; set; }
        public bool? ConfigManagerUserConfig { get; set; }
        public string? CreationClassName { get; set; }
        public string? Description { get; set; }
        public string? DeviceID { get; set; }
        public bool? ErrorCleared { get; set; }
        public string? ErrorDescription { get; set; }
        public DateTime? InstallDate { get; set; }
        public bool? IsLocked { get; set; }
        public uint? LastErrorCode { get; set; }
        public string? Name { get; set; }
        public string? PNPDeviceID { get; set; }
        public ushort[]? PowerManagementCapabilities { get; set; }
        public bool? PowerManagementSupported { get; set; }
        public string? Status { get; set; }
        public ushort? StatusInfo { get; set; }
        public string? SystemCreationClassName { get; set; }
        public string? SystemName { get; set; }

        public virtual void Import(ManagementBaseObject obj)
        {
            Availability = (ushort?)obj[nameof(Availability)];
            Caption = (string)obj[nameof(Caption)];
            ConfigManagerErrorCode = (uint?)obj[nameof(ConfigManagerErrorCode)];
            ConfigManagerUserConfig = (bool?)obj[nameof(ConfigManagerUserConfig)];
            CreationClassName = (string)obj[nameof(CreationClassName)];
            Description = (string)obj[nameof(Description)];
            DeviceID = (string)obj[nameof(DeviceID)];
            ErrorCleared = (bool?)obj[nameof(ErrorCleared)];
            ErrorDescription = (string)obj[nameof(ErrorDescription)];
            InstallDate = (DateTime?)obj[nameof(InstallDate)];
            IsLocked = (bool?)obj[nameof(IsLocked)];
            LastErrorCode = (uint?)obj[nameof(LastErrorCode)];
            Name = (string)obj[nameof(Name)];
            PNPDeviceID = (string)obj[nameof(PNPDeviceID)];
            PowerManagementCapabilities = (ushort[])obj[nameof(PowerManagementCapabilities)];
            PowerManagementSupported = (bool?)obj[nameof(PowerManagementSupported)];
            Status = (string)obj[nameof(Status)];
            StatusInfo = (ushort?)obj[nameof(StatusInfo)];
            SystemCreationClassName = (string)obj[nameof(SystemCreationClassName)];
            SystemName = (string)obj[nameof(SystemName)];
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
        public string? MonitorManufacturer { get; set; }
        public string? MonitorType { get; set; }
        public uint? PixelsPerXLogicalInch { get; set; }
        public uint? PixelsPerYLogicalInch { get; set; }
        public uint? ScreenHeight { get; set; }
        public uint? ScreenWidth { get; set; }

        public override void Import(ManagementBaseObject obj)
        {
            base.Import(obj);

            Bandwidth = (uint?)obj[nameof(Bandwidth)];
            DisplayType = (ushort?)obj[nameof(DisplayType)];
            MonitorManufacturer = (string)obj[nameof(MonitorManufacturer)];
            MonitorType = (string)obj[nameof(MonitorType)];
            PixelsPerXLogicalInch = (uint?)obj[nameof(PixelsPerXLogicalInch)];
            PixelsPerYLogicalInch = (uint?)obj[nameof(PixelsPerYLogicalInch)];
            ScreenHeight = (uint?)obj[nameof(ScreenHeight)];
            ScreenWidth = (uint?)obj[nameof(ScreenWidth)];
        }

    }
}
