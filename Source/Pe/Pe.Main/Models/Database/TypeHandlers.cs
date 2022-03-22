using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace ContentTypeTextNet.Pe.Main.Models.Database
{
    internal class GuidTypeHandler: SqlMapper.TypeHandler<System.Guid>
    {
        #region TypeHandler

        public override void SetValue(IDbDataParameter parameter, System.Guid value)
        {
            parameter.Value = value.ToString("D");
        }

        public override System.Guid Parse(object value)
        {

            var s = (string)value;
            if(s != null) {
                if(System.Guid.TryParse(s, out var ret)) {
                    return ret;
                }
            }

            return System.Guid.Empty;
            ;
        }
        #endregion
    }
}
