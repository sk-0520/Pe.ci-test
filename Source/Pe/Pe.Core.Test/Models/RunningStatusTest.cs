using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    public class RunningStatusTest
    {
        #region function

        [Fact]
        public void StateTest()
        {
            using var test = new RunningStatus(NullLoggerFactory.Instance);

            Assert.Equal(RunningState.None, test.State);
            Assert.PropertyChanged(test, nameof(test.State), () => {
                test.State = RunningState.Running;
            });
            Assert.Equal(RunningState.Running, test.State);
        }

        #endregion
    }
}
