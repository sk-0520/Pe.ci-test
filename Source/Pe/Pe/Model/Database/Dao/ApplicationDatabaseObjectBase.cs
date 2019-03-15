using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Database.Dao
{
    public abstract class ApplicationDatabaseObjectBase : DatabaseAccessObjectBase
    {
        public ApplicationDatabaseObjectBase(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, ILogger logger)
            : base(commander, statementLoader, logger)
        { }

        public ApplicationDatabaseObjectBase(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, loggerFactory)
        { }

        #region function

        protected string FromTimespan(TimeSpan timespan)
        {
            return timespan.ToString();
        }

        protected TimeSpan ToTimespan(string raw)
        {
            return TimeSpan.Parse(raw);
        }

        protected string FromColor(Color color)
        {
            return color.ToString();
        }

        protected Color ToColor(string raw)
        {
            var colorConverter = new ColorConverter();
            var color = (Color)colorConverter.ConvertFrom(raw);
            return color;
        }

        #endregion
    }
}
