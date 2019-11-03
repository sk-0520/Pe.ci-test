using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Core.Models.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao
{
    public abstract class ApplicationDatabaseObjectBase : DatabaseAccessObjectBase
    {
        public ApplicationDatabaseObjectBase(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
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

        protected int ToInt(long value)
        {
            return (int)Math.Clamp(value, int.MinValue, int.MaxValue);
        }

        #endregion
    }
}
