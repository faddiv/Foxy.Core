using Moq;
using System;
using Xunit;

namespace CommonLibraries.Core.Cleanup
{
    public class DisposabeContainerExtensionsTests
    {

        [Fact]
        public void AddCleanupAction_adds_CleanupAction_to_the_DisposableContainer()
        {
            var container = new DisposableContainer();
            var mock1 = new Mock<Action>();

            container.AddCleanupAction(mock1.Object);
            container.Dispose();

            mock1.Verify(d => d(), Times.Once);
        }

        [Fact]
        public void AddEventSubscription_adds_instance_handler_delegate_to_the_DisposableContainer()
        {

            var container = new DisposableContainer();
            var instanceMock = new Mock<IDummyObserver>();
            var source = new DummyEventSource();
            var args = new EventArgs();

            container.AddEventSubscription(
                source,
                nameof(source.InstanceHandler1),
                new EventHandler(instanceMock.Object.EventHandler));
            source.InvokeInstanceHandler1(args);
            container.Dispose();
            source.InvokeInstanceHandler1(args);

            instanceMock.Verify(d => d.EventHandler(It.IsAny<object>(), It.IsAny<EventArgs>()), Times.Once);
        }

        [Fact]
        public void AddEventSubscription_adds_static_handler_delegate_to_the_DisposableContainer()
        {

            var container = new DisposableContainer();
            var instanceMock = new Mock<IDummyObserver>();
            var args = new EventArgs();

            container.AddEventSubscription<DummyEventSource>(
                nameof(DummyEventSource.StaticHandler1),
                new EventHandler(instanceMock.Object.EventHandler));
            DummyEventSource.InvokeStaticHandler1(args);
            container.Dispose();
            DummyEventSource.InvokeStaticHandler1(args);

            instanceMock.Verify(d => d.EventHandler(It.IsAny<object>(), It.IsAny<EventArgs>()), Times.Once);
        }

        [Fact]
        public void AddEventSubscription_adds_static_handler_delegate_to_the_DisposableContainer2()
        {

            var container = new DisposableContainer();
            var instanceMock = new Mock<IDummyObserver>();
            var args = new EventArgs();

            container.AddEventSubscription(
                typeof(DummyEventSource),
                nameof(DummyEventSource.StaticHandler1),
                new EventHandler(instanceMock.Object.EventHandler));
            DummyEventSource.InvokeStaticHandler1(args);
            container.Dispose();
            DummyEventSource.InvokeStaticHandler1(args);

            instanceMock.Verify(d => d.EventHandler(It.IsAny<object>(), It.IsAny<EventArgs>()), Times.Once);
        }

        [Fact]
        public void AddEventSubscription_adds_instance_event_handler_to_the_DisposableContainer()
        {

            var container = new DisposableContainer();
            var instanceMock = new Mock<IDummyObserver>();
            var source = new DummyEventSource();
            var args = new EventArgs();

            container.AddEventSubscription<object, EventArgs>(
                source,
                nameof(source.InstanceHandler1),
                instanceMock.Object.EventHandler);
            source.InvokeInstanceHandler1(args);
            container.Dispose();
            source.InvokeInstanceHandler1(args);

            instanceMock.Verify(d => d.EventHandler(It.IsAny<object>(), It.IsAny<EventArgs>()), Times.Once);
        }


        [Fact]
        public void AddEventSubscription_adds_Static_event_handler_to_DisposableContainer()
        {

            var container = new DisposableContainer();
            var instanceMock = new Mock<IDummyObserver>();
            var source = new DummyEventSource();
            var args = new EventArgs();

            container.AddEventSubscription<DummyEventSource, object, EventArgs>(
                nameof(DummyEventSource.StaticHandler1),
                instanceMock.Object.EventHandler);
            DummyEventSource.InvokeStaticHandler1(args);
            container.Dispose();
            DummyEventSource.InvokeStaticHandler1(args);

            instanceMock.Verify(d => d.EventHandler(It.IsAny<object>(), It.IsAny<EventArgs>()), Times.Once);
        }
    }
}
