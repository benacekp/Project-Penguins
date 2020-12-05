using System;
using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Gui.GuiItems.Sliders
{
	public class ProgressBar : MonoBehaviour
	{
		[SerializeField, FoldoutGroup("Info:"), OnValueChanged("SetValueFix"), PropertyRange(0, "maxValue")]
		private int value;
		[SerializeField, FoldoutGroup("Info:")]
		private int maxValue = 100;

		[SerializeField, FoldoutGroup("Settings:")]
		private Image fill;
		[SerializeField, FoldoutGroup("Settings:")]
		private TextMeshProUGUI txt_value;

		private int targetValue; // pomocná proměnná pro animování
		
		public void Init(int pMaxValue, int pValue)
		{
			maxValue = pMaxValue;
			SetValueFix(pValue);
		}

		[Button]
		public void SetValueAnim(int pValue)
		{
			targetValue = pValue;
			StartCoroutine(ValueAnimation());
		}

		public void SetValueFix(int pValue)
		{
			value = pValue;
			fill.fillAmount = (float)value / maxValue;
			txt_value.text = value.ToString();
		}

		private IEnumerator ValueAnimation()
		{
			while (Mathf.Abs(value - targetValue) > 0)
			{
				SetValueFix(value+1);
				yield return new WaitForSeconds(0.05f);
			}
		}

		private void OnDisable()
		{
			SetValueFix(targetValue);
			StopAllCoroutines();
		}
	}
}
