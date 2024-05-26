using System;
using System.Linq;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.KeyAction
{
    public class KeyActionCheckerTest
    {
        #region property

        private Test Test { get; } = Test.Create();

        #endregion

        #region function

        [Fact]
        public void FindTest_Simple()
        {
            var keyActionChecker = new KeyActionChecker(Test.LoggerFactory) {
                KeyDisableToEnableTime = TimeSpan.Zero,
            };
            keyActionChecker.DisableJobs.Add(new KeyActionDisableJob(
                new KeyActionDisableData(KeyActionId.NewId(), false),
                new KeyMappingData() {
                    Key = Key.B
                }
            ));

            var modifierKeyStatus = new ModifierKeyStatus();

            var expectedUpA = keyActionChecker.Find(false, Key.A, modifierKeyStatus, new KBDLLHOOKSTRUCT());
            var expectedUpB = keyActionChecker.Find(false, Key.B, modifierKeyStatus, new KBDLLHOOKSTRUCT());
            var expectedUpC = keyActionChecker.Find(false, Key.C, modifierKeyStatus, new KBDLLHOOKSTRUCT());
            Assert.False(expectedUpA.Any());
            Assert.False(expectedUpB.Any());
            Assert.False(expectedUpC.Any());

            var expectedDownA = keyActionChecker.Find(true, Key.A, modifierKeyStatus, new KBDLLHOOKSTRUCT());
            var expectedDownB = keyActionChecker.Find(true, Key.B, modifierKeyStatus, new KBDLLHOOKSTRUCT());
            var expectedDownC = keyActionChecker.Find(true, Key.C, modifierKeyStatus, new KBDLLHOOKSTRUCT());
            Assert.False(expectedDownA.Any());
            Assert.True(expectedDownB.Any());
            Assert.False(expectedDownC.Any());

        }

        [Fact]
        public void FindTest_DisableToEnable()
        {
            var keyActionChecker = new KeyActionChecker(Test.LoggerFactory);
            keyActionChecker.DisableJobs.Add(new KeyActionDisableJob(
                new KeyActionDisableData(KeyActionId.NewId(), false),
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
            Assert.True(expected1.Any());
            Assert.False(expected2.Any());
            Assert.True(expected3.Any());
        }

        [Fact]
        public void FindTest_DisableStopEnable()
        {
            var keyActionChecker = new KeyActionChecker(Test.LoggerFactory);
            keyActionChecker.DisableJobs.Add(new KeyActionDisableJob(
                new KeyActionDisableData(KeyActionId.NewId(), true),
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
            Assert.True(expected1.Any());
            Assert.True(expected2.Any());
            Assert.True(expected3.Any());
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Assertions", "xUnit2013:Do not use equality check to check for collection size.", Justification = "<保留中>")]
        public void FindTest_Replace()
        {
            var keyActionChecker = new KeyActionChecker(Test.LoggerFactory);
            keyActionChecker.ReplaceJobs.Add(new KeyActionReplaceJob(
                new KeyActionReplaceData(KeyActionId.NewId(), Key.B),
                new KeyMappingData() {
                    Key = Key.A
                }
            ));
            keyActionChecker.DisableJobs.Add(new KeyActionDisableJob(
                new KeyActionDisableData(KeyActionId.NewId(), false),
                new KeyMappingData() {
                    Key = Key.C
                }
            ));
            keyActionChecker.DisableJobs.Add(new KeyActionDisableJob(
                new KeyActionDisableData(KeyActionId.NewId(), false),
                new KeyMappingData() {
                    Key = Key.C
                }
            ));

            var modifierKeyStatus = new ModifierKeyStatus();

            var expected1 = keyActionChecker.Find(true, Key.C, modifierKeyStatus, new KBDLLHOOKSTRUCT());
            Assert.Equal(1, expected1.Count);

            var expected2 = keyActionChecker.Find(true, Key.A, modifierKeyStatus, new KBDLLHOOKSTRUCT());
            Assert.Equal(1, expected2.Count);

            keyActionChecker.DisableJobs.Add(new KeyActionDisableJob(
                new KeyActionDisableData(KeyActionId.NewId(), false),
                new KeyMappingData() {
                    Key = Key.A
                }
            ));
            var expected3 = keyActionChecker.Find(true, Key.A, modifierKeyStatus, new KBDLLHOOKSTRUCT());
            Assert.Equal(1, expected3.Count);
            var job = (KeyActionReplaceJob)expected3.First();
            Assert.Equal(Key.B, keyActionChecker.ReplaceJobs[0].ActionData.ReplaceKey);
        }
        #endregion
    }
}
