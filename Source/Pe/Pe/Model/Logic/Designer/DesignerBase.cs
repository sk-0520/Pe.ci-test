using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Logic.Designer
{
    public abstract class DesignerBase
    {
        public DesignerBase(ILogger logger)
        {
            Logger = logger;
        }

        public DesignerBase(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateTartget(GetType());
        }

        #region property

        protected ILogger Logger { get; }

        #endregion

        #region function

        protected double GetHorizontal(Thickness thickness) => thickness.Left + thickness.Right;

        protected double GetVertical(Thickness thickness) => thickness.Top + thickness.Bottom;

        #endregion
    }
}
