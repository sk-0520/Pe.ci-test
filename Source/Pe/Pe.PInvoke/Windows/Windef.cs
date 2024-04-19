using System.Runtime.InteropServices;

namespace ContentTypeTextNet.Pe.PInvoke.Windows
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1712:列挙値の前に型名を付けないでください", Justification = "WindowsAPI")]
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
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }

        //public RECT(System.Drawing.Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }

        public int X
        {
            get { return this.Left; }
            set { this.Right -= (this.Left - value); this.Left = value; }
        }

        public int Y
        {
            get { return this.Top; }
            set { this.Bottom -= (this.Top - value); this.Top = value; }
        }

        public int Height
        {
            get { return this.Bottom - this.Top; }
            set { this.Bottom = value + this.Top; }
        }

        public int Width
        {
            get { return this.Right - this.Left; }
            set { this.Right = value + this.Left; }
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
            return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", this.Left, this.Top, this.Right, this.Bottom);
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

    /// <summary>
    ///COLORREF 値は、 RGB 色を指定するために使用されます。
    /// </summary>
    /// <remarks>
    ///<para>明示的な RGB 色を指定する場合、 COLORREF 値には次の16進数形式があります。</para>
    ///<code>0x00bbggrr</code>
    ///<para>下位バイトには、赤の相対的な輝度の値が含まれています。2番目のバイトには緑の値が含まれます。3番目のバイトには blue の値が含まれています。 上位バイトは0にする必要があります。 1バイトの最大値は0xFF です。</para>
    ///<para>COLORREF color 値を作成するには、 RGBマクロを使用します。 色の値の赤、緑、および青の各要素の個々の値を抽出するには、それぞれ GetRValue、 Getgvalue、および getgvalue マクロを使用します。</para>
    /// </remarks>
    /// <seealso href="https://docs.microsoft.com/ja-jp/windows/win32/gdi/colorref" />
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
