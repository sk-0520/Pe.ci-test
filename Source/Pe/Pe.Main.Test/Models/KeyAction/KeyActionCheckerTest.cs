using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using ContentTypeTextNet.Pe.Main.Models.Logic;
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
                new KeyActionDisableData() {
                },
                new KeyMappingData() {
                    Key = Key.B
                }
            ));

            var modifierKeyStatus = new ModifierKeyStatus();

            var resultUpA = keyActionChecker.Find(false, Key.A, modifierKeyStatus);
            var resultUpB = keyActionChecker.Find(false, Key.B, modifierKeyStatus);
            var resultUpC = keyActionChecker.Find(false, Key.C, modifierKeyStatus);
            Assert.IsFalse(resultUpA.Any());
            Assert.IsFalse(resultUpB.Any());
            Assert.IsFalse(resultUpC.Any());

            var resultDownA = keyActionChecker.Find(true, Key.A, modifierKeyStatus);
            var resultDownB = keyActionChecker.Find(true, Key.B, modifierKeyStatus);
            var resultDownC = keyActionChecker.Find(true, Key.C, modifierKeyStatus);
            Assert.IsFalse(resultDownA.Any());
            Assert.IsTrue(resultDownB.Any());
            Assert.IsFalse(resultDownC.Any());

        }

        [TestMethod]
        public void FindTest_DisableToEnable()
        {
            var keyActionChecker = new KeyActionChecker(Test.LoggerFactory);
            keyActionChecker.DisableJobs.Add(new KeyActionDisableJob(
                new KeyActionDisableData() {
                },
                new KeyMappingData() {
                    Key = Key.A
                }
            ));

            var modifierKeyStatus = new ModifierKeyStatus();

            var result1 = keyActionChecker.Find(true, Key.A, modifierKeyStatus);
            keyActionChecker.KeyDisableToEnableTime = TimeSpan.MaxValue;
            var result2 = keyActionChecker.Find(true, Key.A, modifierKeyStatus);
            keyActionChecker.KeyDisableToEnableTime = TimeSpan.Zero;
            var result3 = keyActionChecker.Find(true, Key.A, modifierKeyStatus);
            Assert.IsTrue(result1.Any());
            Assert.IsFalse(result2.Any());
            Assert.IsTrue(result3.Any());
        }

        [TestMethod]
        public void FindTest_Rplace()
        {
            var keyActionChecker = new KeyActionChecker(Test.LoggerFactory);
            keyActionChecker.ReplaceJobs.Add(new KeyActionReplaceJob(
                new KeyActionReplaceData() {
                    ReplaceKey = Key.B,
                },
                new KeyMappingData() {
                    Key = Key.A
                }
            ));
            keyActionChecker.DisableJobs.Add(new KeyActionDisableJob(
                new KeyActionDisableData() {
                },
                new KeyMappingData() {
                    Key = Key.C
                }
            ));
            keyActionChecker.DisableJobs.Add(new KeyActionDisableJob(
                new KeyActionDisableData() {
                },
                new KeyMappingData() {
                    Key = Key.C
                }
            ));

            var modifierKeyStatus = new ModifierKeyStatus();

            var result1 = keyActionChecker.Find(true, Key.C, modifierKeyStatus);
            Assert.AreEqual(1, result1.Count);

            var result2 = keyActionChecker.Find(true, Key.A, modifierKeyStatus);
            Assert.AreEqual(1, result2.Count);

            keyActionChecker.DisableJobs.Add(new KeyActionDisableJob(
                new KeyActionDisableData() {
                },
                new KeyMappingData() {
                    Key = Key.A
                }
            ));
            var result3 = keyActionChecker.Find(true, Key.A, modifierKeyStatus);
            Assert.AreEqual(1, result3.Count);
            var job = (KeyActionReplaceJob)result3.First();
            Assert.AreEqual(Key.B, keyActionChecker.ReplaceJobs[0].ActionData.ReplaceKey);
        }
        #endregion
    }
}
