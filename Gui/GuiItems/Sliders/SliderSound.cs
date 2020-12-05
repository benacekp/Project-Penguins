using System;
using _Scripts.ScriptableAssets;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace _Scripts.Gui.GuiItems.Sliders
{
	[RequireComponent(typeof(AudioSource))]
	public class SliderSound : MonoBehaviour
	{
		[SerializeField] private Slider slider;
		[SerializeField] private AudioMixer audioMixer;
		private AudioSource audioSource;
		
		private void Awake()
		{
			audioSource = gameObject.GetComponent<AudioSource>();
		}
		
		private void OnEnable()
		{
			audioMixer.SetFloat("volume", PlayerData.Instance.VolumeSounds);
			slider.value = PlayerData.Instance.VolumeSounds;
		}

		public void OnSliderValueChange(float pValue)
		{
			audioMixer.SetFloat("volume", pValue);
			PlayerData.Instance.VolumeSounds = pValue;
			if (audioSource!= null && !audioSource.isPlaying)
			{
				audioSource.Play();
			}
		}
	}
}
