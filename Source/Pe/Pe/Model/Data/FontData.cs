using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Model.Data
{
    public interface IReadOnlyFontData
    {
        #region property

        /// <summary>
        /// フォント名。
        /// </summary>
        string Family { get; }
        /// <summary>
        /// フォントサイズ。
        /// </summary>
        double Size { get; }
        /// <summary>
        /// フォントを太字にするか。
        /// </summary>
        bool Bold { get; }
        /// <summary>
        /// フォントを斜体にするか。
        /// </summary>
        bool Italic { get; }
        bool Underline { get; }
        bool LineThrough { get; }

        #endregion
    }

    [Serializable, DataContract]
    public class FontData : IReadOnlyFontData
    {
        #region IReadOnlyFontData

        /// <summary>
        /// フォント名。
        /// </summary>
        [DataMember]
        public string Family { get; set; }
        /// <summary>
        /// フォントサイズ。
        /// </summary>
        [DataMember]
        public double Size { get; set; }
        /// <summary>
        /// フォントを太字にするか。
        /// </summary>
        [DataMember]
        public bool Bold { get; set; }
        /// <summary>
        /// フォントを斜体にするか。
        /// </summary>
        [DataMember]
        public bool Italic { get; set; }

        [DataMember]
        public bool Underline { get; set; }
        [DataMember]
        public bool LineThrough { get; set; }

        #endregion
    }
}
