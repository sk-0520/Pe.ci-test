/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/09/25
 * 時刻: 23:33
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Runtime.Serialization;

namespace ContentTypeTextNet.Pe.Applications.Updater
{
	public enum UpdaterCode
	{
		Unknown,
		NotFoundArgument,
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