using Moq;
using System;
using Xunit;

namespace CommonLibraries.Core.Cleanup
{
    public class EventSubscriptionTests
    {
        [Fact]
        public void EventSubscription_ctor_subscribe_same_type_event_hendler()
        {
            // Arrange
            var instanceMock = new Mock<IDummyObserver>();
            var source = new DummyEventSource();

            // Act
            var subscription = new EventSubscription(
                source, nameof(source.InstanceHandler1), new EventHandler(instanceMock.Object.EventHandler));

            // Assert
            var args = new EventArgs();
            source.InvokeInstanceHandler1(args);
            instanceMock.Verify(e => e.EventHandler(source, args), Times.Once);
        }

        [Fact]
        public void EventSubscription_ctor_subscribe_different_type_event_hendler()
        {
            // Arrange
            var instanceMock = new Mock<IDummyObserver>();
            var source = new DummyEventSource();

            // Act
            var subscription = new EventSubscription(
                source, nameof(source.InstanceHandler1), new Action<object, EventArgs>(instanceMock.Object.EventHandler));

            // Assert
            var args = new EventArgs();
            source.InvokeInstanceHandler1(args);
            instanceMock.Verify(e => e.EventHandler(source, args), Times.Once);
        }

        [Fact]
        public void EventSubscription_ctor_subscribe_multi_same_type_delegate()
        {
            // Arrange
            var instanceMock = new Mock<IDummyObserver>();
            var source = new DummyEventSource();

            // Act
            var subscription = new EventSubscription(
                source, nameof(source.InstanceHandler1), Delegate.Combine(
                    new EventHandler(instanceMock.Object.EventHandler),
                    new EventHandler(instanceMock.Object.EventHandler)
                    ));

            // Assert
            var args = new EventArgs();
            source.InvokeInstanceHandler1(args);
            instanceMock.Verify(e => e.EventHandler(source, args), Times.Exactly(2));
        }

        [Fact]
        public void EventSubscription_ctor_subscribe_multi_different_type_delegate()
        {
            // Arrange
            var instanceMock = new Mock<IDummyObserver>();
            var source = new DummyEventSource();

            // Act
            var subscription = new EventSubscription(
                source, nameof(source.InstanceHandler1), Delegate.Combine(
                    new Action<object, EventArgs>(instanceMock.Object.EventHandler),
                    new Action<object, EventArgs>(instanceMock.Object.EventHandler)
                    ));

            // Assert
            var args = new EventArgs();
            source.InvokeInstanceHandler1(args);
            instanceMock.Verify(e => e.EventHandler(source, args), Times.Exactly(2));
        }

        [Fact]
        public void EventSubscription_Dispose_unsubscribe_same_type_event()
        {
            // Arrange
            var instanceMock = new Mock<IDummyObserver>();
            var source = new DummyEventSource();
            var subscription = new EventSubscription(
                source, nameof(source.InstanceHandler1), new EventHandler(instanceMock.Object.EventHandler));

            // Act
            subscription.Dispose();

            // Assert
            var args = new EventArgs();
            source.InvokeInstanceHandler1(args);
            instanceMock.Verify(e => e.EventHandler(It.IsAny<object>(), It.IsAny<EventArgs>()), Times.Never);
        }

        [Fact]
        public void EventSubscription_Dispose_unsubscribe_different_type_event()
        {
            // Arrange
            var instanceMock = new Mock<IDummyObserver>();
            var source = new DummyEventSource();
            var subscription = new EventSubscription(
                source, nameof(source.InstanceHandler1), new Action<object, EventArgs>(instanceMock.Object.EventHandler));

            // Act
            subscription.Dispose();

            // Assert
            var args = new EventArgs();
            source.InvokeInstanceHandler1(args);
            instanceMock.Verify(e => e.EventHandler(It.IsAny<object>(), It.IsAny<EventArgs>()), Times.Never);
        }

        [Fact]
        public void EventSubscription_ctor_subscribe_static_same_type_event()
        {
            // Arrange
            var instanceMock = new Mock<IDummyObserver>();

            // Act
            var subscription = new EventSubscription(
                typeof(DummyEventSource), nameof(DummyEventSource.StaticHandler1), new EventHandler(instanceMock.Object.EventHandler));

            // Assert
            var args = new EventArgs();
            DummyEventSource.InvokeStaticHandler1(args);
            instanceMock.Verify(e => e.EventHandler(null, args), Times.Once);
        }

        [Fact]
        public void EventSubscription_ctor_subscribe_static_different_type_event()
        {
            // Arrange
            var instanceMock = new Mock<IDummyObserver>();

            // Act
            var subscription = new EventSubscription(
                typeof(DummyEventSource), nameof(DummyEventSource.StaticHandler1), new EventHandler(instanceMock.Object.EventHandler));

            // Assert
            var args = new EventArgs();
            DummyEventSource.InvokeStaticHandler1(args);
            instanceMock.Verify(e => e.EventHandler(null, args), Times.Once);
        }

    }
    #region Dummy classes
    public class DummyEventSource
    {
        public event EventHandler InstanceHandler1;

        public void InvokeInstanceHandler1(EventArgs args)
        {
            InstanceHandler1?.Invoke(this, args);
        }

        public static event EventHandler StaticHandler1;

        public static void InvokeStaticHandler1(EventArgs args)
        {
            StaticHandler1?.Invoke(null, args);
        }
    }

    public interface IDummyObserver
    {
        void EventHandler(object sender, EventArgs args);
    }
    #endregion
}
