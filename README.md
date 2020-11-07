# Foxy.Core
Foxy.Core provides extensions and useful helpers functions that base only on .net standard 2.0.

# Content (Namespaces)
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

## Collections
This namespace contains some usefull extensions for the ```System.Collections.Generic``` namespace.

### AddRange
Sometimes I work with ```ICollection``` or it's derivative but I still would like to use ``` AddRange``` from the ```List<T>```. This extension comes to help.

```csharp
    ICollection<Element> collection = GetSomeCollection();
    IEnumerable<Element> newValues = CreateNewValues();
    collection.AddRange(newValues);
```

### AddElements
Adds several element to a collection.
```csharp
    ICollection<Element> collection = GetSomeCollection();
    collection.AddElements(element1,element2,element3);
```

### FindAndRemove
This methods find an element in the list and if found removes it. A commonly used pattern by me when working with a list of ORM records is take a copy of the target list, and for every element from the source I try to find in the target. If it is not there I add to the target, if there I take out from the copy and I update. The remaining should be deleted. It looks like this:
```csharp
    var targetList = element.Items.ToList();
    foreach(var sourceItem in source) {
        var targetItem = targetList.FindAndRemove(item => item.Key == sourceItem.Key);
        if(targetItem == null) {
            element.Items.Add(CreateElement(sourceItem));
        } else {
            UpdateElement(targetItem, sourceItem);
        }
    }
    element.Items.RemoveAll(targetList);
```
### ToChunks
Sometimes it is necessary to process a very large list but you can process only a small batch at once. This extension breaks the large list to fixed size.
```csharp
    var largeEnumerator = GetData();
    foreach(var chunk in largeEnumerator.ToChunks(size)) {
        ProcessChunk(chunk);
    }
```

### SortByKey
If you want to sort a ```List<T>``` or an ```Array<T>``` by a key element this extension simplifies it.
```csharp
    List<Element> list = GetData();
    list.SortByKey(lement => element.Key);
```

## Linq
This namespace contains a left join for linq. This method extracts the common pattern that is used for left joins and usually recognized by ORMs so it can be used in database translated queries also. I use navigation properties to express left joins mostly but sometimes I need queries like this and it is more readable to write a LeftJoin than the original pattern.

The original left join pattern looks like this:
```csharp
    var result = outer.GroupJoin(inner, outer => outer.Key, inner.Key, (inner, outer) => new { inner, outer })
        .SelectMany(groups => groups.outer.DefaultIfEmpty(), (joined, groupElement) => new { inner = joined.inner, outer = groupElement });
```
With the extension you can write like the regular join:
```csharp
    var result = outer.LeftJoin(inner, outer => outer.Key, inner.Key, (inner, outer) => new { inner, outer });
```

## Text
This namespace contains a [natural string comparer](https://en.wikipedia.org/wiki/Natural_sort_order). Natural string comparison in short means when you compare strings you treat digits as numbers if both string contains digit in the same position. There are several implementation on the stackoverflow and in the github so when I decided to write an own implementation I set the following goals:
 - It has to be pure managed implementation. No native call.
 - It has to be the fastest implementation in the .net world. (Sorting is process consuming)
 - It has to have the same interface as the original ```StringComparer``` so it can be swapped where needed.
 - Have some fun and learn new things while implementiing it. :)

I think I managed to statisfy all three goal so enjoy the result. For this I say a **huge thanks** for the .net development team since the most impacttfull parts of the solution comes from the original StringComparer implementation.

