using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace CommonLibraries.Core.Cleanup
{
    public class DisposableContainerTests
    {
        [Fact]
        public void AddManagedResource_adds_Disposables_which_is_called_on_dispose()
        {
            var container = new DisposableContainer();
            var mock1 = new Mock<IDisposable>();
            var mock2 = new Mock<IDisposable>();

            container.AddManagedResource(mock1.Object);
            container.AddManagedResource(mock2.Object);
            container.Dispose();

            mock1.Verify(d => d.Dispose(), Times.Once);
            mock2.Verify(d => d.Dispose(), Times.Once);
        }

        [Fact]
        public void AddManagedResource_adds_Disposable_only_once()
        {
            var container = new DisposableContainer();
            var mock = new Mock<IDisposable>();

            container.AddManagedResource(mock.Object);
            container.AddManagedResource(mock.Object);
            container.Dispose();

            mock.Verify(d => d.Dispose(), Times.Once);
        }

        [Fact]
        public void Dispose_disposes_only_once()
        {
            var container = new DisposableContainer();
            var mock = new Mock<IDisposable>();

            container.AddManagedResource(mock.Object);
            container.Dispose();
            container.Dispose();

            mock.Verify(d => d.Dispose(), Times.Once);
        }

        [Fact]
        public void Dispose_doesnt_stop_when_a_dispose_failed()
        {
            var container = new DisposableContainer();
            var mock1 = new Mock<IDisposable>();
            mock1.Setup(e => e.Dispose()).Throws(new Exception());
            var mock2 = new Mock<IDisposable>();

            container.AddManagedResource(mock1.Object);
            container.AddManagedResource(mock2.Object);
            container.Dispose();

            mock2.Verify(d => d.Dispose(), Times.Once);
        }

        [Fact]
        public void Dispose_calls_DisposeFailed_on_Exception()
        {
            var container = new DisposableContainer();
            var mock1 = new Mock<IDisposable>();
            var ex = new Exception();
            mock1.Setup(e => e.Dispose()).Throws(ex);
            IDisposable resultDisposable = null;
            Exception resultException = null;
            container.DisposeFailed += (d, e) =>
            {
                resultDisposable = d;
                resultException = e;
            };

            container.AddManagedResource(mock1.Object);
            container.Dispose();

            resultDisposable.Should().BeSameAs(mock1.Object);
            resultException.Should().BeSameAs(ex);
        }
    }
}
