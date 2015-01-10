using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Library.Utility;

namespace ContentTypeTextNet.Pe.Applications.Hash
{
	public class HashViewModel: AbstractViewModel
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
				OnPropertyChanged();

				if(!string.IsNullOrWhiteSpace(EventName)) {
					// イベント取得
					this._waitEvent = new EventWaitHandle(false, EventResetMode.AutoReset, this._model.EventName);
					Task.Factory.StartNew(() => {
						if(this._waitEvent.WaitOne(Timeout.Infinite, true)) {
							ForceExit = true;
						}
					});
				}
			}
		}

		class HashProgress
		{
			public HashAlgorithm Hash { get; set; }
			public Action<decimal> Percent { get; set; }
			public Action<string> Result { get; set; }
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
				OnPropertyChanged();

				if(File.Exists(this._model.FilePath)) {

					SHA1 = string.Empty;
					MD5 = string.Empty;
					CRC32 = string.Empty;

					var hashList = new[] {
						new HashProgress() {
							Hash = new SHA1CryptoServiceProvider(),
							Percent = p => PercentSHA1 = p,
							Result = r => SHA1 = r,
						},
						new HashProgress() {
							Hash = new MD5CryptoServiceProvider(),
							Percent = p => PercentMD5 = p,
							Result = r => MD5 = r,
						},
						new HashProgress() {
							Hash = new CRC32(),
							Percent = p => PercentCRC32 = p,
							Result = r => CRC32 = r,
						},
					};

					var binary = FileUtility.ToBinary(this._model.FilePath);

					int blockSize = 1024 * 1;
					int binaryLength = binary.Length;

					hashList.AsParallel().ForAll(hashItem => {
						//foreach(var hashItem in hashList) {
						//var b = hash.ComputeHash(binary);
						decimal doneSize = 0;
						int percent = 0;
						Task.Factory.StartNew(() => {
							for(int offset = 0; offset < binaryLength; offset += blockSize) {
								if(offset + blockSize < binaryLength) {
									hashItem.Hash.TransformBlock(binary, offset, blockSize, null, 0);
									doneSize = offset + blockSize;
								} else {
									hashItem.Hash.TransformFinalBlock(binary, offset, binaryLength - offset);
									doneSize = binaryLength;
								}
								if(percent != doneSize * 100 / binaryLength) {
									percent = (int)(doneSize * 100 / binaryLength);
									hashItem.Percent(percent);
									//Debug.WriteLine("{0}, {1}", DateTime.Now.ToLongTimeString(), percent);
								}
							}
							hashItem.Percent(100);
							var text = ToHashString(hashItem.Hash.Hash);
							hashItem.Result(text);
						});
					});
					//}
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
				OnPropertyChanged();
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
				OnPropertyChanged();
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
				OnPropertyChanged();
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
				OnPropertyChanged();
			}
		}

		public decimal PercentSHA1
		{
			get { return this._model.PercentSHA1; }
			set
			{
				if(this._model.PercentSHA1 == value) {
					return;
				}

				this._model.PercentSHA1 = value;
				OnPropertyChanged();
			}
		}

		public decimal PercentMD5
		{
			get { return this._model.PercentMD5; }
			set
			{
				if(this._model.PercentMD5 == value) {
					return;
				}

				this._model.PercentMD5 = value;
				OnPropertyChanged();
			}
		}

		public decimal PercentCRC32
		{
			get { return this._model.PercentCRC32; }
			set
			{
				if(this._model.PercentCRC32 == value) {
					return;
				}

				this._model.PercentCRC32 = value;
				OnPropertyChanged();
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
				OnPropertyChanged();
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
				OnPropertyChanged();
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
