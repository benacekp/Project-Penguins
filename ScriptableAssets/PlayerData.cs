using System;
using System.Collections.Generic;
using _Scripts.Gui.GuiItems.SpinWheel;
using BlackFramework.ScriptableArchitecture;
using Plugins.BlackFramework;
using Plugins.BlackFramework.ScriptableArchitecture.Variables;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.ScriptableAssets
{
	[TypeInfoBox("Asset který udržuje veškerá data o hráčově hře.")]
	[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Penguins/New Player Data")]
	public class PlayerData : SingletonScriptableObject<PlayerData>
	{
		public enum PlayerResourceType { coins, gems, hearts }

		[SerializeField, TitleGroup("Progress:")] 
		public int LastFinishedLevel = 0;
		
		[Title("Penguins:"), InlineEditor()] public PenguinsManager PenguinsManager;

		[TitleGroup("Player Sources:")]public Dictionary<PlayerResourceType, IntReference> PlayerResources = new Dictionary<PlayerResourceType, IntReference>();

		[TitleGroup("SpinWheelCountDown")] 
		public BlackTime LastSpinTime;


		[SerializeField, TitleGroup("Audio:")] 
		public float VolumeSounds;

		[SerializeField, TitleGroup("Audio:")]
		public float VolumeMusic;

		public PlayerProgressInfo GetPlayerProgress()
		{
			return new PlayerProgressInfo(PenguinsManager.Penguins.Count, PlayerResources[PlayerResourceType.coins].Value, PlayerResources[PlayerResourceType.hearts].Value, PlayerResources[PlayerResourceType.gems].Value);
		}

		[Button(ButtonSizes.Large), GUIColor(0.9f, 0.4f, 0.1f), PropertyOrder(-1)]
		public void ResetProgress()
		{
			// level progress
			LastFinishedLevel = 0;
			
			// resources
			PlayerResources[PlayerResourceType.coins].Value = 0;
			PlayerResources[PlayerResourceType.hearts].Value = 0;
			PlayerResources[PlayerResourceType.gems].Value = 0;

			// penguins
			PenguinsManager.ResetPenguins();
		}

		public void GiveResources(ResourceMessenger pResource)
		{
			PlayerResources[pResource.Type].Value += pResource.Count;
		}

		public bool TryPay(PlayerResourceType pResource, int pValue)
		{
			if (HaveEnoughtResource(pResource, pValue))
			{
				PlayerResources[pResource].Value -= pValue;
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool HaveEnoughtResource(PlayerResourceType pResource, int pValue)
		{
			return PlayerResources[pResource].Value >= pValue;
		}

	}

	// Messenger pro data o tom, jak si hráč stojí
	public struct PlayerProgressInfo
	{
		public int Penguins { get; private set; }
		public int Coins{ get; private set; }
		public int Hearts{ get; private set; }
		public int Gems{ get; private set; }

		public PlayerProgressInfo(int pPenguins, int pCoins, int pHearts, int pGems)
		{
			Penguins = pPenguins;
			Coins = pCoins;
			Hearts = pHearts;
			Gems = pGems;
		}
	}

}
