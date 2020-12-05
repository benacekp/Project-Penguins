using System;
using _Scripts.Gui.GuiItems.Sliders;
using _Scripts.ScriptableAssets;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.Gui.GuiItems.SpinWheel
{
    public class SpinRewardsController : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        private int coinsReward, heartsReward;

        [SerializeField]
        private ProgressBar coinsProgressbar, heartsProgressBar;
        
        [SerializeField] private UnityEvent OnPlayerTakesReward = new UnityEvent();

        public void Init(int heardsMaxReward, int coinsMaxReward)
        {
            coinsReward = 0;
            heartsReward = 0;
            
            coinsProgressbar.Init(coinsMaxReward, 0);
            heartsProgressBar.Init(heardsMaxReward, 0);
        }

        [Button]
        public void GiveReward(ResourceMessenger pReward)
        {
            switch (pReward.Type)
            {
                case PlayerData.PlayerResourceType.coins:
                    coinsReward += pReward.Count;
                    coinsProgressbar.SetValueAnim(coinsReward);
                    break;
                case PlayerData.PlayerResourceType.gems:
                    break;
                case PlayerData.PlayerResourceType.hearts:
                    heartsReward += pReward.Count;
                    heartsProgressBar.SetValueAnim(heartsReward);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(pReward.Type), pReward.Type, null);
            }
        }

        public void PlayerTakesRewards()
        {
            PlayerData.Instance.GiveResources(new ResourceMessenger(PlayerData.PlayerResourceType.coins, coinsReward));
            PlayerData.Instance.GiveResources(new ResourceMessenger(PlayerData.PlayerResourceType.hearts, heartsReward));
            OnPlayerTakesReward.Invoke();
        }
    }
}
