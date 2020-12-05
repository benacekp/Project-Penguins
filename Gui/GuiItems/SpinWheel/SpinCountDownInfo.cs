using System;
using System.Collections;
using _Scripts.RemoteSettings;
using _Scripts.ScriptableAssets;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Scripts.Gui.GuiItems.SpinWheel
{
    public class SpinCountDownInfo : MonoBehaviour
    {
        [SerializeField, TitleGroup("Settings:")] 
        private Text txt_skipPrize;
        [SerializeField, TitleGroup("Settings:")] 
        private TextMeshProUGUI txt_countDown;


        [SerializeField, TitleGroup("Events:")] 
        private UnityEvent OnPayedSkip,OnNoCoins, CountDownFinished;

        private int skipPrize;
        private DateTime targetTime;
        private int countDownSecondsLeft;
        private TimeSpan timeToCountDownFinish;

        public void Activate()
        {
            gameObject.SetActive(true);
            
            skipPrize = RemoteSettingsData.Instance.spinWheelSettingsData.CountDownSkipPrize;
            txt_skipPrize.text = skipPrize.ToString();
            
            // kontrola count down
            targetTime = PlayerData.Instance.LastSpinTime.GetDateTime();
            targetTime = targetTime.AddHours(RemoteSettingsData.Instance.spinWheelSettingsData.CountDownHours);
            targetTime = targetTime.AddMinutes(RemoteSettingsData.Instance.spinWheelSettingsData.CountDownMinutes);
            targetTime = targetTime.AddSeconds(RemoteSettingsData.Instance.spinWheelSettingsData.CountDownSeconds);
            StartCoroutine(CheckCountDown());
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public void BtnSkipCountDown()
        {
            if (PlayerData.Instance.TryPay(PlayerData.PlayerResourceType.coins, skipPrize))
            {
                OnPayedSkip.Invoke();
            }
            else
            {
                OnNoCoins.Invoke();
            }
        }

        private IEnumerator CheckCountDown()
        {
            timeToCountDownFinish = targetTime - DateTime.Now;
            while (timeToCountDownFinish.TotalSeconds > 0)
            {
                timeToCountDownFinish = targetTime - DateTime.Now;
                txt_countDown.text = timeToCountDownFinish.Hours + "h  " + timeToCountDownFinish.Minutes + "min  " + timeToCountDownFinish.Seconds + "s";
                yield return new WaitForSeconds(1);
            }
            
            OnCountDownFinished();
        }

        private void OnCountDownFinished()
        {
            StopCoroutine(CheckCountDown());
            CountDownFinished.Invoke();
        }
    }
}
