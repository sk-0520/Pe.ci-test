using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class KeyOptionsEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void Insert_Select_Delete_Test()
        {
            var testKeyActions = Test.BuildDao<KeyActionsEntityDao>(AccessorKind.Main);
            var testKeyOptions = Test.BuildDao<KeyOptionsEntityDao>(AccessorKind.Main);

            var action = new KeyActionData() {
                KeyActionId = KeyActionId.NewId(),
                KeyActionKind = KeyActionKind.Replace,
                KeyActionContent = "Replace",
                Comment = "Comment:Replace",
            };
            testKeyActions.InsertKeyAction(action, Test.DiContainer.Build<IDatabaseCommonStatus>());

            testKeyOptions.InsertOption(action.KeyActionId, "NAME1", "VALUE1", Test.DiContainer.Build<IDatabaseCommonStatus>());
            testKeyOptions.InsertOption(action.KeyActionId, "NAME2", "VALUE2", Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.Throws<SQLiteException>(() => testKeyOptions.InsertOption(action.KeyActionId, "NAME2", "VALUE2", Test.DiContainer.Build<IDatabaseCommonStatus>()));

            var actual = testKeyOptions.SelectOptions(action.KeyActionId).ToDictionary(k => k.Key, v => v.Value);
            Assert.Equal("VALUE1", actual["NAME1"]);
            Assert.Equal("VALUE2", actual["NAME2"]);

            testKeyOptions.DeleteByKeyActionId(action.KeyActionId);
            Assert.Empty(testKeyOptions.SelectOptions(action.KeyActionId));
        }

        #endregion
    }
}
