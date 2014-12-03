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
	public enum PeUpdaterCode
	{
		Unknown,
		NotFoundArgument,
	}
	/// <summary>
	/// Desctiption of PeUpdaterException.
	/// </summary>
	[Serializable]
	public class PeUpdaterException : Exception, ISerializable
	{
		public PeUpdaterException(): base(PeUpdaterCode.Unknown.ToString())
		{
	 		PeUpdaterCode = PeUpdaterCode.Unknown;
		}
		
		public PeUpdaterException(PeUpdaterCode pc): base(pc.ToString())
		{
	 		PeUpdaterCode = pc;
		}
	 	
	 	public PeUpdaterCode PeUpdaterCode { get; private set; }

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}
	}
}