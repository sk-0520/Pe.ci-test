namespace ContentTypeTextNet.Test.Library.SharedLibraryTest.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using NUnit.Framework;

	[TestFixture]
	class ReflectionUtilityTest
	{
		public class GetSerializeMembers_Pair
		{
			public GetSerializeMembers_Pair(int count, object data)
			{
				Count = count;
				Data = data;
			}
			public int Count { get; set; }
			public object Data { get; set; }
		}

		class Test_pub0_pro0_pri0
		{ }
		class Test_pub1_pro0_pri0
		{
			public int a = 0;
		}
		class Test_pub0_pro1_pri0
		{
			protected int a = 0;
		}
		class Test_pub0_pro0_pri1
		{
#pragma warning disable 0414
			private int a = 0;
#pragma warning restore 0414
		}

		
		IEnumerable<object> GetSerializeMembers_Test()
		{
			yield return new GetSerializeMembers_Pair(0, new Test_pub0_pro0_pri0());
			yield return new GetSerializeMembers_Pair(0, new Test_pub1_pro0_pri0());
			yield return new GetSerializeMembers_Pair(0, new Test_pub0_pro1_pri0());
			yield return new GetSerializeMembers_Pair(0, new Test_pub0_pro0_pri1());
		}

		[TestCaseSource("GetSerializeMembers_Test")]
		public void GetSerializeMembers(GetSerializeMembers_Pair pair)
		{
			var result = ReflectionUtility.GetSerializeMembers(pair.Data);
			Assert.AreEqual(pair.Count, result.Count());
		}
	}
}
