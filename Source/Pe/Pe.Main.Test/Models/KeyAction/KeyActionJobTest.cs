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
                : base(new KeyActionCommonData(), new[] { new KeyMappingData() })
            { }

            public bool PublicTestMapping(IReadOnlyKeyMappingData mapping, bool isDown, Key key, in ModifierKeyStatus modifierKeyStatus)
            {
                return TestMapping(mapping, isDown, key, modifierKeyStatus);
            }

            public override void Reset() => throw new NotImplementedException();
            public override bool Check(bool isDown, Key key, in ModifierKeyStatus modifierKeyStatus) => throw new NotImplementedException();
        }
        #region function

        [TestMethod]
        public void TestMappingTest_None()
        {
            var job = new PublicKeyActionJob();

            var mapping = new KeyMappingData() {
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

            var mapping = new KeyMappingData() {
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

            var mapping = new KeyMappingData() {
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

            var mapping = new KeyMappingData() {
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

            var mapping = new KeyMappingData() {
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

            var mapping = new KeyMappingData() {
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

    [TestClass]
    public class KeyActionReplaceJobTest
    {
        #region function

        [TestMethod]
        public void Constructor_Test()
        {
            Assert.ThrowsException<ArgumentException>(() => new KeyActionReplaceJob(new KeyActionReplaceData(), new KeyMappingData()));
            Assert.ThrowsException<ArgumentException>(() => new KeyActionReplaceJob(new KeyActionReplaceData(), new KeyMappingData() { Key = Key.A }));
            Assert.ThrowsException<ArgumentException>(() => new KeyActionReplaceJob(new KeyActionReplaceData() { ReplaceKey = Key.A }, new KeyMappingData() { Key = Key.A }));
            new KeyActionReplaceJob(new KeyActionReplaceData() { ReplaceKey = Key.B }, new KeyMappingData() { Key = Key.A });
            Assert.IsTrue(true);
        }

        #endregion
    }

    [TestClass]
    public class KeyActionDisableJobTest
    {
        #region function

        [TestMethod]
        public void Constructor_Test()
        {
            Assert.ThrowsException<ArgumentException>(() => new KeyActionDisableJob(new KeyActionDisableData(), new KeyMappingData()));
            Assert.ThrowsException<ArgumentException>(() => new KeyActionDisableJob(new KeyActionDisableData(), new KeyMappingData() { }));
            new KeyActionDisableJob(new KeyActionDisableData(), new KeyMappingData() { Shift = ModifierKey.Left });
            new KeyActionDisableJob(new KeyActionDisableData(), new KeyMappingData() { Shift = ModifierKey.Right });
            new KeyActionDisableJob(new KeyActionDisableData(), new KeyMappingData() { Shift = ModifierKey.Any });
            new KeyActionDisableJob(new KeyActionDisableData(), new KeyMappingData() { Control = ModifierKey.All });
            new KeyActionDisableJob(new KeyActionDisableData(), new KeyMappingData() { Control = ModifierKey.Left });
            new KeyActionDisableJob(new KeyActionDisableData(), new KeyMappingData() { Control = ModifierKey.Right });
            new KeyActionDisableJob(new KeyActionDisableData(), new KeyMappingData() { Control = ModifierKey.Any });
            new KeyActionDisableJob(new KeyActionDisableData(), new KeyMappingData() { Control = ModifierKey.All });
            new KeyActionDisableJob(new KeyActionDisableData(), new KeyMappingData() { Alt = ModifierKey.All });
            new KeyActionDisableJob(new KeyActionDisableData(), new KeyMappingData() { Alt = ModifierKey.Left });
            new KeyActionDisableJob(new KeyActionDisableData(), new KeyMappingData() { Alt = ModifierKey.Right });
            new KeyActionDisableJob(new KeyActionDisableData(), new KeyMappingData() { Alt = ModifierKey.Any });
            new KeyActionDisableJob(new KeyActionDisableData(), new KeyMappingData() { Alt = ModifierKey.All });
            new KeyActionDisableJob(new KeyActionDisableData(), new KeyMappingData() { Super = ModifierKey.All });
            new KeyActionDisableJob(new KeyActionDisableData(), new KeyMappingData() { Super = ModifierKey.Left });
            new KeyActionDisableJob(new KeyActionDisableData(), new KeyMappingData() { Super = ModifierKey.Right });
            new KeyActionDisableJob(new KeyActionDisableData(), new KeyMappingData() { Super = ModifierKey.Any });
            new KeyActionDisableJob(new KeyActionDisableData(), new KeyMappingData() { Super = ModifierKey.All });
            new KeyActionDisableJob(new KeyActionDisableData(), new KeyMappingData() { Key = Key.A });
            Assert.IsTrue(true);
        }

        #endregion
    }

    [TestClass]
    public class KeyActionPressJobTest
    {
        #region function

        [TestMethod]
        public void Constructor_Test()
        {
            Assert.ThrowsException<ArgumentException>(() => new KeyActionPressJob(new KeyActionPressedData(), new[] {
                new KeyMappingData(),
            }));
            Assert.ThrowsException<ArgumentException>(() => new KeyActionPressJob(new KeyActionPressedData(), new[] {
                new KeyMappingData(){ Shift = ModifierKey.Left },
            }));
            Assert.ThrowsException<ArgumentException>(() => new KeyActionPressJob(new KeyActionPressedData(), new[] {
                new KeyMappingData(){ Shift = ModifierKey.Left },
                new KeyMappingData(){ Key = Key.A },
                new KeyMappingData(){ Shift = ModifierKey.Right },
            }));

            new KeyActionPressJob(new KeyActionPressedData(), new[] {
                new KeyMappingData(){ Key = Key.A },
            });
            new KeyActionPressJob(new KeyActionPressedData(), new[] {
                new KeyMappingData(){ Key = Key.A },
                new KeyMappingData(){ Key = Key.A },
            });
            new KeyActionPressJob(new KeyActionPressedData(), new[] {
                new KeyMappingData(){ Key = Key.A },
                new KeyMappingData(){ Key = Key.B },
                new KeyMappingData(){ Key = Key.C },
            });
        }

        [TestMethod]
        public void CheckTest_Simple()
        {
            var job = new KeyActionPressJob(new KeyActionPressedData(), new[] {
                new KeyMappingData(){ Key = Key.A },
            });
            Assert.IsFalse(job.NextWaiting);

            var result1 = job.Check(true, Key.B, new ModifierKeyStatus());
            Assert.IsFalse(result1);
            Assert.IsFalse(job.NextWaiting);

            var result2 = job.Check(true, Key.None, new ModifierKeyStatus());
            Assert.IsFalse(result2);
            Assert.IsFalse(job.NextWaiting);

            var result3 = job.Check(true, Key.LeftShift, new ModifierKeyStatus());
            Assert.IsFalse(result3);
            Assert.IsFalse(job.NextWaiting);

            var result4 = job.Check(true, Key.A, new ModifierKeyStatus());
            Assert.IsTrue(result4);
            Assert.IsTrue(job.IsHit);
            Assert.IsFalse(job.NextWaiting);

            var result5 = job.Check(true, Key.B, new ModifierKeyStatus());
            Assert.IsFalse(result5);
            Assert.IsFalse(job.IsHit);
            Assert.IsFalse(job.NextWaiting);

            var result6 = job.Check(true, Key.A, new ModifierKeyStatus());
            Assert.IsTrue(result6);
            Assert.IsTrue(job.IsHit);
            Assert.IsFalse(job.NextWaiting);
            job.Reset();
            Assert.IsFalse(job.IsHit);
        }

        [TestMethod]
        public void CheckTest_Multi1()
        {
            var job = new KeyActionPressJob(new KeyActionPressedData(), new[] {
                new KeyMappingData(){ Key = Key.A },
                new KeyMappingData(){ Key = Key.B },
                new KeyMappingData(){ Key = Key.C },
            });
            Assert.IsFalse(job.NextWaiting);

            var result1 = job.Check(true, Key.A, new ModifierKeyStatus());
            Assert.IsTrue(result1);
            Assert.IsFalse(job.IsHit);
            Assert.IsTrue(job.NextWaiting);

            var result2 = job.Check(true, Key.B, new ModifierKeyStatus());
            Assert.IsTrue(result2);
            Assert.IsFalse(job.IsHit);
            Assert.IsTrue(job.NextWaiting);

            var result3 = job.Check(true, Key.C, new ModifierKeyStatus());
            Assert.IsTrue(result3);
            Assert.IsTrue(job.IsHit);
            Assert.IsFalse(job.NextWaiting);
        }

        [TestMethod]
        public void CheckTest_Multi2()
        {
            var job = new KeyActionPressJob(new KeyActionPressedData(), new[] {
                new KeyMappingData(){ Key = Key.A },
                new KeyMappingData(){ Key = Key.B },
                new KeyMappingData(){ Key = Key.C },
            });
            Assert.IsFalse(job.NextWaiting);

            var result1 = job.Check(true, Key.A, new ModifierKeyStatus());
            Assert.IsTrue(result1);
            Assert.IsFalse(job.IsHit);
            Assert.IsTrue(job.NextWaiting);

            var result2 = job.Check(true, Key.LeftShift, new ModifierKeyStatus());
            Assert.IsFalse(result2);
            Assert.IsFalse(job.IsHit);
            Assert.IsTrue(job.NextWaiting);

            var result3 = job.Check(true, Key.B, new ModifierKeyStatus());
            Assert.IsTrue(result3);
            Assert.IsFalse(job.IsHit);
            Assert.IsTrue(job.NextWaiting);

            var result4 = job.Check(true, Key.RightShift, new ModifierKeyStatus());
            Assert.IsFalse(result4);
            Assert.IsFalse(job.IsHit);
            Assert.IsTrue(job.NextWaiting);

            var result5 = job.Check(true, Key.C, new ModifierKeyStatus());
            Assert.IsTrue(result5);
            Assert.IsTrue(job.IsHit);
            Assert.IsFalse(job.NextWaiting);
        }

        [TestMethod]
        public void CheckTest_Multi3()
        {
            var job = new KeyActionPressJob(new KeyActionPressedData(), new[] {
                new KeyMappingData(){ Key = Key.A },
                new KeyMappingData(){ Key = Key.B },
                new KeyMappingData(){ Key = Key.C },
            });
            Assert.IsFalse(job.NextWaiting);

            var result1 = job.Check(true, Key.B, new ModifierKeyStatus());
            Assert.IsFalse(result1);
            Assert.IsFalse(job.IsHit);
            Assert.IsFalse(job.NextWaiting);

            var result2 = job.Check(true, Key.A, new ModifierKeyStatus());
            Assert.IsTrue(result2);
            Assert.IsFalse(job.IsHit);
            Assert.IsTrue(job.NextWaiting);

            var result3 = job.Check(true, Key.C, new ModifierKeyStatus());
            Assert.IsFalse(result3);
            Assert.IsFalse(job.IsHit);
            Assert.IsFalse(job.NextWaiting);

            var result4 = job.Check(true, Key.A, new ModifierKeyStatus());
            Assert.IsTrue(result4);
            Assert.IsFalse(job.IsHit);
            Assert.IsTrue(job.NextWaiting);

            var result5 = job.Check(true, Key.B, new ModifierKeyStatus());
            Assert.IsTrue(result5);
            Assert.IsFalse(job.IsHit);
            Assert.IsTrue(job.NextWaiting);

            var result6 = job.Check(true, Key.C, new ModifierKeyStatus());
            Assert.IsTrue(result6);
            Assert.IsTrue(job.IsHit);
            Assert.IsFalse(job.NextWaiting);
        }

        #endregion
    }


}
