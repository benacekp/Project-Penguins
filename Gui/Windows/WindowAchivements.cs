using System;
using _Scripts.Analitics;
using UnityEngine;

namespace _Scripts.Gui.Windows
{
	public class WindowAchivements : MonoBehaviour
	{
		private void OnEnable()
		{
			AnalyticsController.SendScreenVisit("achivements");
		}
	}
}
