namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Model;

	/// <summary>
	/// 最低限度の機能を保持したログ出力処理。
	/// </summary>
	public class Logger: LoggerBase, IIsDisposed
	{
		#region varable

		string _filePath = null;

		#endregion

		public Logger()
			:base()
		{
			FileWriter = null;
		}

		~Logger()
		{
			Dispose(false);
		}

		#region property

		/// <summary>
		/// ファイルログに使用するファイルパス。
		/// <para>値設定が有効なものであれば既存ファイルを閉じて指定されたファイルに追記していく。</para>
		/// </summary>
		public string FilePath
		{
			get { return this._filePath; }
			set 
			{
				if(this._filePath != value) {
					if(FileWriter != null) {
						FileWriter.Dispose();
						FileWriter = null;
					}
				}

				this._filePath = value;

				if(!string.IsNullOrWhiteSpace(this._filePath)) {
					FileWriter = new StreamWriter(new FileStream(this._filePath, FileMode.Append, FileAccess.ReadWrite, FileShare.Read), Encoding.UTF8);
				}
			}
		}

		/// <summary>
		/// FilePathで設定されたパスのファイルストリーム。
		/// </summary>
		protected TextWriter FileWriter { get; private set; }


		#endregion

		#region IIsDisposed

		public bool IsDisposed { get; protected set; }

		protected virtual void Dispose(bool disposing)
		{
			if(IsDisposed) {
				return;
			}

			if(FileWriter != null) {
				FileWriter.Dispose();
				FileWriter = null;
			}

			IsDisposed = true;
			GC.SuppressFinalize(this);
		}

		public void Dispose()
		{
			Dispose(true);
		}

		#endregion

		#region LoggerBase

		protected override void PutsFile(LogItemModel item)
		{
			if(FileWriter != null) {
				FileWriter.WriteLine(item);
			}
		}

		protected override void PutsConsole(LogItemModel item)
		{
			Console.WriteLine(PutsOutput(item, 'C'));
		}

		protected override void PutsDebug(LogItemModel item)
		{
			System.Diagnostics.Debug.WriteLine(PutsOutput(item, 'D'));
		}

		/// <summary>
		/// このクラスでは何もしない。
		/// <para>サブクラスで適当にどうぞ。</para>
		/// </summary>
		/// <param name="item"></param>
		protected override void PutsCustom(LogItemModel item)
		{ }

		#endregion

		#region function

		string PutsOutput(LogItemModel item, char c)
		{
			return string.Format(
				"{0}{1}[{2}] {3}(4): {5}",
				item.DateTime,
				c,
				item.LogKind.ToString().ToUpper()[0],
				item.CallerMember,
				item.CallerLine,
				item.Message
			);
		}

		#endregion


	}
}
