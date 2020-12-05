using _Scripts.Gui.GuiItems.SpinWheel;
using _Scripts.Gui.Windows;
using BlackFramework.ScriptableArchitecture;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.RemoteSettings
{
	[TypeInfoBox("Asset který udržuje veškerá data z Remote Settings")]
	[CreateAssetMenu(fileName = "RemoteSettingsData", menuName = "Penguins/Create Remote Settings Data")]
	public class RemoteSettingsData : SingletonScriptableObject<RemoteSettingsData>
	{
		[FoldoutGroup("Data:")]
		public SpinWheelSettingsData spinWheelSettingsData;



		public void Init()
		{
			UnityEngine.RemoteSettings.Completed += HandleRemoteSettingsUpdate;
		}

		private void HandleRemoteSettingsUpdate(bool wasUpdatedFromServer, bool settingsChanged, int serverResponse)
		{
			Debug.Log("RemoteSettings: Načítám remote data.");
			spinWheelSettingsData = JsonUtility.FromJson<SpinWheelSettingsData>(UnityEngine.RemoteSettings.GetString("SpinWheel"));
		}

		[Button()]
		private void ShowSpinDataInJson()
		{
			SpinWheelSettingsData data =
				new SpinWheelSettingsData(new SpinWheelWin[] {new SpinWheelWin("se", new int[] {1, 2, 3})}, 8, 0, 0, 0, 9);
			new SpinWheelWin("se", new int[] {1, 2, 3});
			
			Debug.Log(JsonUtility.ToJson(data));
		}
	}
}
