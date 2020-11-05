using System;

namespace Foxy.Core.Cleanup
{
    /// <summary>
    /// Connects other classes from this namespace to the DisposableContainer.
    /// </summary>
    public static partial class DisposableContainerExtensions
    {
        public static void AddCleanupAction(this DisposableContainer container,
            Action cleanupAction)
        {
            container.AddManagedResource(new CleanupAction(cleanupAction));
        }

        /// <summary>
        /// Subscribe the handler for the given event and adds the unsubscription 
        /// to the DisposableContanier so it is called on Dispose.
        /// </summary>
        /// <param name="container">The container where the event unsubscription is added.</param>
        /// <param name="instance">The instance which contains the event.</param>
        /// <param name="eventName">The name of the event. It is case sensitive.</param>
        /// <param name="handler">The handler which are subscripted to the event.</param>
        public static void AddEventSubscription(this DisposableContainer container,
            object instance,
            string eventName,
            Action handler)
        {
            container.AddManagedResource(new EventSubscription(instance, eventName, handler));
        }

        /// <summary>
        /// Subscribe the handler for the given event and adds the unsubscription 
        /// to the DisposableContanier so it is called on Dispose.
        /// </summary>
        /// <param name="container">The container where the event unsubscription is added.</param>
        /// <param name="type">The type which contains the static event.</param>
        /// <param name="eventName">The name of the event. It is case sensitive.</param>
        /// <param name="handler">The handler which are subscripted to the event.</param>
        public static void AddEventSubscription(this DisposableContainer container,
            Type type,
            string eventName,
            Action handler)
        {
            container.AddManagedResource(new EventSubscription(type, eventName, handler));
        }
    }
}
