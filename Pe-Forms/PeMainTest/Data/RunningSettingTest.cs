namespace ContentTypeTextNet.Pe.Test.ApplicationTest.Data
{
	using ContentTypeTextNet.Pe.PeMain.Data;
	using NUnit.Framework;

	[TestFixture]
	class RunningSettingTest
	{
		[TestCase(0, 1)]
		[TestCase(-1, 0)]
		[TestCase(int.MaxValue - 1, int.MaxValue)]
		[TestCase(int.MaxValue , int.MaxValue)]
		public void IncrementExecuteCountTest(int init, int result)
		{
			var ri = new RunningSetting() {
				ExecuteCount = init,
			};
			ri.IncrementExecuteCount();
			Assert.AreEqual(ri.ExecuteCount, result);
		}
	}
}
