using System;
using System.Collections.Generic;
using _Scripts.Gui.Windows;
using _Scripts.RemoteSettings;
using _Scripts.ScriptableAssets;
using BlackFramework.MenuSystem;
using Plugins.BlackFramework;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Gui.GuiItems.SpinWheel
{
	public class SpinWheelSpinView : MonoBehaviour
	{
		[SerializeField, ReadOnly, TitleGroup("Info:")] 
		private List<SpinWheelPrize> prizes = new List<SpinWheelPrize>();
		[SerializeField, TitleGroup("Info:"), Range(0, 7), ReadOnly] 
		private int prizeIndex;        // index výhry, která bude vytočena 
		[SerializeField, TitleGroup("Info:"), ReadOnly] 
		private bool spinWheelUsed; // udržuje, zda bylo točeton
		[SerializeField, TitleGroup("Info:")]
		public Dictionary<PlayerData.PlayerResourceType, int> maxRewards = new Dictionary<PlayerData.PlayerResourceType, int>();
		
		[SerializeField, TitleGroup("Settings:")] 
		private RectTransform[] prizeHolders;
		[SerializeField, TitleGroup("Settings:")] 
		private SpinWheelPrize prizePrefab;
		[SerializeField, TitleGroup("Settings:"), Required]
		private SpinWheerRotation wheel;   // kolo se kterým se otáčí
		[SerializeField, TitleGroup("Settings:")] 
		private Menu popupMissPrefab;

		[SerializeField, TitleGroup("Settings:")]
		private SpinRewardsController rewards;

		private bool lockSpinButton;

		public void Activate()
		{
			maxRewards.Clear();
			maxRewards.Add(PlayerData.PlayerResourceType.coins, 0);
			maxRewards.Add(PlayerData.PlayerResourceType.hearts, 0);
			
			gameObject.SetActive(true);
			GenerateWins();
			rewards.Init(maxRewards[PlayerData.PlayerResourceType.hearts],
				maxRewards[PlayerData.PlayerResourceType.coins]);
		}

		public void Deactivate()
		{
			gameObject.SetActive(false);
		}

		public void BtnSpin()
		{
			if(lockSpinButton) return;
			
			prizeIndex = Random.Range(0, prizes.Count - 1);
			wheel.Spin(prizeIndex);
			lockSpinButton = true;
		}
		
		public void OnSpinEnd()
		{
			lockSpinButton = false;
			spinWheelUsed = true;
            
			// kontroluji, jestli jsem trefil miss
			if (prizes[prizeIndex] != null)
			{
				rewards.GiveReward(prizes[prizeIndex].Reward);
				DeletePrize(prizeIndex);
			}
			else
			{
				MenuManager.Instance.OpenMenu(MenuManager.Instance.CreateInstance(popupMissPrefab));
			}
		}
		
		private void DeletePrize(int pPrizeIndex)
		{
			if (prizes[pPrizeIndex] != null)
			{
				Destroy(prizes[prizeIndex].gameObject);
			}
		}
		
		[Button]
		public void GenerateWins()
		{
			SpinWheelSettingsData data = RemoteSettingsData.Instance.spinWheelSettingsData;
			int winsCount = data.Wins.Length;
			if (winsCount > prizeHolders.Length) winsCount = prizeHolders.Length;

			ClearPrizes();
            
			List<SpinWheelWin> wins = new List<SpinWheelWin>(data.Wins);
			SpinWheelWin rndWin;
			// generuji výhry
			for (int i = 0; i < winsCount; i++)
			{
				SpinWheelPrize prize = Instantiate(prizePrefab);
				rndWin = wins[Random.Range(0, wins.Count - 1)];
				ResourceMessenger resourceCount = prize.Init(rndWin, prizeHolders[i]);
				maxRewards[resourceCount.Type] += resourceCount.Count;
				wins.Remove(rndWin);
				prizes.Add(prize);
			}
		}
		
		private void ClearPrizes()
		{
			foreach (SpinWheelPrize prize in prizes)
			{
				if (prize != null)
				{
					Destroy(prize.gameObject);
				}
			}
            
			prizes.Clear();
		}
		
		private void OnDisable()
		{
			if (spinWheelUsed)
			{
				OnCloseUsedSpinWheel();
			}
		}

		private void OnCloseUsedSpinWheel()
		{
			PlayerData.Instance.LastSpinTime = new BlackTime(DateTime.Now);
			NotificationManager.Instance.SendSpinWheelNotification();
		}
	}
	
	/// <summary>
	/// Udržuje data položky do kola štěstí, ze které se generuje výhra
	/// </summary>
	[Serializable]
	public struct SpinWheelWin
	{
		public string win_type;
		public int[]values;

		public SpinWheelWin(string pWinType, int[] pValues)
		{
			win_type = pWinType;
			values = pValues;
		}
	}
}
