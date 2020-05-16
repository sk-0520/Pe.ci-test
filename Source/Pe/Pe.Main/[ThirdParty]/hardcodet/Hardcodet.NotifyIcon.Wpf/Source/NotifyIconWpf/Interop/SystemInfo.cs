#pragma warning disable S125 // Sections of code should not be commented out
#pragma warning disable S1066 // Collapsible "if" statements should be merged
#pragma warning disable S3265 // Non-flags enums should not be used in bitwise operations
#pragma warning disable S3881 // "IDisposable" should be implemented correctly
#pragma warning disable S3963 // "static" fields should be initialized inline
#pragma warning disable S112 // General exceptions should never be thrown
#pragma warning disable S101 // Types should be named in PascalCase
#pragma warning disable S1128 // Unused "using" should be removed
#pragma warning disable S2344 // Enumeration type names should not have "Flags" or "Enum" suffixes

using System.Windows.Interop;

namespace Hardcodet.Wpf.TaskbarNotification.Interop
{
    /// <summary>
    /// This class is a helper for system information, currently to get the DPI factors
    /// </summary>
    public static class SystemInfo
    {
        private static readonly System.Windows.Point DpiFactors;

        static SystemInfo()
        {
            using (var source = new HwndSource(new HwndSourceParameters()))
            {
                if (source.CompositionTarget?.TransformToDevice != null)
                {
                    DpiFactors = new System.Windows.Point(source.CompositionTarget.TransformToDevice.M11, source.CompositionTarget.TransformToDevice.M22);
                    return;
                }
                DpiFactors = new System.Windows.Point(1, 1);
            }
        }

        /// <summary>
        /// Returns the DPI X Factor
        /// </summary>
        public static double DpiFactorX => DpiFactors.X;

        /// <summary>
        /// Returns the DPI Y Factor
        /// </summary>
        public static double DpiFactorY => DpiFactors.Y;
    }
}
