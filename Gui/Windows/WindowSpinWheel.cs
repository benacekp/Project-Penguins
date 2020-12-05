using System;
using _Scripts.Analitics;
using _Scripts.Gui.GuiItems.SpinWheel;
using UnityEngine;

namespace _Scripts.Gui.Windows
{
    public class WindowSpinWheel : MonoBehaviour
    {

        [SerializeField]
        private SpinWheelSpinView spinView;

        [SerializeField]
        private SpinCountDownInfo countDownView;


        private void OnEnable()
        {
            AnalyticsController.SendScreenVisit("spin_wheel");
        }

        private void Start()
        {
            spinView.Deactivate();
            countDownView.Activate();
        }

        public void OnCountDownFinished()
        {
            countDownView.Deactivate();
            spinView.Activate();
        }
    }



    [Serializable]
    public struct SpinWheelSettingsData
    {
        public SpinWheelWin[] Wins;
        public int ContinueCost;
        public int CountDownHours;
        public int CountDownMinutes;
        public int CountDownSeconds;
        public int CountDownSkipPrize;

        public SpinWheelSettingsData(SpinWheelWin[] pWinds, int pContinueCost, int pCountDownHours, int pCountDownMinutes, int pCountDownSeconds, int pCountDownSkipPrize)
        {
            Wins = pWinds;
            ContinueCost = pContinueCost;
            CountDownHours = pCountDownHours;
            CountDownMinutes = pCountDownMinutes;
            CountDownSeconds = pCountDownSeconds;
            CountDownSkipPrize = pCountDownSkipPrize;
        }
    }
}
