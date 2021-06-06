using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public class Policy: IPolicy
    {
        #region IPolicy

        public PolicyBuilder CreateBuilder() => new PolicyBuilder();
        IPolicyBuilder IPolicy.CreateBuilder() => CreateBuilder();

        #endregion
    }

    public class PolicyBuilder: IPolicyBuilder
    {

    }
}
