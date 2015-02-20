﻿namespace ContentTypeTextNet.Pe.PeMain.UI.Ex
{
	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;

	public abstract class ExButton: Button
	{ }

	public class ColorChangedEventArg: EventArgs
	{
		public ColorChangedEventArg(Color color)
		{
			Color = color;
		}

		public Color Color { get; private set; }
	}

	/// <summary>
	/// 色を保持するボタン。
	/// </summary>
	public class ColorButton: ExButton
	{
		protected Color _color = Color.Red;

		public event EventHandler<ColorChangedEventArg> ColorChanged = delegate { };

		public ColorButton()
		{
			TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			ImageAlign = ContentAlignment.MiddleCenter;
		}

		/// <summary>
		/// 保持する色。
		/// </summary>
		public Color Color
		{
			get { return this._color; }
			set
			{
				if(this._color != value) {
					var prevColor = this._color;
					this._color = value;
					OnChangedColor(prevColor);
				}
			}
		}

		protected virtual void OnChangedColor(Color prevColor)
		{
			ColorChanged(this, new ColorChangedEventArg(this._color));
		}
	}

	public class ColorImageButton: ColorButton
	{
		public override string Text
		{
			get { return string.Empty; }
			set { base.Text = value; }
		}

		protected override void OnChangedColor(Color prevColor)
		{
			RefreshColorImage();
			base.OnChangedColor(prevColor);
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			RefreshColorImage();
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			RefreshColorImage();
		}

		protected virtual Image CreateColorImage()
		{
			var image = new Bitmap(ClientSize.Width, ClientSize.Height);
			
			using(var g = Graphics.FromImage(image)) {
				using(var brush = new SolidBrush(Color)) {
					g.FillRectangle(brush, 0, 0, image.Width, image.Height);
				}
			}

			return image;
		}

		private void RefreshColorImage()
		{
			var oldImage = Image;
			Image = CreateColorImage();
			oldImage.ToDispose();
		}
	}
}
