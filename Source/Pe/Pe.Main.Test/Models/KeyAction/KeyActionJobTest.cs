using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Main.Test.Models.KeyAction
{
    [TestClass]
    public class KeyActionJobTest
    {
        class PublicKeyActionJob : KeyActionJobBase
        {
            public PublicKeyActionJob()
                : base(new KeyActionCommonData(), new[] { new KeyMappingItemData() })
            { }

            public bool PublicTestMapping(IReadOnlyKeyMappingItemData mapping, bool isDown, Key key, in ModifierKeyStatus modifierKeyStatus)
            {
                return TestMapping(mapping, isDown, key, modifierKeyStatus);
            }
        }
        #region function

        [TestMethod]
        public void TestMappingTest_None()
        {
            var job = new PublicKeyActionJob();

            var mapping = new KeyMappingItemData() {
            };

            var actual = job.PublicTestMapping(
                mapping,
                true,
                Key.A,
                new ModifierKeyStatus()
            );
            Assert.IsFalse(actual);
        }

        [TestMethod]
        [DataRow(false, Key.None, Key.A)]
        [DataRow(false, Key.A, Key.None)]
        [DataRow(false, Key.A, Key.B)]
        [DataRow(true, Key.None, Key.None)] // 実運用上ないけどデータ的には正
        [DataRow(true, Key.A, Key.A)]
        public void TestMappingTest_KeyOnly(bool result, Key mapKey, Key inputKey)
        {
            var job = new PublicKeyActionJob();

            var mapping = new KeyMappingItemData() {
                Key = mapKey,
            };

            var actual = job.PublicTestMapping(
                mapping,
                true,
                inputKey,
                new ModifierKeyStatus()
            );
            Assert.AreEqual(result, actual);
        }

        [TestMethod]
        [DataRow(true, ModifierKey.None, false, false)]
        [DataRow(false, ModifierKey.Left, false, false)]
        [DataRow(true, ModifierKey.Left, true, false)]
        [DataRow(false, ModifierKey.Left, true, true)]
        [DataRow(false, ModifierKey.Left, false, true)]
        public void TestMappingTest_ModShift(bool result, ModifierKey mapMod, bool inputLeft, bool inputRight)
        {
            var job = new PublicKeyActionJob();

            var mapping = new KeyMappingItemData() {
                Key = Key.None,
                Shift = mapMod,
            };

            var actual = job.PublicTestMapping(
                mapping,
                true,
                Key.None,
                new ModifierKeyStatus(
                    new ModifierKeyState(inputLeft, inputRight),
                    new ModifierKeyState(),
                    new ModifierKeyState(),
                    new ModifierKeyState()
                )
            );
            Assert.AreEqual(result, actual);
        }

        [TestMethod]
        [DataRow(true, ModifierKey.None, false, false)]
        [DataRow(false, ModifierKey.Left, false, false)]
        [DataRow(true, ModifierKey.Left, true, false)]
        [DataRow(false, ModifierKey.Left, true, true)]
        [DataRow(false, ModifierKey.Left, false, true)]
        public void TestMappingTest_ModControl(bool result, ModifierKey mapMod, bool inputLeft, bool inputRight)
        {
            var job = new PublicKeyActionJob();

            var mapping = new KeyMappingItemData() {
                Key = Key.None,
                Control = mapMod,
            };

            var actual = job.PublicTestMapping(
                mapping,
                true,
                Key.None,
                new ModifierKeyStatus(
                    new ModifierKeyState(),
                    new ModifierKeyState(inputLeft, inputRight),
                    new ModifierKeyState(),
                    new ModifierKeyState()
                )
            );
            Assert.AreEqual(result, actual);
        }

        [TestMethod]
        [DataRow(true, ModifierKey.None, false, false)]
        [DataRow(false, ModifierKey.Left, false, false)]
        [DataRow(true, ModifierKey.Left, true, false)]
        [DataRow(false, ModifierKey.Left, true, true)]
        [DataRow(false, ModifierKey.Left, false, true)]
        public void TestMappingTest_ModAlt(bool result, ModifierKey mapMod, bool inputLeft, bool inputRight)
        {
            var job = new PublicKeyActionJob();

            var mapping = new KeyMappingItemData() {
                Key = Key.None,
                Alt = mapMod,
            };

            var actual = job.PublicTestMapping(
                mapping,
                true,
                Key.None,
                new ModifierKeyStatus(
                    new ModifierKeyState(),
                    new ModifierKeyState(),
                    new ModifierKeyState(inputLeft, inputRight),
                    new ModifierKeyState()
                )
            );
            Assert.AreEqual(result, actual);
        }


        [TestMethod]
        [DataRow(true, ModifierKey.None, false, false)]
        [DataRow(false, ModifierKey.Left, false, false)]
        [DataRow(true, ModifierKey.Left, true, false)]
        [DataRow(false, ModifierKey.Left, true, true)]
        [DataRow(false, ModifierKey.Left, false, true)]
        public void TestMappingTest_ModSuper(bool result, ModifierKey mapMod, bool inputLeft, bool inputRight)
        {
            var job = new PublicKeyActionJob();

            var mapping = new KeyMappingItemData() {
                Key = Key.None,
                Super = mapMod,
            };

            var actual = job.PublicTestMapping(
                mapping,
                true,
                Key.None,
                new ModifierKeyStatus(
                    new ModifierKeyState(),
                    new ModifierKeyState(),
                    new ModifierKeyState(),
                    new ModifierKeyState(inputLeft, inputRight)
                )
            );
            Assert.AreEqual(result, actual);
        }

        #endregion
    }
}
