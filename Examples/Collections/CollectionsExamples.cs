using Foxy.Core.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Examples.Collections
{
    public static class CollectionsExamples
    {
        public static void Run_AddRange_Example()
        {
            ICollection<Element> collection = GetSomeCollection();
            IEnumerable<Element> newValues = CreateNewValues();

            collection.AddRange(newValues);

            WriteResult(collection);
        }

        public static void Run_AddElements_Example()
        {
            var element1 = new Element { Key = 2, Name = "Second" };
            var element2 = new Element { Key = 3, Name = "Third" };
            ICollection<Element> collection = GetSomeCollection();

            collection.AddElements(element1, element2);

            WriteResult(collection);
        }

        public static void Run_FindAndRemoveAndRemoveAll_Example()
        {
            var source = new List<ElementViewModel> {
                new ElementViewModel { Key = 2, Name = "SecondUpdated" },
                new ElementViewModel { Key = 3, Name = "Third" }
            };
            var element = new MainElement();

            var targetList = element.Items.ToList();
            foreach (var sourceItem in source)
            {
                var targetItem = targetList.FindAndRemove(item => item.Key == sourceItem.Key);
                if (targetItem == null)
                {
                    element.Items.Add(CreateElement(sourceItem));
                }
                else
                {
                    UpdateElement(targetItem, sourceItem);
                }
            }
            element.Items.RemoveAll(targetList);

            WriteResult(element.Items);
        }

        public static void Run_ToChunk_Example()
        {
            var size = 10;
            var largeEnumerator = GetData();
            foreach (var chunk in largeEnumerator.ToChunks(size))
            {
                ProcessChunk(chunk);
            }
        }

        public static void Run_SortByKey_Example()
        {
            List<Element> list = GetUnsortedList();
            list.SortByKey(element => element.Key);

            WriteResult(list);
        }

        private static void ProcessChunk(List<int> chunk)
        {
            Console.WriteLine($"Next chunk: {string.Join(", ", chunk)}");
        }

        private static IEnumerable<int> GetData()
        {
            return Enumerable.Range(0, 35);
        }

        private static List<Element> GetUnsortedList()
        {
            var rnd = new Random();
            return Enumerable
                .Range(0, 10)
                .Select(e => new Element { Key = rnd.Next(100) })
                .ToList();
        }

        private static void UpdateElement(Element targetItem, ElementViewModel sourceItem)
        {

            targetItem.Name = sourceItem.Name;
        }

        private static Element CreateElement(ElementViewModel sourceItem)
        {
            return new Element
            {
                Key = sourceItem.Key,
                Name = sourceItem.Name
            };
        }

        private static void WriteResult(ICollection<Element> collection)
        {
            foreach (var item in collection)
            {
                Console.WriteLine($"Key:{item.Key} Name:{item.Name}");
            }
        }

        private static IEnumerable<Element> CreateNewValues()
        {
            yield return new Element { Key = 2, Name = "Second" };
            yield return new Element { Key = 3, Name = "Third" };
        }

        private static ICollection<Element> GetSomeCollection()
        {
            return new List<Element>
            {
                new Element {Key = 1, Name = "First" }
            };
        }
    }
}
