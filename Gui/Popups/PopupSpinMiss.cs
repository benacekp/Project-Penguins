using System.Xml.Serialization;
using _Scripts.AdMob;
using _Scripts.Gui.Munus;
using _Scripts.RemoteSettings;
using _Scripts.ScriptableAssets;
using BlackFramework.MenuSystem;
using GoogleMobileAds.Api;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Scripts.Gui.Popups
{
    public class PopupSpinMiss : MonoBehaviour
    {
        [SerializeField] private int playOnCost;
        [SerializeField] private Text txt_continueCost;

        /*[SerializeField] private AdMobController adMob;*/
        
        [SerializeField] private UnityEvent OnContinueNew;
        [SerializeField] private UnityEvent OnGiveUp;
        [SerializeField] private UnityEvent OnNoCoins;
        [SerializeField] private UnityEvent OnGetAdReward;

        [SerializeField] private GameObject btn_AdRewardContinue;
        
        private RewardedAd ad;
        
        private void Start()
        {
            btn_AdRewardContinue.SetActive(false);
            
            playOnCost = RemoteSettingsData.Instance.spinWheelSettingsData.ContinueCost;
            txt_continueCost.text = playOnCost.ToString();


            //Todo_PB: spouštět revard reklamu
            // ad = adMob.GetRewardedAd("SpinWheelContinue");

            BtnPlayOnAd();
            
        }
        
        

        public void BtnGiveUp()
        {
            OnGiveUp.Invoke();
        }

        public void BtnPlayOnNew()
        {
            
            if (PlayerData.Instance.TryPay(PlayerData.PlayerResourceType.coins, playOnCost))
            {
                Debug.Log("probehla platba");
                OnContinueNew.Invoke();
            }
            else
            {
                Debug.Log("no money - best offer");
                OnNoCoins.Invoke();
            }
            
        }

        public void BtnPlayOnAd()
        {
            Debug.Log("btn_showAd - chci zobrazit reklamu");
            ad.Show();
        }
        
        /*** AD EVENTS ***/

        public void ShowAdRewardButton()
        {
            Debug.Log("Ad Loaded - show button");
            btn_AdRewardContinue.SetActive(true);
        }

        public void GiveReward()
        {
            OnGetAdReward.Invoke();
        }

    }
}
