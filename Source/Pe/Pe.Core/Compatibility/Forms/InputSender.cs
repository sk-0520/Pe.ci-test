using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinForms = System.Windows.Forms;

namespace ContentTypeTextNet.Pe.Core.Compatibility.Forms
{
    public class InputSender
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
            WinForms.SendKeys.SendWait(keys);
        }

        #endregion
    }
}
