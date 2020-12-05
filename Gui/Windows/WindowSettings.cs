using System;
using _Scripts.Analitics;
using _Scripts.Gui.GuiItems.Sliders;
using _Scripts.ScriptableAssets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Scripts.Gui.Windows
{
	public class WindowSettings : MonoBehaviour
	{
		[SerializeField] private SliderMusic sliderMusic;
		[SerializeField] private SliderSound sliderSounds;
		
		private float volumeMusic;
		private float volumeSounds;
		
		private void OnEnable()
		{
			AnalyticsController.SendScreenVisit("settings");
			volumeMusic = PlayerData.Instance.VolumeMusic;
			volumeSounds = PlayerData.Instance.VolumeSounds;
		}

		public void BtnResetGameProgress()
		{
			PlayerData.Instance.ResetProgress();
			SceneManager.LoadScene("Map");
		}

		public void Reset()
		{
			sliderMusic.OnSliderValueChange(PlayerData.Instance.VolumeMusic = volumeMusic);
			sliderSounds.OnSliderValueChange(PlayerData.Instance.VolumeSounds = volumeSounds);
			
		}
	}
}
