using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.Base.Linq;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class KeyMappingsEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void Insert_Select_Delete_Test()
        {
            var testKeyActions = Test.BuildDao<KeyActionsEntityDao>(AccessorKind.Main);
            var testKeyMappings = Test.BuildDao<KeyMappingsEntityDao>(AccessorKind.Main);

            var action = new KeyActionData() {
                KeyActionId = KeyActionId.NewId(),
                KeyActionKind = KeyActionKind.Replace,
                KeyActionContent = "Replace",
                Comment = "Comment:Replace",
            };
            testKeyActions.InsertKeyAction(action, Test.DiContainer.Build<IDatabaseCommonStatus>());
            var mappings = new[] {
                new KeyMappingData(){
                    Key = System.Windows.Input.Key.A,
                    Shift = ModifierKey.All,
                },
                new KeyMappingData(){
                    Key = System.Windows.Input.Key.B,
                    Shift = ModifierKey.Left,
                },
                new KeyMappingData(){
                    Key = System.Windows.Input.Key.C,
                    Shift = ModifierKey.Right,
                }
            };
            foreach(var mapping in mappings.Counting()) {
                testKeyMappings.InsertMapping(action.KeyActionId, mapping.Value, mapping.Number, Test.DiContainer.Build<IDatabaseCommonStatus>());
            }

            var actual = testKeyMappings.SelectMappings(action.KeyActionId).ToArray();
            Assert.Equal(System.Windows.Input.Key.A, actual[0].Key);
            Assert.Equal(ModifierKey.All, actual[0].Shift);
            Assert.Equal(ModifierKey.None, actual[0].Control);
            Assert.Equal(ModifierKey.None, actual[0].Alt);
            Assert.Equal(ModifierKey.None, actual[0].Super);
            Assert.Equal(System.Windows.Input.Key.B, actual[1].Key);
            Assert.Equal(ModifierKey.Left, actual[1].Shift);
            Assert.Equal(ModifierKey.None, actual[1].Control);
            Assert.Equal(ModifierKey.None, actual[1].Alt);
            Assert.Equal(ModifierKey.None, actual[1].Super);
            Assert.Equal(System.Windows.Input.Key.C, actual[2].Key);
            Assert.Equal(ModifierKey.Right, actual[2].Shift);
            Assert.Equal(ModifierKey.None, actual[2].Control);
            Assert.Equal(ModifierKey.None, actual[2].Alt);
            Assert.Equal(ModifierKey.None, actual[2].Super);

            testKeyMappings.DeleteByKeyActionId(action.KeyActionId);
            Assert.Empty(testKeyMappings.SelectMappings(action.KeyActionId));
        }


        #endregion

    }
}
