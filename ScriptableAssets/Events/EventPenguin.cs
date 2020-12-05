using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using _Scripts.Level;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.ScriptableAssets.Events
{
	[CreateAssetMenu(fileName = "NewEventPenguin", menuName = "Penguins/Events/Event Penguin")]
	[SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
	[SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeNullComparison")]
	public class EventPenguin : ScriptableObject
	{
		public PenguinController Penguin;
		
		private readonly List<EventListenerPenguin> listeners = new List<EventListenerPenguin>();

		public void Raise()
		{
			foreach (EventListenerPenguin listener in listeners)
			{
				listener.OnEventRaised();
			}
		}

		public void RegistreListener(EventListenerPenguin pListener)
		{
			if (!listeners.Contains(pListener))
			{
				listeners.Add(pListener);
			}
		}

		public void UnregistreListener(EventListenerPenguin pListener)
		{
			if (listeners.Contains(pListener))
			{
				listeners.Remove(pListener);
			}
		}
	}

	[Serializable]
	public class PenguinEvent : UnityEvent<PenguinController>
	{
		
	}
}
