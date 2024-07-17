using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Compatibility.Windows
{
    public class PodStructUtilityTest
    {
        #region function

        public static TheoryData<RECT, Rect> Convert_input_Rect_Data => new() {
            { new RECT(), new Rect() },
            { new RECT(1, 2, 3 + 1, 4 + 2), new Rect(1, 2, 3, 4) },
            { new RECT(1, 2, 3 + 1, 4 + 2), new Rect(1.1, 2.1, 3.1, 4.1) },
            { new RECT(1, 2, 3 + 1, 4 + 2), new Rect(1.9, 2.9, 3.9, 4.9) },
        };
        [Theory]
        [MemberData(nameof(Convert_input_Rect_Data))]
        public void Convert_input_Rect_Test(RECT expected, Rect input)
        {
            var actual = PodStructUtility.Convert(input);
            Assert.Equal(expected, actual);
        }

        public static TheoryData<Rect, RECT> Convert_input_RECT_Data => new() {
            { new Rect(), new RECT() },
            { new Rect(1, 2, 3, 4), new RECT(1, 2, 3 + 1, 4 + 2) },
        };
        [Theory]
        [MemberData(nameof(Convert_input_RECT_Data))]
        public void Convert_input_RECT_Test(Rect expected, RECT input)
        {
            var actual = PodStructUtility.Convert(input);
            Assert.Equal(expected, actual);
        }

        public static TheoryData<SIZE, Size> Convert_input_Size_Data => new() {
            { new SIZE(), new Size() },
            { new SIZE(1, 2), new Size(1, 2) },
            { new SIZE(1, 2), new Size(1.1, 2.1) },
            { new SIZE(1, 2), new Size(1.9, 2.9) },
        };
        [Theory]
        [MemberData(nameof(Convert_input_Size_Data))]
        public void Convert_input_Size_Test(SIZE expected, Size input)
        {
            var actual = PodStructUtility.Convert(input);
            Assert.Equal(expected, actual);
        }

        public static TheoryData<Size, SIZE> Convert_input_SIZE_Data => new() {
            { new Size(), new SIZE() },
            { new Size(1, 2), new SIZE(1, 2) },
        };
        [Theory]
        [MemberData(nameof(Convert_input_SIZE_Data))]
        public void Convert_input_SIZE_Test(Size expected, SIZE input)
        {
            var actual = PodStructUtility.Convert(input);
            Assert.Equal(expected, actual);
        }

        public static TheoryData<POINT, Point> Convert_input_Point_Data => new() {
            { new POINT(), new Point() },
            { new POINT(1, 2), new Point(1, 2) },
            { new POINT(1, 2), new Point(1.1, 2.1) },
            { new POINT(1, 2), new Point(1.9, 2.9) },
        };
        [Theory]
        [MemberData(nameof(Convert_input_Point_Data))]
        public void Convert_input_Point_Test(POINT expected, Point input)
        {
            var actual = PodStructUtility.Convert(input);
            Assert.Equal(expected, actual);
        }

        public static TheoryData<Point, POINT> Convert_input_POINT_Data => new() {
            { new Point(), new POINT() },
            { new Point(1, 2), new POINT(1, 2) },
        };
        [Theory]
        [MemberData(nameof(Convert_input_POINT_Data))]
        public void Convert_input_POINT_Test(Point expected, POINT input)
        {
            var actual = PodStructUtility.Convert(input);
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
