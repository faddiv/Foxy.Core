using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace CommonLibraries.Core.Cleanup
{
    /// <summary>
    /// Subscribe the provided handler to the provided event and on calling Dispose it unsubscribe it.
    /// </summary>
    [SuppressMessage("Design", "CA1063:Implement IDisposable Correctly",
        Justification = "Does not have unmanaged resources.")]
    public class EventSubscription : IDisposable
    {
        private readonly EventInfo _eventInfo;
        private readonly Delegate _handler;
        private readonly object _instance;

        /// <summary>
        /// Creates an EventSubscription for a static event.
        /// </summary>
        /// <param name="componentType"></param>
        /// <param name="eventName"></param>
        /// <param name="handler"></param>
        public EventSubscription(Type componentType, string eventName, Delegate handler)
        {
            _instance = null;
            _eventInfo = GetEventInfo(componentType, eventName);
            _handler = EnsureDelegateType(handler);
            _eventInfo.AddEventHandler(_instance, _handler);
        }

        /// <summary>
        /// Creates an EventSubscription for an instance level event.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="eventName"></param>
        /// <param name="handler"></param>
        public EventSubscription(object instance, string eventName, Delegate handler)
        {
            _instance = instance;
            _eventInfo = GetEventInfo(instance.GetType(), eventName);
            _handler = EnsureDelegateType(handler);
            _eventInfo.AddEventHandler(_instance, _handler);
        }

        private static EventInfo GetEventInfo(Type instanceType, string eventName)
        {
            var events = instanceType.GetEvents(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
            var eventInfo = events.FirstOrDefault(e => e.Name == eventName);
            return eventInfo;
        }

        private Delegate EnsureDelegateType(Delegate handler)
        {
            var internalHandler = handler;
            if (_eventInfo.EventHandlerType != handler.GetType())
            {
                Delegate[] delegates = handler.GetInvocationList();
                if (delegates.Length == 1)
                {

                    internalHandler = CreateDelegate(delegates, 0);
                }
                else
                {
                    Delegate[] delegatesDest = new Delegate[delegates.Length];
                    for (int index = 0; index < delegates.Length; index++)
                    {
                        delegatesDest[index] = CreateDelegate(delegates, index);
                    }
                    internalHandler = Delegate.Combine(delegatesDest);
                }
            }

            return internalHandler;
        }

        private Delegate CreateDelegate(Delegate[] delegates, int index)
        {
            return Delegate.CreateDelegate(_eventInfo.EventHandlerType,
                                    delegates[0].Target, delegates[0].Method);
        }

        /// <summary>
        /// Unsubscribe the eventhandler from the event.
        /// </summary>
        public void Dispose()
        {
            _eventInfo.RemoveEventHandler(_instance, _handler);
        }
    }
}
