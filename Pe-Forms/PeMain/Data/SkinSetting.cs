namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using ContentTypeTextNet.Pe.PeMain.IF;

	[Serializable]
	public class SkinSetting: NameItem, IDeepClone
	{
		#region IDeepClone

		public IDeepClone DeepClone()
		{
			return new SkinSetting() {
				Name = this.Name,
			};
		}

		#endregion
	}
}
