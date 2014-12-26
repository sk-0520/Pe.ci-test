using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Applications.Hash
{
	public class HashViewModel: ViewModelBase
	{
		private HashModel _model;

		public HashViewModel(HashModel model)
		{
			this._model = model;
		}

		public string FilePath 
		{
			get { return this._model.FilePath; }
			set
			{
				if(this._model.FilePath == value) {
					return;
				}

				this._model.FilePath = value;
				OnPropertyChanged("FilePath");

				if(File.Exists(this._model.FilePath)) {
					SHA1 = string.Empty;
					MD5 = string.Empty;
					CRC32 = string.Empty;
					// 
				}
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
			}
		}
	}
}
