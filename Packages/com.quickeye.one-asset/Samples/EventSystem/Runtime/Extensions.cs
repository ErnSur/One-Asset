using UnityEngine;
using UnityEngine.Events;

namespace QuickEye.EventSystem
{
    public static class GameEventExtensions
    {
        public static GameEvent Subscribe(this GameEvent e,
            MonoBehaviour owner, UnityAction callback, EventCallbackOption eventCallbackOption)
        {
            e.Subscribe(callback);
            var eventMessenger = GetOrCreateCommonEventMessenger(owner);

            if (eventCallbackOption.HasFlag(EventCallbackOption.ExecuteWithLastPayload))
                callback();

            if (eventCallbackOption.HasFlag(EventCallbackOption.UnsubscribeOnDisable))
                eventMessenger.Disabled += () => e.Unsubscribe(callback);

            if (eventCallbackOption.HasFlag(EventCallbackOption.UnsubscribeOnDestroy))
                eventMessenger.Destroyed += () => e.Unsubscribe(callback);


            return e;
        }

        public static GameEvent<T> Subscribe<T>(this GameEvent<T> e,
            MonoBehaviour owner, UnityAction<T> callback, EventCallbackOption eventCallbackOption)
        {
            e.Subscribe(callback);
            var eventMessenger = GetOrCreateCommonEventMessenger(owner);

            if (eventCallbackOption.HasFlag(EventCallbackOption.ExecuteWithLastPayload))
                callback(e.LastPayload);

            if (eventCallbackOption.HasFlag(EventCallbackOption.UnsubscribeOnDisable))
                eventMessenger.Disabled += () => e.Unsubscribe(callback);

            if (eventCallbackOption.HasFlag(EventCallbackOption.UnsubscribeOnDestroy))
                eventMessenger.Destroyed += () => e.Unsubscribe(callback);


            return e;
        }

        public static GameEvent<T> SubscribeAndUnsubscribeOnDestroy<T>(this GameEvent<T> e,
            MonoBehaviour owner, UnityAction<T> callback)
        {
            e.Subscribe(callback);
            var eventMessenger = GetOrCreateCommonEventMessenger(owner);
            eventMessenger.Destroyed += () => e.Unsubscribe(callback);
            return e;
        }

        public static GameEvent SubscribeAndUnsubscribeOnDestroy(this GameEvent e,
            MonoBehaviour owner, UnityAction callback)
        {
            e.Subscribe(callback);
            var eventMessenger = GetOrCreateCommonEventMessenger(owner);
            eventMessenger.Destroyed += () => e.Unsubscribe(callback);
            return e;
        }

        public static GameEvent<T> SubscribeAndUnsubscribeOnDisable<T>(this GameEvent<T> e,
            MonoBehaviour owner, UnityAction<T> callback)
        {
            e.Subscribe(callback);
            var eventMessenger = GetOrCreateCommonEventMessenger(owner);
            eventMessenger.Disabled += () => e.Unsubscribe(callback);
            return e;
        }

        public static GameEvent SubscribeAndUnsubscribeOnDisable(this GameEvent e,
            MonoBehaviour owner, UnityAction callback)
        {
            e.Subscribe(callback);
            var eventMessenger = GetOrCreateCommonEventMessenger(owner);
            eventMessenger.Disabled += () => e.Unsubscribe(callback);
            return e;
        }

        static CommonEventMessenger GetOrCreateCommonEventMessenger(MonoBehaviour owner)
        {
            if (!owner.gameObject.TryGetComponent<CommonEventMessenger>(out var component))
            {
                component = owner.gameObject.AddComponent<CommonEventMessenger>();
            }

            return component;
        }
    }
}