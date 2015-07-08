namespace ContentTypeTextNet.Library.SharedLibrary.Logic.T4Template
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Security;
	using System.Text;
	using System.Threading.Tasks;
	using Microsoft.VisualStudio.TextTemplating;

	/// <summary>
	/// セッションの実装.
	/// 単純にDictionaryと、IDを持っているだけのコレクションクラスである.
	/// </summary>
	[Serializable]
	public sealed class TextTemplatingSession : Dictionary<string, Object>, ITextTemplatingSession, ISerializable
	{
		public TextTemplatingSession()
		{
			this.Id = Guid.NewGuid();
		}

		private TextTemplatingSession(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.Id = (Guid)info.GetValue("Id", typeof(Guid));
		}

		[SecurityCritical]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("Id", this.Id);
		}

		public Guid Id
		{
			get;
			private set;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			var o = obj as TextTemplatingSession;
			return o != null && o.Equals(this);
		}

		public bool Equals(Guid other)
		{
			return other.Equals(Id);
		}

		public bool Equals(ITextTemplatingSession other)
		{
			return other != null && other.Id == this.Id;
		}
	}
}
