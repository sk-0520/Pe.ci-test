using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Plugins.Reference.FileFinder.Addon
{
    internal class FileFinderCommandItem: ICommandItem
    {
        #region define

        [DllImport("shell32.dll")]
        private static extern IntPtr ExtractAssociatedIcon(IntPtr hInst, StringBuilder lpIconPath, out ushort lpiIcon);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
        private extern static bool DestroyIcon(IntPtr handle);

        #endregion

        public FileFinderCommandItem(string path, string fullMatchValue, IImageLoader imageLoader, IAddonExecutor addonExecutor)
        {
            Path = path;
            FullMatchValue = fullMatchValue;
            ImageLoader = imageLoader;
            AddonExecutor = addonExecutor;
        }

        public FileFinderCommandItem(string path, IImageLoader imageLoader, IAddonExecutor addonExecutor)
            : this(path, path, imageLoader, addonExecutor)
        { }

        #region property

        private string Path { get; }

        //System.Windows.Controls.Image? ImageControl { get; set; }
        private ImageSource? ImageSource { get; set; }

        private IImageLoader ImageLoader { get; }
        private IAddonExecutor AddonExecutor { get; }

        #endregion

        #region ICommandItem

        public CommandItemKind Kind { get; } = CommandItemKind.Plugin;

        public List<HitValue> HeaderValues { get; } = new List<HitValue>();
        IReadOnlyList<HitValue> ICommandItem.HeaderValues => HeaderValues;

        public List<HitValue> DescriptionValues { get; } = new List<HitValue>();
        IReadOnlyList<HitValue> ICommandItem.DescriptionValues => DescriptionValues;

        public List<HitValue> ExtendDescriptionValues { get; } = new List<HitValue>();
        IReadOnlyList<HitValue> ICommandItem.ExtendDescriptionValues => ExtendDescriptionValues;

        public int Score { get; internal set; }

        public string FullMatchValue { get; }

        public void Execute(ICommandExecuteParameter parameter)
        {
            if(parameter.IsExtend) {
                AddonExecutor.Execute(Path);
            } else {
                AddonExecutor.ExtendsExecute(Path);
            }
        }

        public object GetIcon(in IconScale iconScale)
        {
            //var c = char.ToUpper(Path[0]);
            //if(Path.Length == "C:\\".Length && ('A' <= c && c <= 'Z') && Path[1] == System.IO.Path.VolumeSeparatorChar && System.IO.Path.EndsInDirectorySeparator(Path)) {

            //}
            //using var icon = Icon.ExtractAssociatedIcon(Path);
            //Image = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, null);
            if(ImageSource == null) {
                //var hInstance = Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]);
                //var hIcon = ExtractAssociatedIcon(hInstance, new StringBuilder(Path), out _);
                //try {
                //    using var icon = Icon.FromHandle(hIcon);
                //    ImageSource = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, null);
                //    ImageSource.Freeze();
                //} finally {
                //    DestroyIcon(hIcon);
                //}
                ImageSource = ImageLoader.LoadIconFromFile(Path, 0, iconScale);
                ImageSource?.Freeze();
            }
            var iconSize = iconScale.ToIconSize();
            return new System.Windows.Controls.Image() {
                Source = ImageSource,
                //Width = iconSize.Width,
                //Height = iconSize.Height,
                Width = (int)iconScale.Box,
                Height = (int)iconScale.Box,
            };

        }

        public bool IsEquals(ICommandItem? commandItem)
        {
            if(commandItem == null) {
                return false;
            }

            if(Kind != commandItem.Kind) {
                return false;
            }

            if(commandItem is FileFinderCommandItem fileCommandItem) {
                var a = System.IO.Path.TrimEndingDirectorySeparator(Path);
                var b = System.IO.Path.TrimEndingDirectorySeparator(fileCommandItem.Path);

                return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        #endregion
    }
}
