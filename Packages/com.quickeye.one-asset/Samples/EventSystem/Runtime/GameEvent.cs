using UnityEngine;
using UnityEngine.Events;

namespace QuickEye.EventSystem
{
    // TODO:
    // Generate Event asset as a GameEvents.asset sub-asset
        // assets should be regenerated on recompile
        // This would prevent old unused assets from piling up. Con: This would also delete assets that may still be referenced somewhere...
        // TODO: how to handle event renames?
        // This would force correct names for all of the events at all times
        // easier to find events in the project
    // Every Game Event should have it's own unique ID different from the name so that renames don't break the references
    public abstract class GameEvent<TArgs> : GameEventBase, IInvokable
    {
        [SerializeField]
        TArgs _lastPayload;

        [SerializeField]
        UnityEvent<TArgs> _event = new UnityEvent<TArgs>();

        public UnityEvent<TArgs> Event => _event;
        public TArgs LastPayload => _lastPayload;

        public void Subscribe(UnityAction<TArgs> callback)
        {
            Event.AddListener(callback);
        }

        public void Unsubscribe(UnityAction<TArgs> callback)
        {
            Event.RemoveListener(callback);
        }

        public void Invoke(TArgs payload)
        {
            Debug.Log($"[!] Invoke: {name} with: {payload}");
            Event?.Invoke(_lastPayload = payload);
            wasInvoked = true;
        }
        protected override void ResetValues()
        {
            base.ResetValues();
            _event = new UnityEvent<TArgs>();
            _lastPayload = default;
        }

        void IInvokable.RepeatLastInvoke() => Invoke(_lastPayload);
    }

    public abstract class GameEvent : GameEventBase, IInvokable
    {
        [SerializeField]
        UnityEvent _event = new UnityEvent();

        public UnityEvent Event => _event;

        public void Subscribe(UnityAction callback)
        {
            Event.AddListener(callback);
#if UNITY_EDITOR
            //UnityEditor.Events.UnityEventTools.AddPersistentListener(Event, callback);
#else
#endif
        }

        public void Unsubscribe(UnityAction callback)
        {
            Event.RemoveListener(callback);
#if UNITY_EDITOR
            //UnityEditor.Events.UnityEventTools.RemovePersistentListener(Event, callback);
#else
#endif
        }

        public void Invoke()
        {
            Debug.Log($"[!] Invoke: {name}");
            Event?.Invoke();
            wasInvoked = true;
        }

        protected override void ResetValues()
        {
            base.ResetValues();
            _event = new UnityEvent();
        }

        void IInvokable.RepeatLastInvoke() => Invoke();
    }
}