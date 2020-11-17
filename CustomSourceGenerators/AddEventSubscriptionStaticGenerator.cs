using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomSourceGenerators
{
    [Generator]
    public class AddEventSubscriptionStaticGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var sourceBuilder = new StringBuilder(@"
using System;

namespace Foxy.Core.Cleanup
{
    partial class DisposableContainerExtensions
    {");
            for (int i = 1; i <= 16; i++)
            {
                var range = Enumerable.Range(1, i).ToList();
                var typeParameters = string.Join(", ", range.Select(e => $"T{ e }"));
                sourceBuilder.Append($@"
        /// <summary>
        /// Subscribe the handler for the given event and adds the unsubscription 
        /// to the DisposableContanier so it is called on Dispose.
        /// </summary>
        /// <param name=""container"">The container where the event unsubscription is added.</param>
        /// <param name=""type"">The type which contains the static event.</param>
        /// <param name=""eventName"">The name of the event. It is case sensitive.</param>
        /// <param name=""handler"">The handler which are subscripted to the event.</param>
        public static void AddEventSubscription<{typeParameters}>(this DisposableContainer container,
            Type type,
            string eventName,
            Action<{typeParameters}> handler)
        {{
            container.AddManagedResource(new EventSubscription(type, eventName, handler));
        }}");
            }
            // finish creating the source to inject
            sourceBuilder.Append(@"
    }
}");
            // inject the created source into the users compilation
            context.AddSource("AddEventSubscriptionStatic", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        }

        public void Initialize(GeneratorInitializationContext context)
        {
        }
    }
}
