using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinForms = System.Windows.Forms;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms
{
    class InputSender
    {
        #region function

        public Task SendAsync(string keys)
        {
            return Task.Run(() => {
                WinForms.SendKeys.SendWait(keys);
            });
        }

        /// <summary>
        /// なんだかなぁ。
        /// </summary>
        /// <param name="keys"></param>
        public void Send(string keys)
        {
            SendAsync(keys);
        }

        #endregion
    }
}
