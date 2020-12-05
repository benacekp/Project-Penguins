using System;
using _Scripts.Analitics;
using _Scripts.Gui.Windows;
using UnityEngine;

namespace _Scripts.Gui.Popups
{
    public class PopupBestOffer : Window
    {
        private void OnEnable()
        {
            AnalyticsController.SendScreenVisit("best_offer");
        }
    }
}
