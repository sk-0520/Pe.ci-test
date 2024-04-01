using System;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.KeyAction
{
    public class KeyActionJobTest
    {
        class PublicKeyActionJob: KeyActionJobBase
        {
            public PublicKeyActionJob()
                : base(new KeyActionCommonData(KeyActionId.NewId(), KeyActionKind.Replace), new[] { new KeyMappingData() })
            { }

            public bool PublicTestMapping(IReadOnlyKeyMappingData mapping, bool isDown, Key key, in ModifierKeyStatus modifierKeyStatus)
            {
                return TestMapping(mapping, isDown, key, modifierKeyStatus);
            }

            public override void Reset() => throw new NotImplementedException();
            public override bool Check(bool isDown, Key key, in ModifierKeyStatus modifierKeyStatus) => throw new NotImplementedException();
        }
        #region function

        [Fact]
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
            Assert.False(actual);
        }

        [Theory]
        [InlineData(false, Key.None, Key.A)]
        [InlineData(false, Key.A, Key.None)]
        [InlineData(false, Key.A, Key.B)]
        [InlineData(true, Key.None, Key.None)] // 実運用上ないけどデータ的には正
        [InlineData(true, Key.A, Key.A)]
        public void TestMappingTest_KeyOnly(bool expected, Key mapKey, Key inputKey)
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
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(true, ModifierKey.None, false, false)]
        [InlineData(false, ModifierKey.Left, false, false)]
        [InlineData(true, ModifierKey.Left, true, false)]
        [InlineData(false, ModifierKey.Left, true, true)]
        [InlineData(false, ModifierKey.Left, false, true)]
        public void TestMappingTest_ModShift(bool expected, ModifierKey mapMod, bool inputLeft, bool inputRight)
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
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(true, ModifierKey.None, false, false)]
        [InlineData(false, ModifierKey.Left, false, false)]
        [InlineData(true, ModifierKey.Left, true, false)]
        [InlineData(false, ModifierKey.Left, true, true)]
        [InlineData(false, ModifierKey.Left, false, true)]
        public void TestMappingTest_ModControl(bool expected, ModifierKey mapMod, bool inputLeft, bool inputRight)
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
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(true, ModifierKey.None, false, false)]
        [InlineData(false, ModifierKey.Left, false, false)]
        [InlineData(true, ModifierKey.Left, true, false)]
        [InlineData(false, ModifierKey.Left, true, true)]
        [InlineData(false, ModifierKey.Left, false, true)]
        public void TestMappingTest_ModAlt(bool expected, ModifierKey mapMod, bool inputLeft, bool inputRight)
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
            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData(true, ModifierKey.None, false, false)]
        [InlineData(false, ModifierKey.Left, false, false)]
        [InlineData(true, ModifierKey.Left, true, false)]
        [InlineData(false, ModifierKey.Left, true, true)]
        [InlineData(false, ModifierKey.Left, false, true)]
        public void TestMappingTest_ModSuper(bool expected, ModifierKey mapMod, bool inputLeft, bool inputRight)
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
            Assert.Equal(expected, actual);
        }

        #endregion
    }

    public class KeyActionReplaceJobTest
    {
        #region function

        [Fact]
        public void Constructor_Test()
        {
            Assert.Throws<ArgumentException>(() => new KeyActionReplaceJob(new KeyActionReplaceData(KeyActionId.NewId(), Key.None), new KeyMappingData()));
            Assert.Throws<ArgumentException>(() => new KeyActionReplaceJob(new KeyActionReplaceData(KeyActionId.NewId(), Key.A), new KeyMappingData() { Key = Key.A }));
            Assert.Throws<ArgumentException>(() => new KeyActionReplaceJob(new KeyActionReplaceData(KeyActionId.NewId(), Key.None), new KeyMappingData()));
            Assert.Throws<ArgumentException>(() => new KeyActionReplaceJob(new KeyActionReplaceData(KeyActionId.NewId(), Key.A), new KeyMappingData() { Key = Key.A }));
            new KeyActionReplaceJob(new KeyActionReplaceData(KeyActionId.NewId(), Key.B), new KeyMappingData() { Key = Key.A });
            Assert.True(true);
        }

        #endregion
    }

    public class KeyActionDisableJobTest
    {
        #region function

        [Fact]
        public void Constructor_Test()
        {
            Assert.Throws<ArgumentException>(() => new KeyActionDisableJob(new KeyActionDisableData(KeyActionId.NewId(), false), new KeyMappingData()));
            Assert.Throws<ArgumentException>(() => new KeyActionDisableJob(new KeyActionDisableData(KeyActionId.NewId(), false), new KeyMappingData() { }));
            new KeyActionDisableJob(new KeyActionDisableData(KeyActionId.NewId(), false), new KeyMappingData() { Shift = ModifierKey.Left });
            new KeyActionDisableJob(new KeyActionDisableData(KeyActionId.NewId(), false), new KeyMappingData() { Shift = ModifierKey.Right });
            new KeyActionDisableJob(new KeyActionDisableData(KeyActionId.NewId(), false), new KeyMappingData() { Shift = ModifierKey.Any });
            new KeyActionDisableJob(new KeyActionDisableData(KeyActionId.NewId(), false), new KeyMappingData() { Control = ModifierKey.All });
            new KeyActionDisableJob(new KeyActionDisableData(KeyActionId.NewId(), false), new KeyMappingData() { Control = ModifierKey.Left });
            new KeyActionDisableJob(new KeyActionDisableData(KeyActionId.NewId(), false), new KeyMappingData() { Control = ModifierKey.Right });
            new KeyActionDisableJob(new KeyActionDisableData(KeyActionId.NewId(), false), new KeyMappingData() { Control = ModifierKey.Any });
            new KeyActionDisableJob(new KeyActionDisableData(KeyActionId.NewId(), false), new KeyMappingData() { Control = ModifierKey.All });
            new KeyActionDisableJob(new KeyActionDisableData(KeyActionId.NewId(), false), new KeyMappingData() { Alt = ModifierKey.All });
            new KeyActionDisableJob(new KeyActionDisableData(KeyActionId.NewId(), false), new KeyMappingData() { Alt = ModifierKey.Left });
            new KeyActionDisableJob(new KeyActionDisableData(KeyActionId.NewId(), false), new KeyMappingData() { Alt = ModifierKey.Right });
            new KeyActionDisableJob(new KeyActionDisableData(KeyActionId.NewId(), false), new KeyMappingData() { Alt = ModifierKey.Any });
            new KeyActionDisableJob(new KeyActionDisableData(KeyActionId.NewId(), false), new KeyMappingData() { Alt = ModifierKey.All });
            new KeyActionDisableJob(new KeyActionDisableData(KeyActionId.NewId(), false), new KeyMappingData() { Super = ModifierKey.All });
            new KeyActionDisableJob(new KeyActionDisableData(KeyActionId.NewId(), false), new KeyMappingData() { Super = ModifierKey.Left });
            new KeyActionDisableJob(new KeyActionDisableData(KeyActionId.NewId(), false), new KeyMappingData() { Super = ModifierKey.Right });
            new KeyActionDisableJob(new KeyActionDisableData(KeyActionId.NewId(), false), new KeyMappingData() { Super = ModifierKey.Any });
            new KeyActionDisableJob(new KeyActionDisableData(KeyActionId.NewId(), false), new KeyMappingData() { Super = ModifierKey.All });
            new KeyActionDisableJob(new KeyActionDisableData(KeyActionId.NewId(), false), new KeyMappingData() { Key = Key.A });
            Assert.True(true);
        }

        #endregion
    }

    public class KeyActionPressJobTest
    {
        #region define

        class KeyActionPressedData: KeyActionPressedDataBase
        {
            public KeyActionPressedData()
                : base(KeyActionId.NewId(), KeyActionKind.Replace)
            { }
        }

        #endregion

        #region function

        [Fact]
        public void Constructor_Test()
        {
            Assert.Throws<ArgumentException>(() => new KeyActionPressJob(new KeyActionPressedData(), new[] {
                new KeyMappingData(),
            }));
            Assert.Throws<ArgumentException>(() => new KeyActionPressJob(new KeyActionPressedData(), new[] {
                new KeyMappingData(){ Shift = ModifierKey.Left },
            }));
            Assert.Throws<ArgumentException>(() => new KeyActionPressJob(new KeyActionPressedData(), new[] {
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

        [Fact]
        public void CheckTest_Simple()
        {
            var job = new KeyActionPressJob(new KeyActionPressedData(), new[] {
                new KeyMappingData(){ Key = Key.A },
            });
            Assert.False(job.NextWaiting);

            var expected1 = job.Check(true, Key.B, new ModifierKeyStatus());
            Assert.False(expected1);
            Assert.False(job.NextWaiting);

            var expected2 = job.Check(true, Key.None, new ModifierKeyStatus());
            Assert.False(expected2);
            Assert.False(job.NextWaiting);

            var expected3 = job.Check(true, Key.LeftShift, new ModifierKeyStatus());
            Assert.False(expected3);
            Assert.False(job.NextWaiting);

            var expected4 = job.Check(true, Key.A, new ModifierKeyStatus());
            Assert.True(expected4);
            Assert.True(job.IsAllHit);
            Assert.False(job.NextWaiting);

            var expected5 = job.Check(true, Key.B, new ModifierKeyStatus());
            Assert.False(expected5);
            Assert.False(job.IsAllHit);
            Assert.False(job.NextWaiting);

            var expected6 = job.Check(true, Key.A, new ModifierKeyStatus());
            Assert.True(expected6);
            Assert.True(job.IsAllHit);
            Assert.False(job.NextWaiting);
            job.Reset();
            Assert.False(job.IsAllHit);
        }

        [Fact]
        public void CheckTest_Multi1()
        {
            var job = new KeyActionPressJob(new KeyActionPressedData(), new[] {
                new KeyMappingData(){ Key = Key.A },
                new KeyMappingData(){ Key = Key.B },
                new KeyMappingData(){ Key = Key.C },
            });
            Assert.False(job.NextWaiting);

            var expected1 = job.Check(true, Key.A, new ModifierKeyStatus());
            Assert.True(expected1);
            Assert.False(job.IsAllHit);
            Assert.True(job.NextWaiting);

            var expected2 = job.Check(true, Key.B, new ModifierKeyStatus());
            Assert.True(expected2);
            Assert.False(job.IsAllHit);
            Assert.True(job.NextWaiting);

            var expected3 = job.Check(true, Key.C, new ModifierKeyStatus());
            Assert.True(expected3);
            Assert.True(job.IsAllHit);
            Assert.False(job.NextWaiting);
        }

        [Fact]
        public void CheckTest_Multi2()
        {
            var job = new KeyActionPressJob(new KeyActionPressedData(), new[] {
                new KeyMappingData(){ Key = Key.A },
                new KeyMappingData(){ Key = Key.B },
                new KeyMappingData(){ Key = Key.C },
            });
            Assert.False(job.NextWaiting);

            var expected1 = job.Check(true, Key.A, new ModifierKeyStatus());
            Assert.True(expected1);
            Assert.False(job.IsAllHit);
            Assert.True(job.NextWaiting);

            var expected2 = job.Check(true, Key.LeftShift, new ModifierKeyStatus());
            Assert.False(expected2);
            Assert.False(job.IsAllHit);
            Assert.True(job.NextWaiting);

            var expected3 = job.Check(true, Key.B, new ModifierKeyStatus());
            Assert.True(expected3);
            Assert.False(job.IsAllHit);
            Assert.True(job.NextWaiting);

            var expected4 = job.Check(true, Key.RightShift, new ModifierKeyStatus());
            Assert.False(expected4);
            Assert.False(job.IsAllHit);
            Assert.True(job.NextWaiting);

            var expected5 = job.Check(true, Key.C, new ModifierKeyStatus());
            Assert.True(expected5);
            Assert.True(job.IsAllHit);
            Assert.False(job.NextWaiting);
        }

        [Fact]
        public void CheckTest_Multi3()
        {
            var job = new KeyActionPressJob(new KeyActionPressedData(), new[] {
                new KeyMappingData(){ Key = Key.A },
                new KeyMappingData(){ Key = Key.B },
                new KeyMappingData(){ Key = Key.C },
            });
            Assert.False(job.NextWaiting);

            var expected1 = job.Check(true, Key.B, new ModifierKeyStatus());
            Assert.False(expected1);
            Assert.False(job.IsAllHit);
            Assert.False(job.NextWaiting);

            var expected2 = job.Check(true, Key.A, new ModifierKeyStatus());
            Assert.True(expected2);
            Assert.False(job.IsAllHit);
            Assert.True(job.NextWaiting);

            var expected3 = job.Check(true, Key.C, new ModifierKeyStatus());
            Assert.False(expected3);
            Assert.False(job.IsAllHit);
            Assert.False(job.NextWaiting);

            var expected4 = job.Check(true, Key.A, new ModifierKeyStatus());
            Assert.True(expected4);
            Assert.False(job.IsAllHit);
            Assert.True(job.NextWaiting);

            var expected5 = job.Check(true, Key.B, new ModifierKeyStatus());
            Assert.True(expected5);
            Assert.False(job.IsAllHit);
            Assert.True(job.NextWaiting);

            var expected6 = job.Check(true, Key.C, new ModifierKeyStatus());
            Assert.True(expected6);
            Assert.True(job.IsAllHit);
            Assert.False(job.NextWaiting);
        }

        #endregion
    }


}
