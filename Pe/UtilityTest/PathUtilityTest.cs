﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ContentTypeTextNet.Pe.Library.Utility;

namespace ContentTypeTextNet.Pe.Test.UtilityTest
{
	[TestFixture]
	public class PathUtilityTest
	{
		[TestCase(false, "")]
		[TestCase(false, "a.txt")]
		[TestCase(false, "a.xexe")]
		[TestCase(false, ".htaccess")]
		[TestCase(false, "exe")]
		[TestCase(false, "com")]
		[TestCase(false, "bat")]
		[TestCase(true, ".exe")]
		[TestCase(true, ".com")]
		[TestCase(true, ".bat")]
		[TestCase(true, "a.exe")]
		[TestCase(true, "b.com")]
		[TestCase(true, "v.bat")]
		public void IsExecutePathTest(bool test, string s)
		{
			Assert.IsTrue(test == PathUtility.IsExecutePath(s));
		}

		[TestCase("a.txt", "a", "txt")]
		[TestCase("a.txt.txt", "a.txt", "txt")]
		[TestCase("a..txt", "a.", "txt")]
		[TestCase("a..txt", "a", ".txt")]
		public void AppendExtensionTest(string test, string path, string ext)
		{
			Assert.IsTrue(test == PathUtility.AppendExtension(path, ext));
		}

		[TestCase("", "")]
		[TestCase("", " ")]
		[TestCase("abc", "abc")]
		[TestCase("_abc", "*abc")]
		[TestCase("_abc_", "*abc*")]
		[TestCase("_a_bc_", "*a\\bc*")]
		[TestCase("_a_b_c_", "*a\\b/c*")]
		[TestCase("a_b", "a?b")]
		[TestCase("a_b", "a<b")]
		[TestCase("a_b", "a>b")]
		public void ToSafeNameTest(string test, string name)
		{
			Assert.IsTrue(test == PathUtility.ToSafeName(name));
		}
	}
}
