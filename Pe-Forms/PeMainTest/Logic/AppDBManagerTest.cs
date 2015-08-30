namespace ContentTypeTextNet.Pe.Test.ApplicationTest.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.PeMain.Logic;
	using NUnit.Framework;
	using System.Data;
	using System.Data.SQLite;
	using ContentTypeTextNet.Pe.PeMain.Logic.DB;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.UI;

	[TestFixture]
	class AppDBManagerTest
	{
		const int noteStart = 1;
		const int noteCount = 100;

		public AppDBManager CreateAppDBManagerTest()
		{
			var connection = new SQLiteConnection("Data Source=:memory:");
			var dbManager = new AppDBManager(connection, false);
			using(var query = dbManager.CreateQuery()) {
				foreach(var pair in App.GetTableCreatCommand()) {
					query.ExecuteCommand(pair.Value);
				}
			}

			var noteItemList = new List<NoteItem>();
			foreach(var n in Enumerable.Range(noteStart, noteCount)) {
				var noteItem = new NoteItem();
				noteItem.NoteId = n;
				noteItem.Title = n.ToString();
				noteItemList.Add(noteItem);
			}
			var noteDb = new NoteDB(dbManager);
			var tran = dbManager.BeginTransaction();
			noteDb.Resist(noteItemList);
			noteDb.ToDisabled(noteItemList.Where(n => n.NoteId % 2 == 0));
			tran.Commit();

			return dbManager;
		}

		[Test]
		public void AppDBManagerTest_ctor()
		{
			Assert.IsNotNull(CreateAppDBManagerTest());
		}

		AppDBManager dbManager_ExistsTableTest;
		[TestFixtureSetUp]
		public void ExistsTableTest()
		{
			dbManager_ExistsTableTest = CreateAppDBManagerTest();
		}

		[TestCase(false, "")]
		[TestCase(false, "M_VERSION")]
		[TestCase(true, "M_NOTE")]
		[TestCase(true, "T_NOTE")]
		[TestCase(true, "T_NOTE_STYLE")]
		[TestCase(false, "m_note")]
		[TestCase(false, "t_note")]
		[TestCase(false, "t_note")]
		public void ExistsTableTest(bool test, string tableName)
		{
			Assert.AreEqual(test, dbManager_ExistsTableTest.ExistsTable(tableName));
		}

		AppDBManager dbManager_GetTableIdTest;
		[TestFixtureSetUp]
		public void GetTableIdTest()
		{
			dbManager_GetTableIdTest = CreateAppDBManagerTest();
		}

		[TestCase(1, 100, "M_NOTE", "NOTE_ID")]
		[TestCase(1, 100, "T_NOTE", "NOTE_ID")]
		[TestCase(1, 100, "T_NOTE_STYLE", "NOTE_ID")]
		public void GetTableIdTest(long min, long max, string tableName, string idColumnName)
		{
			var dbManager = dbManager_GetTableIdTest;
			var dto = dbManager.GetTableId(tableName, idColumnName);
			Assert.AreEqual(min, dto.MinId);
			Assert.AreEqual(max, dto.MaxId);
		}

		[Test]
		public void DeleteDisableItemTest()
		{
			var dbManager = CreateAppDBManagerTest();

			Func<string, long> getCount = (tableName) => {
				using(var query = dbManager.CreateQuery()) {
					query.SetExpression("TABLE_NAME", tableName);
					using(var r = query.ExecuteReader("select count(*) as NUM from {TABLE_NAME}")) {
						r.Read();
						return dbManager.To<long>(r["NUM"]);
					}
				}
			};
			Assert.AreEqual(noteCount, getCount("M_NOTE"));
			Assert.AreEqual(noteCount, getCount("T_NOTE"));
			Assert.AreEqual(noteCount, getCount("T_NOTE_STYLE"));
			dbManager.DeleteDisableItem(new NullLogger());
			var enabledCount = noteCount / 2;
			Assert.AreEqual(enabledCount, getCount("M_NOTE"));
			Assert.AreEqual(enabledCount, getCount("T_NOTE"));
			Assert.AreEqual(enabledCount, getCount("T_NOTE_STYLE"));
		}
	}
}