### Some note on the implementation
>  As [best practices tells](https://docs.microsoft.com/en-us/dotnet/standard/base-types/best-practices-strings) you should use Ordinal or OrdinalIgnoreCase whenever possible. In my implementation I use caching for culture specific comparisons which eats lots of memory (128kb per culture).

> I focused only on 64 bit processors. Based on the StringComparer the 32 bit version should have a little bit different implementation. I don't know if it is important since almost every processor is 64 bit nowadays.

> My original intent was to help optimize the [NaturalSort.Extension](https://github.com/tompazourek/NaturalSort.Extension) package since that is the most popular natural sort package but in the end I didn't like it's interface and I completely reworked the implementation anyway. In the end I used it as a comparison.

### Perfomance tests
I compared my implementation against The StringComparer (although it is not natural string comparer), NaturalSort.Extension, and StrCmpLogicalW native method. 

<pre>
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.572 (2004/?/20H1)
AMD Ryzen 5 2600, 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=3.1.403
  [Host]     : .NET Core 3.1.9 (CoreCLR 4.700.20.47201, CoreFX 4.700.20.47203), X64 RyuJIT
  Job-TPQDPV : .NET Core 3.1.9 (CoreCLR 4.700.20.47201, CoreFX 4.700.20.47203), X64 RyuJIT
</pre>
<pre>OutlierMode=RemoveAll  </pre>

### Comparing two long text with invariant culture
 - OrdinalPerf -> ```"Lorem ipsum dolor sit amet, consectetur adipiscing elit. 2 a"``` vs 
```"Lorem ipsum dolor sit amet, consectetur adipiscing elit. 10 a"```
 - ToUpperPerf -> ```"LOREM IPSUM DOLOR SIT AMET, CONSECTETUR ADIPISCING ELIT. 2 A"``` vs ```"Lorem ipsum dolor sit amet, consectetur adipiscing elit. 10 a"```
 
|Method|Pairs|Mean|Error|StdDev|Ratio|Rank|
|--- |--- |--- |--- |--- |--- |--- |
|NaturalStringComparer|OrdinalPerf|21.72 ns|0.233 ns|0.206 ns|0.009|1|
|NaturalSortExtension|OrdinalPerf|2,216.32 ns|7.710 ns|6.835 ns|0.961|3|
|PInvokeComparer|OrdinalPerf|2,306.10 ns|8.937 ns|8.360 ns|1.000|4|
|StringComparer|OrdinalPerf|109.82 ns|0.521 ns|0.435 ns|0.048|2|
|NaturalStringComparer|ToUpperPerf|166.78 ns|1.390 ns|1.300 ns|0.07|1|
|NaturalSortExtension|ToUpperPerf|2,509.88 ns|12.436 ns|9.709 ns|1.09|4|
|PInvokeComparer|ToUpperPerf|2,308.19 ns|12.155 ns|10.150 ns|1.00|3|
|StringComparer|ToUpperPerf|243.01 ns|1.770 ns|1.656 ns|0.11|2|

### Sorting a list of text with invariant culture.
|Method|Mean|Error|StdDev|Ratio|Rank|
|--- |--- |--- |--- |--- |--- |
|NaturalStringComparer|7.903 ms|0.0314 ms|0.0294 ms|0.15|1|
|NaturalSortExtension|64.938 ms|0.2719 ms|0.2544 ms|1.21|4|
|PInvokeComparer|53.483 ms|0.1309 ms|0.1161 ms|1.00|3|
|StringComparer|17.276 ms|0.0960 ms|0.0898 ms|0.32|2|

### Comparing two long text with ordinal
 - OrdinalPerf -> ```"Lorem ipsum dolor sit amet, consectetur adipiscing elit. 2 a"``` vs 
```"Lorem ipsum dolor sit amet, consectetur adipiscing elit. 10 a"```
 - ToUpperPerf -> ```"LOREM IPSUM DOLOR SIT AMET, CONSECTETUR ADIPISCING ELIT. 2 A"``` vs ```"Lorem ipsum dolor sit amet, consectetur adipiscing elit. 10 a"```

|Method|Pairs|Mean|Error|StdDev|Ratio|Rank|
|--- |--- |--- |--- |--- |--- |--- |
|NaturalStringComparer|OrdinalPerf|22.863 ns|0.2800 ns|0.2619 ns|0.010|2|
|NaturalSortExtension|OrdinalPerf|981.308 ns|5.6031 ns|5.2411 ns|0.424|3|
|PInvokeComparer|OrdinalPerf|2,310.651 ns|14.8619 ns|13.1746 ns|1.000|4|
|StringComparer|OrdinalPerf|11.285 ns|0.0467 ns|0.0364 ns|0.005|1|
|NaturalStringComparer|ToUpperPerf|6.796 ns|0.0302 ns|0.0252 ns|0.003|2|
|NaturalSortExtension|ToUpperPerf|69.479 ns|0.5270 ns|0.4929 ns|0.030|3|
|PInvokeComparer|ToUpperPerf|2,351.548 ns|7.8520 ns|7.3448 ns|1.000|4|
|StringComparer|ToUpperPerf|3.184 ns|0.0466 ns|0.0413 ns|0.001|1|

### Sorting a list of text with ordinal.
|Method|Mean|Error|StdDev|Ratio|Rank|
|--- |--- |--- |--- |--- |--- |
|NaturalStringComparer|5.597 ms|0.0224 ms|0.0175 ms|0.11|2|
|NaturalSortExtension|23.090 ms|0.0263 ms|0.0220 ms|0.44|3|
|PInvokeComparer|52.110 ms|0.3530 ms|0.2947 ms|1.00|4|
|StringComparer|4.262 ms|0.0147 ms|0.0122 ms|0.08|1|
