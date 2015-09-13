using System;
using System.Runtime.Serialization;

namespace ContentTypeTextNet.Pe.Applications.Updater
{
	public enum UpdaterCode
	{
		Unknown,
		NotFoundArgument,
		ScriptCompile,
	}
	/// <summary>
	/// Desctiption of PeUpdaterException.
	/// </summary>
	[Serializable]
	public class UpdaterException : Exception, ISerializable
	{
		public UpdaterException(): base(UpdaterCode.Unknown.ToString())
		{
	 		UpdaterCode = UpdaterCode.Unknown;
		}
		
		public UpdaterException(UpdaterCode pc): base(pc.ToString())
		{
	 		UpdaterCode = pc;
		}
	 	
	 	public UpdaterCode UpdaterCode { get; private set; }

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}
	}
}