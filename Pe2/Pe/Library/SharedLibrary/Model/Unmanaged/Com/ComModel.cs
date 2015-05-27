namespace ContentTypeTextNet.Pe.Library.SharedLibrary.Model.Unmanaged.Com
{
	/// <summary>
	/// 何かしらのCOMを管理。
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ComModel<T>: ComModelBase
	{
		public ComModel(T com)
			: base(com)
		{
			Com = com;
		}

		public T Com { get; private set; }
	}

}
