using System;
using System.Collections.Generic;
using System.Data.SQLite;
using NUnit.Framework;
using ContentTypeTextNet.Pe.Library.Utility;
using ContentTypeTextNet.Pe.Library.Utility.DB;

namespace ContentTypeTextNet.Pe.Test.UtilityTest
{
	[TestFixture]
	public class SQLiteDBManagerTest
	{
		SQLiteDBManager Create()
		{
			var connection = new SQLiteConnection("Data Source=:memory:");
			return new SQLiteDBManager(connection, false);
		}

		[Test]
		public void CreateTableTest()
		{
			using(var db = Create()) {
				using(var query = db.CreateQuery()) {
					query.ExecuteCommand("create table TEST(COL1 integer, COL2 text)");
					Assert.Throws(typeof(SQLiteException), () => query.ExecuteCommand("create table TEST(COL1 integer, COL2 text)"));
				}
			}
		}
	}
}
