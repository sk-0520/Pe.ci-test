using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;

namespace ContentTypeTextNet.Pe.Library.Utility
{
	/// <summary>
	/// ショートカット。
	/// </summary>
	public class ShortcutFile: IDisposable
	{
		private static StringBuilder CreateStringBuffer()
		{
			return CreateStringBuffer((int)MAX.MAX_PATH);
		}
		private static StringBuilder CreateStringBuffer(int max)
		{
			return new StringBuilder(max, max);
		}
		private static IShellLink CreateShellLink()
		{
			return (IShellLink)new ShellLinkObject();
		}
		protected IShellLink _shellLink;

		public ShortcutFile()
		{
			this._shellLink = CreateShellLink();
		}

		public ShortcutFile(string path)
		{
			Load(path);
		}

		private IPersistFile PersistFile {
			get
			{
				var result = this._shellLink as IPersistFile;
				if (result == null) {
					throw new COMException("cannot create IPersistFile");
				}

				return result;
			}
		}

		public string TargetPath
		{
			get
			{
				var resultBuffer = CreateStringBuffer();
				var findData = new WIN32_FIND_DATA();

				this._shellLink.GetPath(resultBuffer, resultBuffer.MaxCapacity, out findData, SLGP_FLAGS.SLGP_UNCPRIORITY);

				return resultBuffer.ToString();
			}
			set
			{
				this._shellLink.SetPath(value);
			}
		}

		public string Arguments
		{
			get
			{
				var max = 1024;
				var resultBuffer = CreateStringBuffer(max);

				this._shellLink.GetArguments(resultBuffer, resultBuffer.Capacity);

				return resultBuffer.ToString();
			}
			set
			{
				this._shellLink.SetArguments(value);
			}
		}

		public string Description
		{
			get
			{
				var max = 1024 * 5;
				var resultBuffer = CreateStringBuffer(max);

				this._shellLink.GetDescription(resultBuffer, resultBuffer.Capacity);

				return resultBuffer.ToString();
			}
			set
			{
				this._shellLink.SetDescription(value);
			}
		}

		public string WorkingDirectory
		{
			get
			{
				var resultBuffer = CreateStringBuffer();

				this._shellLink.GetWorkingDirectory(resultBuffer, resultBuffer.MaxCapacity);

				return resultBuffer.ToString();
			}
			set
			{
				this._shellLink.SetWorkingDirectory(value);
			}
		}

		public SW ShowCommand
		{
			get
			{
				int rawShowCommand;

				this._shellLink.GetShowCmd(out rawShowCommand);

				return (SW)rawShowCommand;
			}
			set
			{
				this._shellLink.SetShowCmd((int)value);
			}
		}

		public IconPath GetIcon()
		{
			var resultBuffer = CreateStringBuffer();
			int iconIndex;

			this._shellLink.GetIconLocation(resultBuffer, resultBuffer.Capacity, out iconIndex);

			return new IconPath(resultBuffer.ToString(), iconIndex);
		}

		public void SetIcon(IconPath iconPath)
		{
			this._shellLink.SetIconLocation(iconPath.Path, iconPath.Index);
		}

		#region IDisposable

		protected void Dispose(bool disposing)
		{
			Marshal.ReleaseComObject(this._shellLink);
			this._shellLink = null;
		}

		public void Dispose()
		{
			Dispose(true);
		}

		#endregion /////////////////////////////////

		public void Load(string path)
		{
			if(this._shellLink != null) {
				Dispose(true);
			}

			this._shellLink = CreateShellLink();
			PersistFile.Load(path, 0);
		}

		public void Save(string path)
		{
			PersistFile.Save(path, true);
		}
	}

}
