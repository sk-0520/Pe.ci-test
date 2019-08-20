using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Model
{
    /// <summary>
    /// ピクセル情報。
    /// </summary>
    public enum Px
    {
        /// <summary>
        /// 知らん。
        /// </summary>
        Unknown,
        /// <summary>
        /// 論理座標系。
        /// </summary>
        Logical,
        /// <summary>
        /// デバイス座標系。
        /// </summary>
        Device,
    }

    /// <summary>
    /// ピクセル情報を指定。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = false)]
    public class PixelKindAttribute : System.Attribute
    {
        /// <summary>
        /// ピクセル情報を指定。
        /// </summary>
        /// <param name="px"></param>
        public PixelKindAttribute(Px px)
        {
            Px = px;
        }

        #region property

        /// <summary>
        /// ピクセル情報。
        /// </summary>
        public Px Px { get; }

        #endregion
    }
}
