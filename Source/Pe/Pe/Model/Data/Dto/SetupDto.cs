using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Model.Data.Dto
{
    public interface IReadOnlySetupDto : IReadOnlyCommonDto
    {
        #region property

        /// <summary>
        /// 最終使用バージョン。
        /// <para>前バージョンじゃなくて前回の実行バージョンね。</para>
        /// <para>初回(0.84.0以下)なら0.0.0.0ね！</para>
        /// </summary>
        Version LastVersion { get; }
        /// <summary>
        /// 現在バージョン。
        /// </summary>
        Version ExecuteVersion { get; }

        #endregion
    }

    public class SetupDto : CommonDtoBase, IReadOnlySetupDto
    {
        public SetupDto()
        { }

        #region IReadOnlySetupDto

        public Version LastVersion { get; set; }
        public Version ExecuteVersion { get; set; }

        #endregion
    }
}
