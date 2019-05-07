using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Model.Launcher;

namespace ContentTypeTextNet.Pe.Main.Model.Data.Dto.Entity
{
    public class LauncherFilesEntityPathDto : DtoBase
    {
        #region property

        public string File { get; set; }
        public string Option { get; set; }
        public string WorkDirectory { get; set; }


        #endregion
    }


}
