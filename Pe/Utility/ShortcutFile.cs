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

		/// <summary>
		/// ショートカットを作成するためにオブジェクト生成。
		/// </summary>
		public ShortcutFile()
		{
			this._shellLink = CreateShellLink();
		}

		/// <summary>
		/// ショートカットを読み込むためにオブジェクト生成。
		/// </summary>
		/// <param name="path">読み込むショートカットファイルパス。</param>
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

		/// <summary>
		/// ショートカット先パス。
		/// </summary>
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

		/// <summary>
		/// 引数。
		/// </summary>
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

		/// <summary>
		/// コメント。
		/// </summary>
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

		/// <summary>
		/// 作業ディレクトリ。
		/// </summary>
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

		/// <summary>
		/// 表示方法。
		/// </summary>
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

		/// <summary>
		/// アイコン取得。
		/// </summary>
		/// <returns></returns>
		public IconPath GetIcon()
		{
			var resultBuffer = CreateStringBuffer();
			int iconIndex;

			this._shellLink.GetIconLocation(resultBuffer, resultBuffer.Capacity, out iconIndex);

			return new IconPath(resultBuffer.ToString(), iconIndex);
		}

		/// <summary>
		/// アイコン設定。
		/// </summary>
		/// <param name="iconPath"></param>
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

		/// <summary>
		/// ショートカット読み込み。
		/// 
		/// public だが Save との IF を合わせるためだけであり基本的には外から使用しない。
		/// 使っても問題ないけど。
		/// </summary>
		/// <param name="path">読み込むショートカットパス。</param>
		public void Load(string path)
		{
			if(this._shellLink != null) {
				Dispose(true);
			}

			this._shellLink = CreateShellLink();
			PersistFile.Load(path, 0);
		}

		/// <summary>
		/// ショートカットを保存。
		/// </summary>
		/// <param name="path">保存先ショートカットパス。</param>
		public void Save(string path)
		{
			PersistFile.Save(path, true);
		}
	}

}
