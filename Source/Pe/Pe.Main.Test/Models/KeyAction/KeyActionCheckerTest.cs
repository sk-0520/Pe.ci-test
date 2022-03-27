using System;
using System.Linq;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Main.Test.Models.KeyAction
{
    [TestClass]
    public class KeyActionCheckerTest
    {
        #region function

        [TestMethod]
        public void FindTest_Simple()
        {
            var keyActionChecker = new KeyActionChecker(Test.LoggerFactory) {
                KeyDisableToEnableTime = TimeSpan.Zero,
            };
            keyActionChecker.DisableJobs.Add(new KeyActionDisableJob(
                new KeyActionDisableData(new KeyActionId(Guid.NewGuid()), false),
                new KeyMappingData() {
                    Key = Key.B
                }
            ));

            var modifierKeyStatus = new ModifierKeyStatus();

            var expectedUpA = keyActionChecker.Find(false, Key.A, modifierKeyStatus, new KBDLLHOOKSTRUCT());
            var expectedUpB = keyActionChecker.Find(false, Key.B, modifierKeyStatus, new KBDLLHOOKSTRUCT());
            var expectedUpC = keyActionChecker.Find(false, Key.C, modifierKeyStatus, new KBDLLHOOKSTRUCT());
            Assert.IsFalse(expectedUpA.Any());
            Assert.IsFalse(expectedUpB.Any());
            Assert.IsFalse(expectedUpC.Any());

            var expectedDownA = keyActionChecker.Find(true, Key.A, modifierKeyStatus, new KBDLLHOOKSTRUCT());
            var expectedDownB = keyActionChecker.Find(true, Key.B, modifierKeyStatus, new KBDLLHOOKSTRUCT());
            var expectedDownC = keyActionChecker.Find(true, Key.C, modifierKeyStatus, new KBDLLHOOKSTRUCT());
            Assert.IsFalse(expectedDownA.Any());
            Assert.IsTrue(expectedDownB.Any());
            Assert.IsFalse(expectedDownC.Any());

        }

        [TestMethod]
        public void FindTest_DisableToEnable()
        {
            var keyActionChecker = new KeyActionChecker(Test.LoggerFactory);
            keyActionChecker.DisableJobs.Add(new KeyActionDisableJob(
                new KeyActionDisableData(new KeyActionId(Guid.NewGuid()), false),
                new KeyMappingData() {
                    Key = Key.A
                }
            ));

            var modifierKeyStatus = new ModifierKeyStatus();

            var expected1 = keyActionChecker.Find(true, Key.A, modifierKeyStatus, new KBDLLHOOKSTRUCT());
            keyActionChecker.KeyDisableToEnableTime = TimeSpan.MaxValue;
            var expected2 = keyActionChecker.Find(true, Key.A, modifierKeyStatus, new KBDLLHOOKSTRUCT());
            keyActionChecker.KeyDisableToEnableTime = TimeSpan.Zero;
            var expected3 = keyActionChecker.Find(true, Key.A, modifierKeyStatus, new KBDLLHOOKSTRUCT());
            Assert.IsTrue(expected1.Any());
            Assert.IsFalse(expected2.Any());
            Assert.IsTrue(expected3.Any());
        }

        [TestMethod]
        public void FindTest_DisableStopEnable()
        {
            var keyActionChecker = new KeyActionChecker(Test.LoggerFactory);
            keyActionChecker.DisableJobs.Add(new KeyActionDisableJob(
                new KeyActionDisableData(new KeyActionId(Guid.NewGuid()), true),
                new KeyMappingData() {
                    Key = Key.A
                }
            ));

            var modifierKeyStatus = new ModifierKeyStatus();

            var expected1 = keyActionChecker.Find(true, Key.A, modifierKeyStatus, new KBDLLHOOKSTRUCT());
            keyActionChecker.KeyDisableToEnableTime = TimeSpan.MaxValue;
            var expected2 = keyActionChecker.Find(true, Key.A, modifierKeyStatus, new KBDLLHOOKSTRUCT());
            keyActionChecker.KeyDisableToEnableTime = TimeSpan.Zero;
            var expected3 = keyActionChecker.Find(true, Key.A, modifierKeyStatus, new KBDLLHOOKSTRUCT());
            Assert.IsTrue(expected1.Any());
            Assert.IsTrue(expected2.Any());
            Assert.IsTrue(expected3.Any());
        }

        [TestMethod]
        public void FindTest_Replace()
        {
            var keyActionChecker = new KeyActionChecker(Test.LoggerFactory);
            keyActionChecker.ReplaceJobs.Add(new KeyActionReplaceJob(
                new KeyActionReplaceData(new KeyActionId(Guid.NewGuid()), Key.B),
                new KeyMappingData() {
                    Key = Key.A
                }
            ));
            keyActionChecker.DisableJobs.Add(new KeyActionDisableJob(
                new KeyActionDisableData(new KeyActionId(Guid.NewGuid()), false),
                new KeyMappingData() {
                    Key = Key.C
                }
            ));
            keyActionChecker.DisableJobs.Add(new KeyActionDisableJob(
                new KeyActionDisableData(new KeyActionId(Guid.NewGuid()), false),
                new KeyMappingData() {
                    Key = Key.C
                }
            ));

            var modifierKeyStatus = new ModifierKeyStatus();

            var expected1 = keyActionChecker.Find(true, Key.C, modifierKeyStatus, new KBDLLHOOKSTRUCT());
            Assert.AreEqual(1, expected1.Count);

            var expected2 = keyActionChecker.Find(true, Key.A, modifierKeyStatus, new KBDLLHOOKSTRUCT());
            Assert.AreEqual(1, expected2.Count);

            keyActionChecker.DisableJobs.Add(new KeyActionDisableJob(
                new KeyActionDisableData(new KeyActionId(Guid.NewGuid()), false),
                new KeyMappingData() {
                    Key = Key.A
                }
            ));
            var expected3 = keyActionChecker.Find(true, Key.A, modifierKeyStatus, new KBDLLHOOKSTRUCT());
            Assert.AreEqual(1, expected3.Count);
            var job = (KeyActionReplaceJob)expected3.First();
            Assert.AreEqual(Key.B, keyActionChecker.ReplaceJobs[0].ActionData.ReplaceKey);
        }
        #endregion
    }
}
