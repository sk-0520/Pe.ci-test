/*
This file is part of PInvoke.

PInvoke is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

PInvoke is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with PInvoke.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Runtime.InteropServices;

namespace ContentTypeTextNet.Library.PInvoke.Windows
{
    public enum MAX
    {
        MAX_PATH = 260,
    }
    /// <summary>
    /// http://www.pinvoke.net/default.aspx/Structures/rect.html
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left, Top, Right, Bottom;

        public RECT(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        //public RECT(System.Drawing.Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }

        public int X
        {
            get { return Left; }
            set { Right -= (Left - value); Left = value; }
        }

        public int Y
        {
            get { return Top; }
            set { Bottom -= (Top - value); Top = value; }
        }

        public int Height
        {
            get { return Bottom - Top; }
            set { Bottom = value + Top; }
        }

        public int Width
        {
            get { return Right - Left; }
            set { Right = value + Left; }
        }

        //public System.Drawing.Point Location
        //{
        //	get { return new System.Drawing.Point(Left, Top); }
        //	set { X = value.X; Y = value.Y; }
        //}

        //public System.Drawing.Size Size
        //{
        //	get { return new System.Drawing.Size(Width, Height); }
        //	set { Width = value.Width; Height = value.Height; }
        //}

        //public static implicit operator System.Drawing.Rectangle(RECT r)
        //{
        //	return new System.Drawing.Rectangle(r.Left, r.Top, r.Width, r.Height);
        //}

        //public static implicit operator RECT(System.Drawing.Rectangle r)
        //{
        //	return new RECT(r);
        //}

        //public static bool operator ==(RECT r1, RECT r2)
        //{
        //	return r1.Equals(r2);
        //}

        //public static bool operator !=(RECT r1, RECT r2)
        //{
        //	return !r1.Equals(r2);
        //}

        //public bool Equals(RECT r)
        //{
        //	return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
        //}

        //public override bool Equals(object obj)
        //{
        //	if (obj is RECT)
        //		return Equals((RECT)obj);
        //	else if (obj is System.Drawing.Rectangle)
        //		return Equals(new RECT((System.Drawing.Rectangle)obj));
        //	return false;
        //}

        //public override int GetHashCode()
        //{
        //	return ((System.Drawing.Rectangle)this).GetHashCode();
        //}

        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
        }
    }

    /// <summary>
    /// http://pinvoke.net/default.aspx/Structures.POINT
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;

        public POINT(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        //public POINT(System.Drawing.Point pt) : this(pt.X, pt.Y) { }

        //public static implicit operator System.Drawing.Point(POINT p)
        //{
        //	return new System.Drawing.Point(p.X, p.Y);
        //}

        //public static implicit operator POINT(System.Drawing.Point p)
        //{
        //	return new POINT(p.X, p.Y);
        //}
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SIZE
    {
        public int cx;
        public int cy;

        public SIZE(int cx, int cy)
        {
            this.cx = cx;
            this.cy = cy;
        }
    }

    // Alternate
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct COLORREF
    {
        [FieldOffset(0)]
        public byte R;
        [FieldOffset(1)]
        public byte G;
        [FieldOffset(2)]
        public byte B;

        [FieldOffset(0)]
        public uint ColorDWORD;

        //public COLORREF(System.Drawing.Color color)
        //{
        //	R = color.R;
        //	G = color.G;
        //	B = color.B;

        //	ColorDWORD = (uint)R + (((uint)G) << 8) + (((uint)B) << 16);
        //}

        //public System.Drawing.Color GetColor()
        //{
        //	// return System.Drawing.Color.FromArgb((int)(0x000000FFU & ColorDWORD),
        //	// (int)(0x0000FF00U & ColorDWORD) >> 8, (int)(0x00FF0000U & ColorDWORD) >> 16);
        //	return System.Drawing.Color.FromArgb((int)ColorDWORD);
        //}

        //public void SetColor(System.Drawing.Color color)
        //{
        //	ColorDWORD = (uint)color.R + (((uint)color.G) << 8) + (((uint)color.B) << 16);
        //}

    }

}
