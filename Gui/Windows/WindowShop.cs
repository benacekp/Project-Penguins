using System;
using _Scripts.Analitics;
using UnityEngine;

namespace _Scripts.Gui.Windows
{
    public class WindowShop : MonoBehaviour
    {
        private void OnEnable()
        {
            AnalyticsController.SendScreenVisit("shop");
        }
    }
}
