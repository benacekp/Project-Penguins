using System;
using _Scripts.Analitics;
using UnityEngine;

namespace _Scripts.Gui.Windows
{
	public class WindowTreasures : MonoBehaviour
	{
		private void OnEnable()
		{
			AnalyticsController.SendScreenVisit("treasures");
		}
	}
}
