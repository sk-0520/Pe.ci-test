/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/11/21
 * 時刻: 22:26
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using PeUtility;
using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace PeUtilityTest
{
	[TestFixture]
	public class TextUtilityTest
	{
		public void ToUniqTest(string src, IEnumerable<string> list, Func<string, int, string> f)
		{
			TextUtility.ToUnique(src, list, f);
//			Assert.AreEqual(expectedA,a);
//			Assert.AreEqual(expectedB,b);
		}
	}
}
