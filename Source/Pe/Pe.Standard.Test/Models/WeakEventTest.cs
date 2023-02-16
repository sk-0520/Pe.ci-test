using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Standard.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Standard.Test.Models
{
    file class EventSource
    {
        #region define

        public event EventHandler<EventArgs>? Strong;
        public event EventHandler<EventArgs>? Weak
        {
            add => WeakEvent.Add(value);
            remove => WeakEvent.Remove(value);
        }

        #endregion

        #region property

        private WeakEvent<EventArgs> WeakEvent { get; } = new WeakEvent<EventArgs>(nameof(Weak));

        #endregion

        #region function

        public void RaiseStrong()
        {
            Strong?.Invoke(this, EventArgs.Empty);
        }

        public void RaiseWeak()
        {
            WeakEvent?.Raise(this, EventArgs.Empty);
        }

        #endregion
    }

    file class EventListener
    {
        #region property

        public int StrongCount { get; set; } = 0;
        public int WeakCount { get; set; } = 0;

        #endregion

        #region function

        public void Strong(object? sender, EventArgs e)
        {
            StrongCount += 1;
            Assert.IsTrue(true);
        }

        public void Weak(object? sender, EventArgs e)
        {
            if(WeakCount == 0) {
                WeakCount += 1;
                Assert.IsTrue(true);
            } else {
                Assert.Fail();
            }
        }

        #endregion
    }

    [TestClass]
    public class WeakEventTest
    {
        #region property

        public int StrongTestCount { get; set; } = 0;
        public int WeakTestCount { get; set; } = 0;

        #endregion

        #region function

        [TestMethod]
        public void StrongTest()
        {
            var source = new EventSource();

            source.RaiseStrong();
            Assert.AreEqual(0, StrongTestCount);

            source.Strong += Source_Strong;
            source.RaiseStrong();
            Assert.AreEqual(1, StrongTestCount);

            static void Scope(EventSource source)
            {
                var listener = new EventListener();
                source.Strong += listener.Strong;
                source.RaiseStrong();
                listener = null;
            }
            Scope(source);
            Assert.AreEqual(2, StrongTestCount);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            source.RaiseStrong();
            Assert.AreEqual(3, StrongTestCount);
        }

        [TestMethod]
        public void WeakTest()
        {
            var source = new EventSource();

            source.RaiseWeak();
            Assert.AreEqual(0, WeakTestCount);

            source.Weak += Source_Weak;
            source.RaiseWeak();
            Assert.AreEqual(1, WeakTestCount);

            static void Scope(EventSource source)
            {
                var listener = new EventListener();
                source.Weak += listener.Weak;
                source.RaiseWeak();
                listener = null;
            }
            Scope(source);
            Assert.AreEqual(2, WeakTestCount);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            source.RaiseWeak();
            Assert.AreEqual(3, WeakTestCount);
        }

        #endregion

        private void Source_Strong(object? sender, EventArgs e)
        {
            StrongTestCount += 1;
        }

        private void Source_Weak(object? sender, EventArgs e)
        {
            WeakTestCount += 1;
        }
    }

}
