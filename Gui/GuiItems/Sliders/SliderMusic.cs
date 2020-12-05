using System;
using _Scripts.ScriptableAssets;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace _Scripts.Gui.GuiItems.Sliders
{
	public class SliderMusic : MonoBehaviour
	{
		[SerializeField] private Slider slider;
		[SerializeField] private AudioMixer audioMixer;
		
		
		private void OnEnable()
		{
			audioMixer.SetFloat("volume", PlayerData.Instance.VolumeMusic);
			slider.value = PlayerData.Instance.VolumeMusic;
		}

		public void OnSliderValueChange(float pValue)
		{
			audioMixer.SetFloat("volume", pValue);
			PlayerData.Instance.VolumeMusic = pValue;
		}
	}
}
