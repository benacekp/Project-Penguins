using System;
using _Scripts.Analitics;
using UnityEngine;

namespace _Scripts.Gui.Windows
{
    public class WindowPenguins : MonoBehaviour
    {
        private void OnEnable()
        {
            AnalyticsController.SendScreenVisit("penguins");
        }
    }
}
