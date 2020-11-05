using Foxy.Core.Cleanup;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Examples.Cleanup
{
    [Collection("Sequential")]
    public class FooBazExample
    {
        public FooBazExample(ITestOutputHelper testOutputHelper)
        {
            Console.SetOut(new TestOutputHelperWriter(testOutputHelper));
        }

        [Fact]
        public void Run_FooBazExample()
        {
            var foo = new Foo();
            Baz.FireStaticEventHandler();
            foo.Dispose();
            Baz.FireStaticEventHandler();
        }
    }
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
    public class Baz : IDisposable
    {
        public static event Action<object> StaticEventHandler;

        public void Dispose()
        {
            Console.WriteLine("Baz is disposed");
        }

        public static void FireStaticEventHandler()
        {
            var eh = StaticEventHandler;
            if (eh != null)
                eh.Invoke(new object());
            else
            {
                Console.WriteLine("No event handler is subscribed.");
            }

        }
    }
}
