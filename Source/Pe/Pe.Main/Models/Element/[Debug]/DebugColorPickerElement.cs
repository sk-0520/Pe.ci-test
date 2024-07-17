using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element._Debug_
{
    public class DebugColorPickerElement: DebugElementBase
    {
        public DebugColorPickerElement(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        { }

        #region property

        #endregion

        #region function
        #endregion

        #region DebugElementBase

        protected override Task InitializeCoreAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        #endregion


    }
}
