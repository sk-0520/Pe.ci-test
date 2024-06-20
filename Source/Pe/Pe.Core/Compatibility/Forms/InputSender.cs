using System.Threading.Tasks;
using WinForms = System.Windows.Forms;

namespace ContentTypeTextNet.Pe.Core.Compatibility.Forms
{
    public class InputSender
    {
        #region function

        /// <inheritdoc cref="WinForms.SendKeys.Send"/>
        public Task SendAsync(string keys)
        {
            return Task.Run(() => {
                WinForms.SendKeys.Send(keys);
            });
        }

        /// <inheritdoc cref="WinForms.SendKeys.SendWait"/>
        public void SendWait(string keys)
        {
            WinForms.SendKeys.SendWait(keys);
        }

        /// <inheritdoc cref="WinForms.SendKeys.Flush"/>
        public void Flush() => WinForms.SendKeys.Flush();

        #endregion
    }
}
