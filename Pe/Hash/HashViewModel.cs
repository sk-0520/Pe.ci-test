using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ContentTypeTextNet.Pe.Applications.Hash
{
	public class HashViewModel: BaseViewModel
	{
		static string ToHashString(byte[] binary)
		{
			var sb = new StringBuilder(binary.Length * 2);
			foreach(var b in binary) {
				sb.Append(string.Format("{0:x2}", b));
			}
			return sb.ToString();
		}

		private HashModel _model;
		private bool _forceExit;
		private Brush _backgrond;
		private EventWaitHandle _waitEvent;

		public HashViewModel(HashModel model)
		{
			this._model = model;
		}

		public bool ForceExit 
		{ 
			get { return this._forceExit; }
			set
			{
				if(this._forceExit != value) {
					return;
				}

				this._forceExit = value;
			}
		}

		public string EventName
		{
			get { return this._model.EventName; }
			set
			{
				if(this._model.EventName == value) {
					return;
				}

				this._model.EventName = value;
				OnPropertyChanged("EventName");

				if(!string.IsNullOrWhiteSpace(EventName)) {
					// イベント取得
					this._waitEvent = new EventWaitHandle(false, EventResetMode.AutoReset, this._model.EventName);
					Task.Factory.StartNew(() => {
						if(this._waitEvent.WaitOne(Timeout.Infinite, true)) {
							ForceExit = true;
							Debug.WriteLine("asdfghjkl.;");
						}
					});
				}
			}
		}

		public string FilePath
		{
			get { return this._model.FilePath; }
			set
			{
				if(this._model.FilePath == value) {
					return;
				}

				Computed = false;

				this._model.FilePath = value;
				OnPropertyChanged("FilePath");

				if(File.Exists(this._model.FilePath)) {
					SHA1 = string.Empty;
					MD5 = string.Empty;
					CRC32 = string.Empty;

					var map = new Dictionary<HashAlgorithm, Action<byte[]>>() {
						{ new SHA1CryptoServiceProvider(), b => SHA1 = ToHashString(b) },
						{ new MD5CryptoServiceProvider(), b => MD5 = ToHashString(b) },
						{ new CRC32(), b => CRC32 = ToHashString(b) },
					};

					var binary = File.ReadAllBytes(this._model.FilePath);
					foreach(var pair in map) {
						var hash = pair.Key;
						var b = hash.ComputeHash(binary);
						pair.Value(b);
					}

					Computed = true;
				}
				CheckHash();
			}
		}

		public HashType HashType
		{
			get { return this._model.HashType; }
			set
			{
				if(this._model.HashType == value) {
					return;
				}

				this._model.HashType = value;
				OnPropertyChanged("HashType");
				CheckHash();
			}
		}

		public string SHA1
		{
			get { return this._model.SHA1; }
			set
			{
				if(this._model.SHA1 == value) {
					return;
				}

				this._model.SHA1 = value;
				OnPropertyChanged("SHA1");
			}
		}

		public string MD5
		{
			get { return this._model.MD5; }
			set
			{
				if(this._model.MD5 == value) {
					return;
				}

				this._model.MD5 = value;
				OnPropertyChanged("MD5");
			}
		}

		public string CRC32
		{
			get { return this._model.CRC32; }
			set
			{
				if(this._model.CRC32 == value) {
					return;
				}

				this._model.CRC32 = value;
				OnPropertyChanged("CRC32");
			}
		}

		public string Compare
		{
			get { return this._model.Compare; }
			set
			{
				if(this._model.Compare == value) {
					return;
				}

				this._model.Compare = value;
				OnPropertyChanged("Compare");
				CheckHash();
			}
		}

		public bool Computed { get; set; }

		public Brush Backgrond
		{
			get { return this._backgrond; }
			set
			{
				if(this._backgrond == value) {
					return;
				}

				this._backgrond = value;
				OnPropertyChanged("Backgrond");
			}
		}

		void CheckHash()
		{
			if(Computed) {
				var map = new Dictionary<HashType, string>() {
						{ HashType.SHA1, SHA1 },
						{ HashType.MD5, MD5 },
						{ HashType.CRC32, CRC32 },
					};
				var equal = map[HashType] == Compare;
				if(equal) {
					Backgrond = Brushes.Lime;
				} else {
					Backgrond = Brushes.Red;
				}
			} else {
				Backgrond = null;
			}
		}
	}
}
