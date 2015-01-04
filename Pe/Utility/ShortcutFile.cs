using System;
using System.Diagnostics;
using System.IO;
using IWshRuntimeLibrary;

namespace ContentTypeTextNet.Pe.Library.Utility
{
    /// <summary>
    /// ショートカット。
    /// </summary>
    public class ShortcutFile: IWshShortcut
    {
        IWshShortcut _shortcut;

        public ShortcutFile(string path, bool isCreste)
        {
            IsCreate = isCreste;
            if(IsCreate) {
                var wshShell = CreateShell();
                this._shortcut = (IWshShortcut)wshShell.CreateShortcut(path);
            } else {
                Load(path);
            }
        }

        public bool IsCreate { get; private set; }

        public string FullName
        {
            get { return this._shortcut.FullName; }
        }

        public string TargetPath
        {
            get
            {
                var path = this._shortcut.TargetPath;
                var expandPath = Environment.ExpandEnvironmentVariables(path);
                if(FileUtility.Exists(expandPath)) {
                    return expandPath;
                }
                var dirPath = Path.GetDirectoryName(expandPath);
                var x86pfPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                var x64pfPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                var x86Index = dirPath.IndexOf(x86pfPath);
                var x64Index = dirPath.IndexOf(x64pfPath);

                if(x86Index == 0) {
                    // x86指定の場合にx64を考慮(なんかへんだこれ)
                    var relationPath = dirPath.Substring(x86pfPath.Length);
                    var x64TargetPath = Path.Combine(x64pfPath, relationPath);
                    if(FileUtility.Exists(x64TargetPath)) {
                        return x64TargetPath;
                    }
                }

                return path;
            }
            set { this._shortcut.TargetPath = value; }
        }


        public string Arguments
        {
            get { return this._shortcut.Arguments; }
            set { this._shortcut.Arguments = value; }
        }

        public string Description
        {
            get { return this._shortcut.Description; }
            set { this._shortcut.Description = value; }
        }

        public string Hotkey
        {
            get { return this._shortcut.Hotkey; }
            set { this._shortcut.Hotkey = value; }
        }

        public string IconLocation
        {
            get { return this._shortcut.Hotkey; }
            set { this._shortcut.Hotkey = value; }
        }

        public string RelativePath
        {
            set { this._shortcut.RelativePath = value; }
        }

        public int WindowStyle
        {
            get { return this._shortcut.WindowStyle; }
            set { this._shortcut.WindowStyle = value; }
        }

        public string WorkingDirectory
        {
            get { return this._shortcut.WorkingDirectory; }
            set { this._shortcut.WorkingDirectory = value; }
        }

        public string IconPath { get; set; }
        public int IconIndex { get; set; }

        protected IWshRuntimeLibrary.WshShell CreateShell()
        {
            if(Environment.Is64BitOperatingSystem && Environment.Is64BitProcess) {
                return new IWshRuntimeLibrary.WshShell();
            } else {
                return new IWshRuntimeLibrary.WshShellClass();
            }
        }

        public void Load(string path)
        {
            var wshShell = CreateShell();
            this._shortcut = (IWshShortcut)wshShell.CreateShortcut(path);

            var iconPath = this._shortcut.IconLocation;
            var index = iconPath.LastIndexOf(',');
            if(index == -1) {
                IconPath = iconPath;
                IconIndex = 0;
            } else {
                IconPath = iconPath.Substring(0, index);
                IconIndex = int.Parse(iconPath.Substring(index + 1));
            }
        }

        public void Save()
        {
            Debug.Assert(IsCreate);
            this._shortcut.IconLocation = string.Format("{0},{1}", IconPath, IconIndex);
            this._shortcut.Save();
        }
    }
}
