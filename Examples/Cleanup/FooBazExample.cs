namespace Examples.Cleanup
{
    public static class FooBazExample
    {
        public static void Run_FooBazExample()
        {
            var foo = new Foo();
            Baz.FireStaticEventHandler();
            foo.Dispose();
            Baz.FireStaticEventHandler();
        }
    }
}
