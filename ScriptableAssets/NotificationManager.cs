using System;
using _Scripts.RemoteSettings;
using BlackFramework.ScriptableArchitecture;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_IOS
using UnityEngine.iOS;
using NotificationServices = UnityEngine.iOS.NotificationServices;
using Unity.Notifications.iOS;
#endif

namespace _Scripts.ScriptableAssets
{
	[TypeInfoBox("Asset který spravuje posílání push notifikací.")]
	[CreateAssetMenu(fileName = "NotificationManager", menuName = "Penguins/Create Notification Manager")]
	public class NotificationManager : SingletonScriptableObject<NotificationManager>
	{
#if UNITY_IOS
		private iOSNotification notificationSpin;
#endif
		
		public void SendSpinWheelNotification()
		{
			// dotázat se na zapnutí notifikací - jestli je má, a když ne, tak vyvolat request
			RequestRegistreNotifications();
			
            			
			// ověřit, zda je nějaká notifikace aktivní, případně ji vypnout
			CheckScheduledSpinNotification();

			// poslat novou notifikaci

			#region iOS

#if UNITY_IOS
			iOSNotificationTimeIntervalTrigger trigger = new iOSNotificationTimeIntervalTrigger()
			{
				TimeInterval =
 new TimeSpan(0, RemoteSettingsData.Instance.spinWheelSettingsData.CountDownHours, RemoteSettingsData.Instance.spinWheelSettingsData.CountDownMinutes, RemoteSettingsData.Instance.spinWheelSettingsData.CountDownSeconds),
				Repeats = false
			};

			notificationSpin = new iOSNotification()
			{
				Identifier = "notification_spin",
				Title = "Spin Wheel is ready!",
				Body = "Try your luck and get awsome reward!",
				Subtitle = "",
				ShowInForeground = true,
				ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
				CategoryIdentifier = "spin_wheel",
				ThreadIdentifier = "thread1",
				Trigger = trigger,
			};
			
			Debug.Log("Sendikng Notification iOS");
			iOSNotificationCenter.ScheduleNotification(notificationSpin);
#endif

			#endregion
		}

		private void RequestRegistreNotifications()
		{
			// iOS
#if UNITY_IOS
			Debug.Log("Request Notifications iOS");
			NotificationServices.RegisterForNotifications(NotificationType.Alert|NotificationType.Badge|NotificationType.Sound);
#endif
			
		}

		private void CheckScheduledSpinNotification()
		{
			
#if UNITY_IOS
			Debug.Log("Check Sheduled Notification iOS");
			if(notificationSpin == null) return;

			iOSNotificationCenter.RemoveScheduledNotification(notificationSpin.Identifier);
			iOSNotificationCenter.RemoveDeliveredNotification(notificationSpin.Identifier);
#endif
		}
	}
}
