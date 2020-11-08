using System;

namespace Examples.Cleanup
{
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
