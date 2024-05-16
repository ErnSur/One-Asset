using System;

namespace QuickEye.EventSystem
{
    [Flags]
    public enum EventCallbackOption
    {
        None = 0,
        /// <summary>
        /// Callback will be unsubscribed when the owner is disabled.
        /// </summary>
        UnsubscribeOnDisable = 1,

        /// <summary>
        /// Callback will be unsubscribed when the owner is destroyed.
        /// </summary>
        UnsubscribeOnDestroy = 2,

        /// <summary>
        /// Callback will be executed with the last payload.
        /// </summary>
        ExecuteWithLastPayload = 4,

        /// <summary>
        /// Callback will be executed with the last payload and will be unsubscribed when the owner is disabled.
        /// </summary>
        ExecuteAndListenUntilDisabled = UnsubscribeOnDisable | ExecuteWithLastPayload,

        /// <summary>
        /// Callback will be executed with the last payload and will be unsubscribed when the owner is destroyed.
        /// </summary>
        ExecuteAndListenUntilDestroyed = UnsubscribeOnDestroy | ExecuteWithLastPayload,
    }
}