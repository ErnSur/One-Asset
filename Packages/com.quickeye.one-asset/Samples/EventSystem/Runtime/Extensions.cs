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
            var eventMessenger = GetOrCreateCommonEventMessenger(owner.gameObject);

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
            var eventMessenger = GetOrCreateCommonEventMessenger(owner.gameObject);

            if (eventCallbackOption.HasFlag(EventCallbackOption.ExecuteWithLastPayload))
                callback(e.LastPayload);

            if (eventCallbackOption.HasFlag(EventCallbackOption.UnsubscribeOnDisable))
                eventMessenger.Disabled += () => e.Unsubscribe(callback);

            if (eventCallbackOption.HasFlag(EventCallbackOption.UnsubscribeOnDestroy))
                eventMessenger.Destroyed += () => e.Unsubscribe(callback);


            return e;
        }

        public static GameEvent<T> SubscribeAndUnsubscribeOnDestroy<T>(this GameEvent<T> e,
            MonoBehaviour owner, UnityAction<T> callback) =>
            SubscribeAndUnsubscribeOnDestroy(e, owner.gameObject, callback);

        public static GameEvent<T> SubscribeAndUnsubscribeOnDestroy<T>(this GameEvent<T> e,
                                                                       GameObject owner, UnityAction<T> callback)
        {
            e.Subscribe(callback);
            var eventMessenger = GetOrCreateCommonEventMessenger(owner);
            eventMessenger.Destroyed += () => e.Unsubscribe(callback);
            return e;
        }

        public static GameEvent SubscribeAndUnsubscribeOnDestroy(this GameEvent e,
            MonoBehaviour owner, UnityAction callback) =>
            SubscribeAndUnsubscribeOnDestroy(e, owner.gameObject, callback);

        public static GameEvent SubscribeAndUnsubscribeOnDestroy(this GameEvent e,
                                                                 GameObject owner, UnityAction callback)
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
            var eventMessenger = GetOrCreateCommonEventMessenger(owner.gameObject);
            eventMessenger.Disabled += () => e.Unsubscribe(callback);
            return e;
        }

        public static GameEvent SubscribeAndUnsubscribeOnDisable(this GameEvent e,
            MonoBehaviour owner, UnityAction callback)
        {
            e.Subscribe(callback);
            var eventMessenger = GetOrCreateCommonEventMessenger(owner.gameObject);
            eventMessenger.Disabled += () => e.Unsubscribe(callback);
            return e;
        }

        private static CommonEventMessenger GetOrCreateCommonEventMessenger(GameObject owner)
        {
            if (!owner.TryGetComponent<CommonEventMessenger>(out var component))
            {
                component = owner.AddComponent<CommonEventMessenger>();
            }

            return component;
        }
    }
}