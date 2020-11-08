using Foxy.Core.Cleanup;
using System;

namespace Examples.Cleanup
{
    public class Foo : IDisposable
    {
        private readonly DisposableContainer _disposable;
        private readonly Baz _baz;
        public Foo()
        {
            _disposable = new DisposableContainer();

            // Adding a disposabe object.
            _baz = _disposable.AddManagedResource(new Baz());

            // Adding an event. This prevent a memory leak. object is the event argument type.
            _disposable.AddEventSubscription<object>(typeof(Baz), nameof(Baz.StaticEventHandler), EventHandler);

            // Adding an arbitary cleanup action.
            _disposable.AddCleanupAction(SomeCleanup);
        }

        private void SomeCleanup()
        {
            Console.WriteLine("This needed to be executed on dispose");
        }

        private void EventHandler(object obj)
        {
            Console.WriteLine("EventHandler called");
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
