using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class FontsEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void Insert_Select_FontTest()
        {
            var test = Test.BuildDao<FontsEntityDao>(AccessorKind.Main);
            var expected = new {
                FontId = FontId.NewId(),
                Font = new FontData() {
                    FamilyName = "FONT",
                    IsBold = true,
                    IsItalic = true,
                    IsUnderline = true,
                    IsStrikeThrough = true,
                    Size = 123,
                }
            };
            test.InsertFont(expected.FontId, expected.Font, Test.DiContainer.Build<IDatabaseCommonStatus>());
            var actual = test.SelectFont(expected.FontId);
            Assert.Equal(expected.Font.FamilyName, actual.FamilyName);
            Assert.Equal(expected.Font.IsBold, actual.IsBold);
            Assert.Equal(expected.Font.IsItalic, actual.IsItalic);
            Assert.Equal(expected.Font.IsUnderline, actual.IsUnderline);
            Assert.Equal(expected.Font.IsStrikeThrough, actual.IsStrikeThrough);
            Assert.Equal(expected.Font.Size, actual.Size);
        }

        [Fact]
        public void InsertCopyFontTest()
        {
            var mainDatabaseAccessor = Test.DiContainer.Build<IMainDatabaseAccessor>();
            var test = Test.DiContainer.Build<FontsEntityDao>(mainDatabaseAccessor, mainDatabaseAccessor.DatabaseFactory.CreateImplementation());
            var srcFontId = mainDatabaseAccessor.QueryFirst<FontId>("select Fonts.FontId from Fonts");
            var expected = test.SelectFont(srcFontId);
            var dstFontId = FontId.NewId();
            test.InsertCopyFont(srcFontId, dstFontId, Test.DiContainer.Build<IDatabaseCommonStatus>());

            var actual = test.SelectFont(dstFontId);
            Assert.Equal(expected.FamilyName, actual.FamilyName);
            Assert.Equal(expected.IsBold, actual.IsBold);
            Assert.Equal(expected.IsItalic, actual.IsItalic);
            Assert.Equal(expected.IsUnderline, actual.IsUnderline);
            Assert.Equal(expected.IsStrikeThrough, actual.IsStrikeThrough);
            Assert.Equal(expected.Size, actual.Size);
        }

        [Fact]
        public void UpdateFamilyNameTest()
        {
            var test = Test.BuildDao<FontsEntityDao>(AccessorKind.Main);
            var expected = new {
                FontId = FontId.NewId(),
                Font = new FontData() {
                    FamilyName = "FONT",
                    IsBold = false,
                    IsItalic = false,
                    IsUnderline = false,
                    IsStrikeThrough = false,
                    Size = 123,
                }
            };
            test.InsertFont(expected.FontId, expected.Font, Test.DiContainer.Build<IDatabaseCommonStatus>());
            test.UpdateFamilyName(expected.FontId, "UpdateFamilyName", Test.DiContainer.Build<IDatabaseCommonStatus>());
            var actual = test.SelectFont(expected.FontId);
            Assert.Equal("UpdateFamilyName", actual.FamilyName);
            Assert.Equal(expected.Font.IsBold, actual.IsBold);
            Assert.Equal(expected.Font.IsItalic, actual.IsItalic);
            Assert.Equal(expected.Font.IsUnderline, actual.IsUnderline);
            Assert.Equal(expected.Font.IsStrikeThrough, actual.IsStrikeThrough);
            Assert.Equal(expected.Font.Size, actual.Size);
        }

        [Fact]
        public void UpdateBoldTest()
        {
            var test = Test.BuildDao<FontsEntityDao>(AccessorKind.Main);
            var expected = new {
                FontId = FontId.NewId(),
                Font = new FontData() {
                    FamilyName = "FONT",
                    IsBold = false,
                    IsItalic = false,
                    IsUnderline = false,
                    IsStrikeThrough = false,
                    Size = 123,
                }
            };
            test.InsertFont(expected.FontId, expected.Font, Test.DiContainer.Build<IDatabaseCommonStatus>());
            test.UpdateBold(expected.FontId, true, Test.DiContainer.Build<IDatabaseCommonStatus>());
            var actual = test.SelectFont(expected.FontId);
            Assert.Equal(expected.Font.FamilyName, actual.FamilyName);
            Assert.True(actual.IsBold);
            Assert.Equal(expected.Font.IsItalic, actual.IsItalic);
            Assert.Equal(expected.Font.IsUnderline, actual.IsUnderline);
            Assert.Equal(expected.Font.IsStrikeThrough, actual.IsStrikeThrough);
            Assert.Equal(expected.Font.Size, actual.Size);
        }

        [Fact]
        public void UpdateItalicTest()
        {
            var test = Test.BuildDao<FontsEntityDao>(AccessorKind.Main);
            var expected = new {
                FontId = FontId.NewId(),
                Font = new FontData() {
                    FamilyName = "FONT",
                    IsBold = false,
                    IsItalic = false,
                    IsUnderline = false,
                    IsStrikeThrough = false,
                    Size = 123,
                }
            };
            test.InsertFont(expected.FontId, expected.Font, Test.DiContainer.Build<IDatabaseCommonStatus>());
            test.UpdateItalic(expected.FontId, true, Test.DiContainer.Build<IDatabaseCommonStatus>());
            var actual = test.SelectFont(expected.FontId);
            Assert.Equal(expected.Font.FamilyName, actual.FamilyName);
            Assert.Equal(expected.Font.IsBold, actual.IsBold);
            Assert.True(actual.IsItalic);
            Assert.Equal(expected.Font.IsUnderline, actual.IsUnderline);
            Assert.Equal(expected.Font.IsStrikeThrough, actual.IsStrikeThrough);
            Assert.Equal(expected.Font.Size, actual.Size);
        }

        [Fact]
        public void UpdateHeightTest()
        {
            var test = Test.BuildDao<FontsEntityDao>(AccessorKind.Main);
            var expected = new {
                FontId = FontId.NewId(),
                Font = new FontData() {
                    FamilyName = "FONT",
                    IsBold = false,
                    IsItalic = false,
                    IsUnderline = false,
                    IsStrikeThrough = false,
                    Size = 123,
                }
            };
            test.InsertFont(expected.FontId, expected.Font, Test.DiContainer.Build<IDatabaseCommonStatus>());
            test.UpdateHeight(expected.FontId, 456, Test.DiContainer.Build<IDatabaseCommonStatus>());
            var actual = test.SelectFont(expected.FontId);
            Assert.Equal(expected.Font.FamilyName, actual.FamilyName);
            Assert.Equal(expected.Font.IsBold, actual.IsBold);
            Assert.Equal(expected.Font.IsItalic, actual.IsItalic);
            Assert.Equal(expected.Font.IsUnderline, actual.IsUnderline);
            Assert.Equal(expected.Font.IsStrikeThrough, actual.IsStrikeThrough);
            Assert.Equal(456, actual.Size);
        }

        [Fact]
        public void UpdateFontTest()
        {
            static void AssertEquals(IFont expected, IFont actual) {
                Assert.Equal(expected.FamilyName, actual.FamilyName);
                Assert.Equal(expected.IsBold, actual.IsBold);
                Assert.Equal(expected.IsItalic, actual.IsItalic);
                Assert.Equal(expected.IsUnderline, actual.IsUnderline);
                Assert.Equal(expected.IsStrikeThrough, actual.IsStrikeThrough);
                Assert.Equal(expected.Size, actual.Size);
            }

            var test = Test.BuildDao<FontsEntityDao>(AccessorKind.Main);
            var fontId = FontId.NewId();
            var input = new FontData() {
                FamilyName = "FONT",
                IsBold = false,
                IsItalic = false,
                IsUnderline = false,
                IsStrikeThrough = false,
                Size = 123,
            };
            test.InsertFont(fontId, input, Test.DiContainer.Build<IDatabaseCommonStatus>());

            input.FamilyName = "FamilyName";
            test.UpdateFont(fontId, input, Test.DiContainer.Build<IDatabaseCommonStatus>());
            var actualFamilyName = test.SelectFont(fontId);
            AssertEquals(input, actualFamilyName);

            input.IsBold = !input.IsBold;
            test.UpdateFont(fontId, input, Test.DiContainer.Build<IDatabaseCommonStatus>());
            var actualIsBold = test.SelectFont(fontId);
            AssertEquals(input, actualIsBold);

            input.IsItalic = !input.IsItalic;
            test.UpdateFont(fontId, input, Test.DiContainer.Build<IDatabaseCommonStatus>());
            var actualIsItalic = test.SelectFont(fontId);
            AssertEquals(input, actualIsItalic);

            input.IsUnderline = !input.IsUnderline;
            test.UpdateFont(fontId, input, Test.DiContainer.Build<IDatabaseCommonStatus>());
            var actualIsUnderline = test.SelectFont(fontId);
            AssertEquals(input, actualIsUnderline);

            input.IsStrikeThrough = !input.IsStrikeThrough;
            test.UpdateFont(fontId, input, Test.DiContainer.Build<IDatabaseCommonStatus>());
            var actualIsStrikeThrough = test.SelectFont(fontId);
            AssertEquals(input, actualIsStrikeThrough);

            input.Size = input.Size * 2;
            test.UpdateFont(fontId, input, Test.DiContainer.Build<IDatabaseCommonStatus>());
            var actualSize = test.SelectFont(fontId);
            AssertEquals(input, actualSize);
        }

        #endregion
    }
}
