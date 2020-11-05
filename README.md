# Foxy.Core
Foxy.Core provides extensions and useful helpers functions that base only on .net standard 2.0.

# Contents
## Cleanup
This namespace contains a helpers for executing disposing tasks easily. Main class is `DisposableContainerExtensions`. You can put multiple object with `IDisposabe` interface in it. The `Dispose` calls are in separate try catch block so an exception won't stop the disposition of other resources when one element is failed. If you want to react for on an exception you can in the `DisposeFailed` event.

Nowadays usually the dependency injection container handles the disposition but still there use cases when this is usefull. For example the event handler unsubscription.
### Examples
```csharp
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

        private void EventHandler(object obj) { }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
```
