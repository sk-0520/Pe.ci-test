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
		TextWriter _fileWriter = null;

		#endregion

		public Logger()
			:base()
		{ }

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
					ClearFileWriter();
				}

				this._filePath = value;
			}
		}

		/// <summary>
		/// FilePathで設定されたパスのファイルストリーム。
		/// </summary>
		protected TextWriter FileWriter 
		{
			get
			{
				if(this._fileWriter == null && CanFilePuts) {
					this._fileWriter = new StreamWriter(new FileStream(this._filePath, FileMode.Append, FileAccess.ReadWrite, FileShare.Read), Encoding.UTF8);
				}

				return this._fileWriter;
			}
		}
		public bool CanFilePuts
		{
			get
			{
				return !string.IsNullOrWhiteSpace(this._filePath) && LoggerConfig.PutsFile;
			}
		}

		#endregion

		#region LoggerBase

		protected override void Dispose(bool disposing)
		{
			if(!IsDisposed) {
				ClearFileWriter();
			}

			base.Dispose(disposing);
		}

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

		void ClearFileWriter()
		{
			if(this._fileWriter != null) {
				this._fileWriter.Dispose();
				this._fileWriter = null;
			}
		}

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
