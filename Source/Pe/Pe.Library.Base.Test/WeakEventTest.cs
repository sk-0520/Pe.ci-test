using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Base;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Base.Test
{
    file class EventSource
    {
        #region event

        public event EventHandler<EventArgs>? Strong;
        public event EventHandler<EventArgs>? Weak
        {
            add => WeakEvent.Add(value);
            remove => WeakEvent.Remove(value);
        }
        public event EventHandler? NoGenerics
        {
            add => NoGenericsEvent.Add(value);
            remove => NoGenericsEvent.Remove(value);
        }

        #endregion

        public EventSource() {
            WeakEvent = new WeakEvent<EventArgs>(nameof(Weak));
            NoGenericsEvent = new WeakEvent(nameof(NoGenerics));

            Assert.Equal(nameof(Weak), WeakEvent.EventName);
            Assert.Equal(nameof(NoGenerics), NoGenericsEvent.EventName);
        }

        #region property

        private WeakEvent<EventArgs> WeakEvent { get; } = new WeakEvent<EventArgs>(nameof(Weak));
        private WeakEvent NoGenericsEvent { get; } = new WeakEvent(nameof(NoGenerics));

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

        public void RaiseNoGenerics()
        {
            NoGenericsEvent?.Raise(this, EventArgs.Empty);
        }

        #endregion
    }

    file class EventListener
    {
        #region property

        public int StrongCount { get; private set; } = 0;
        public int WeakCount { get; private set; } = 0;
        public int NoGenericsCount { get; private set; } = 0;

        #endregion

        #region function

        public void Strong(object? sender, EventArgs e)
        {
            StrongCount += 1;
            Assert.True(true);
        }

        public void Weak(object? sender, EventArgs e)
        {
            if(WeakCount == 0) {
                WeakCount += 1;
                Assert.True(true);
            } else {
                Assert.Fail();
            }
        }

        public void NoGenerics(object? sender, EventArgs e)
        {
            if(NoGenericsCount == 0) {
                NoGenericsCount += 1;
                Assert.True(true);
            } else {
                Assert.Fail();
            }
        }

        #endregion
    }

    public class WeakEventTest
    {
        #region property

        public int StrongTestCount { get; set; } = 0;
        public int WeakTestCount { get; set; } = 0;
        public int NoGenericsTestCount { get; set; } = 0;

        #endregion

        #region function

        [Fact]
        public void StrongTest()
        {
            var source = new EventSource();

            source.Strong += null;
            source.RaiseStrong();
            Assert.Equal(0, StrongTestCount);
            source.Strong -= null;

            source.Strong += Source_Strong;
            source.RaiseStrong();
            Assert.Equal(1, StrongTestCount);

            source.Strong -= Source_Strong;
            source.RaiseStrong();
            Assert.Equal(1, StrongTestCount);
            source.Strong -= Source_Strong;
            source.Strong += Source_Strong;

            static void Scope(EventSource source)
            {
                var listener = new EventListener();
                source.Strong += listener.Strong;
                source.RaiseStrong();
                listener = null;
            }
            Scope(source);
            Assert.Equal(2, StrongTestCount);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            source.RaiseStrong();
            Assert.Equal(3, StrongTestCount);
        }

        [Fact]
        public void WeakTest()
        {
            var source = new EventSource();

            source.Weak += null;
            source.RaiseWeak();
            Assert.Equal(0, WeakTestCount);
            source.Weak -= null;

            source.Weak += Source_Weak;
            source.RaiseWeak();
            Assert.Equal(1, WeakTestCount);

            source.Weak -= Source_Weak;
            source.RaiseWeak();
            Assert.Equal(1, WeakTestCount);
            source.Weak -= Source_Weak;
            source.Weak += Source_Weak;

            static void Scope(EventSource source)
            {
                var listener = new EventListener();
                source.Weak += listener.Weak;
                source.RaiseWeak();
                listener = null;
            }
            Scope(source);
            Assert.Equal(2, WeakTestCount);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            source.RaiseWeak();
            Assert.Equal(3, WeakTestCount);
        }

        [Fact]
        public void NoGenericsTest()
        {
            var source = new EventSource();

            source.NoGenerics += null;
            source.RaiseNoGenerics();
            Assert.Equal(0, NoGenericsTestCount);
            source.NoGenerics -= null;

            source.NoGenerics += Source_NoGenerics;
            source.RaiseNoGenerics();
            Assert.Equal(1, NoGenericsTestCount);

            source.NoGenerics -= Source_NoGenerics;
            source.RaiseWeak();
            Assert.Equal(1, NoGenericsTestCount);
            source.NoGenerics -= Source_NoGenerics;
            source.NoGenerics += Source_NoGenerics;

            static void Scope(EventSource source)
            {
                var listener = new EventListener();
                source.NoGenerics += listener.NoGenerics;
                source.RaiseNoGenerics();
                listener = null;
            }
            Scope(source);
            Assert.Equal(2, NoGenericsTestCount);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            source.RaiseNoGenerics();
            Assert.Equal(3, NoGenericsTestCount);
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

        private void Source_NoGenerics(object? sender, EventArgs e)
        {
            NoGenericsTestCount += 1;
        }
    }
}
