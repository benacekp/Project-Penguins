using System;
using Plugins.BlackFramework.ScriptableArchitecture.Events;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.ScriptableAssets.Events
{
    public class EventListenerPenguin : MonoBehaviour
    {
        public EventPenguin Event;

        public UnityEventPenguin Response;

        private void OnEnable()
        {
            Event.RegistreListener(this);
        }

        private void OnDisable()
        {
            Event.UnregistreListener(this);
        }

        public void OnEventRaised()
        {
            Response.Invoke(Event);
        }
    }

    [Serializable]
    public class UnityEventPenguin : UnityEvent<EventPenguin>
    {
    }
}
