using System;

namespace Foxy.Core.Cleanup
{
    partial class DisposableContainerExtensions
    {
        /// <summary>
        /// Subscribe the handler for the given event and adds the unsubscription 
        /// to the DisposableContanier so it is called on Dispose.
        /// </summary>
        /// <param name="container">The container where the event unsubscription is added.</param>
        /// <param name="type">The type which contains the static event.</param>
        /// <param name="eventName">The name of the event. It is case sensitive.</param>
        /// <param name="handler">The handler which are subscripted to the event.</param>
        public static void AddEventSubscription<T1>(this DisposableContainer container,
            Type type,
            string eventName,
            Action<T1> handler)
        {
            container.AddManagedResource(new EventSubscription(type, eventName, handler));
        }
        /// <summary>
        /// Subscribe the handler for the given event and adds the unsubscription 
        /// to the DisposableContanier so it is called on Dispose.
        /// </summary>
        /// <param name="container">The container where the event unsubscription is added.</param>
        /// <param name="type">The type which contains the static event.</param>
        /// <param name="eventName">The name of the event. It is case sensitive.</param>
        /// <param name="handler">The handler which are subscripted to the event.</param>
        public static void AddEventSubscription<T1, T2>(this DisposableContainer container,
            Type type,
            string eventName,
            Action<T1, T2> handler)
        {
            container.AddManagedResource(new EventSubscription(type, eventName, handler));
        }
        /// <summary>
        /// Subscribe the handler for the given event and adds the unsubscription 
        /// to the DisposableContanier so it is called on Dispose.
        /// </summary>
        /// <param name="container">The container where the event unsubscription is added.</param>
        /// <param name="type">The type which contains the static event.</param>
        /// <param name="eventName">The name of the event. It is case sensitive.</param>
        /// <param name="handler">The handler which are subscripted to the event.</param>
        public static void AddEventSubscription<T1, T2, T3>(this DisposableContainer container,
            Type type,
            string eventName,
            Action<T1, T2, T3> handler)
        {
            container.AddManagedResource(new EventSubscription(type, eventName, handler));
        }
        /// <summary>
        /// Subscribe the handler for the given event and adds the unsubscription 
        /// to the DisposableContanier so it is called on Dispose.
        /// </summary>
        /// <param name="container">The container where the event unsubscription is added.</param>
        /// <param name="type">The type which contains the static event.</param>
        /// <param name="eventName">The name of the event. It is case sensitive.</param>
        /// <param name="handler">The handler which are subscripted to the event.</param>
        public static void AddEventSubscription<T1, T2, T3, T4>(this DisposableContainer container,
            Type type,
            string eventName,
            Action<T1, T2, T3, T4> handler)
        {
            container.AddManagedResource(new EventSubscription(type, eventName, handler));
        }
        /// <summary>
        /// Subscribe the handler for the given event and adds the unsubscription 
        /// to the DisposableContanier so it is called on Dispose.
        /// </summary>
        /// <param name="container">The container where the event unsubscription is added.</param>
        /// <param name="type">The type which contains the static event.</param>
        /// <param name="eventName">The name of the event. It is case sensitive.</param>
        /// <param name="handler">The handler which are subscripted to the event.</param>
        public static void AddEventSubscription<T1, T2, T3, T4, T5>(this DisposableContainer container,
            Type type,
            string eventName,
            Action<T1, T2, T3, T4, T5> handler)
        {
            container.AddManagedResource(new EventSubscription(type, eventName, handler));
        }
        /// <summary>
        /// Subscribe the handler for the given event and adds the unsubscription 
        /// to the DisposableContanier so it is called on Dispose.
        /// </summary>
        /// <param name="container">The container where the event unsubscription is added.</param>
        /// <param name="type">The type which contains the static event.</param>
        /// <param name="eventName">The name of the event. It is case sensitive.</param>
        /// <param name="handler">The handler which are subscripted to the event.</param>
        public static void AddEventSubscription<T1, T2, T3, T4, T5, T6>(this DisposableContainer container,
            Type type,
            string eventName,
            Action<T1, T2, T3, T4, T5, T6> handler)
        {
            container.AddManagedResource(new EventSubscription(type, eventName, handler));
        }
        /// <summary>
        /// Subscribe the handler for the given event and adds the unsubscription 
        /// to the DisposableContanier so it is called on Dispose.
        /// </summary>
        /// <param name="container">The container where the event unsubscription is added.</param>
        /// <param name="type">The type which contains the static event.</param>
        /// <param name="eventName">The name of the event. It is case sensitive.</param>
        /// <param name="handler">The handler which are subscripted to the event.</param>
        public static void AddEventSubscription<T1, T2, T3, T4, T5, T6, T7>(this DisposableContainer container,
            Type type,
            string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7> handler)
        {
            container.AddManagedResource(new EventSubscription(type, eventName, handler));
        }
        /// <summary>
        /// Subscribe the handler for the given event and adds the unsubscription 
        /// to the DisposableContanier so it is called on Dispose.
        /// </summary>
        /// <param name="container">The container where the event unsubscription is added.</param>
        /// <param name="type">The type which contains the static event.</param>
        /// <param name="eventName">The name of the event. It is case sensitive.</param>
        /// <param name="handler">The handler which are subscripted to the event.</param>
        public static void AddEventSubscription<T1, T2, T3, T4, T5, T6, T7, T8>(this DisposableContainer container,
            Type type,
            string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7, T8> handler)
        {
            container.AddManagedResource(new EventSubscription(type, eventName, handler));
        }
        /// <summary>
        /// Subscribe the handler for the given event and adds the unsubscription 
        /// to the DisposableContanier so it is called on Dispose.
        /// </summary>
        /// <param name="container">The container where the event unsubscription is added.</param>
        /// <param name="type">The type which contains the static event.</param>
        /// <param name="eventName">The name of the event. It is case sensitive.</param>
        /// <param name="handler">The handler which are subscripted to the event.</param>
        public static void AddEventSubscription<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DisposableContainer container,
            Type type,
            string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> handler)
        {
            container.AddManagedResource(new EventSubscription(type, eventName, handler));
        }
        /// <summary>
        /// Subscribe the handler for the given event and adds the unsubscription 
        /// to the DisposableContanier so it is called on Dispose.
        /// </summary>
        /// <param name="container">The container where the event unsubscription is added.</param>
        /// <param name="type">The type which contains the static event.</param>
        /// <param name="eventName">The name of the event. It is case sensitive.</param>
        /// <param name="handler">The handler which are subscripted to the event.</param>
        public static void AddEventSubscription<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this DisposableContainer container,
            Type type,
            string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> handler)
        {
            container.AddManagedResource(new EventSubscription(type, eventName, handler));
        }
        /// <summary>
        /// Subscribe the handler for the given event and adds the unsubscription 
        /// to the DisposableContanier so it is called on Dispose.
        /// </summary>
        /// <param name="container">The container where the event unsubscription is added.</param>
        /// <param name="type">The type which contains the static event.</param>
        /// <param name="eventName">The name of the event. It is case sensitive.</param>
        /// <param name="handler">The handler which are subscripted to the event.</param>
        public static void AddEventSubscription<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this DisposableContainer container,
            Type type,
            string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> handler)
        {
            container.AddManagedResource(new EventSubscription(type, eventName, handler));
        }
        /// <summary>
        /// Subscribe the handler for the given event and adds the unsubscription 
        /// to the DisposableContanier so it is called on Dispose.
        /// </summary>
        /// <param name="container">The container where the event unsubscription is added.</param>
        /// <param name="type">The type which contains the static event.</param>
        /// <param name="eventName">The name of the event. It is case sensitive.</param>
        /// <param name="handler">The handler which are subscripted to the event.</param>
        public static void AddEventSubscription<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this DisposableContainer container,
            Type type,
            string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> handler)
        {
            container.AddManagedResource(new EventSubscription(type, eventName, handler));
        }
        /// <summary>
        /// Subscribe the handler for the given event and adds the unsubscription 
        /// to the DisposableContanier so it is called on Dispose.
        /// </summary>
        /// <param name="container">The container where the event unsubscription is added.</param>
        /// <param name="type">The type which contains the static event.</param>
        /// <param name="eventName">The name of the event. It is case sensitive.</param>
        /// <param name="handler">The handler which are subscripted to the event.</param>
        public static void AddEventSubscription<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this DisposableContainer container,
            Type type,
            string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> handler)
        {
            container.AddManagedResource(new EventSubscription(type, eventName, handler));
        }
        /// <summary>
        /// Subscribe the handler for the given event and adds the unsubscription 
        /// to the DisposableContanier so it is called on Dispose.
        /// </summary>
        /// <param name="container">The container where the event unsubscription is added.</param>
        /// <param name="type">The type which contains the static event.</param>
        /// <param name="eventName">The name of the event. It is case sensitive.</param>
        /// <param name="handler">The handler which are subscripted to the event.</param>
        public static void AddEventSubscription<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this DisposableContainer container,
            Type type,
            string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> handler)
        {
            container.AddManagedResource(new EventSubscription(type, eventName, handler));
        }
        /// <summary>
        /// Subscribe the handler for the given event and adds the unsubscription 
        /// to the DisposableContanier so it is called on Dispose.
        /// </summary>
        /// <param name="container">The container where the event unsubscription is added.</param>
        /// <param name="type">The type which contains the static event.</param>
        /// <param name="eventName">The name of the event. It is case sensitive.</param>
        /// <param name="handler">The handler which are subscripted to the event.</param>
        public static void AddEventSubscription<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this DisposableContainer container,
            Type type,
            string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> handler)
        {
            container.AddManagedResource(new EventSubscription(type, eventName, handler));
        }
        /// <summary>
        /// Subscribe the handler for the given event and adds the unsubscription 
        /// to the DisposableContanier so it is called on Dispose.
        /// </summary>
        /// <param name="container">The container where the event unsubscription is added.</param>
        /// <param name="type">The type which contains the static event.</param>
        /// <param name="eventName">The name of the event. It is case sensitive.</param>
        /// <param name="handler">The handler which are subscripted to the event.</param>
        public static void AddEventSubscription<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this DisposableContainer container,
            Type type,
            string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> handler)
        {
            container.AddManagedResource(new EventSubscription(type, eventName, handler));
        }
    }
}
