using System;

namespace QuickEye.EventSystem
{
    [Serializable]
    public struct ValueChange<T>
    {
        public T OldValue;
        public T NewValue;

        public ValueChange(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}