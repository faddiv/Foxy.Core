using System.Collections.Generic;

namespace Examples.Collections
{
    public class MainElement
    {
        public MainElement()
        {
            Items = new List<Element>
            {
                new Element {Key = 1, Name = "First" },
                new Element { Key = 2, Name = "Second" }
        };
        }
        public List<Element> Items { get; set; }
    }
}
