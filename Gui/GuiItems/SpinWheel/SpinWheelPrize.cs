using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using _Scripts.Gui.Windows;
using _Scripts.ScriptableAssets;
using _Scripts.ScriptableAssets.SpinWheelWins;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _Scripts.Gui.GuiItems.SpinWheel
{
	public class SpinWheelPrize : MonoBehaviour
	{
		[SerializeField] private SpinWheelWin initData;
		[SerializeField] private Image img_icon;
		[SerializeField] private Text txt_value;
		[SerializeField, ReadOnly] private int value;

		public ResourceMessenger Reward { get; private set; }

		private RectTransform trans;
		
		private void Awake()
		{
			trans = gameObject.GetComponent<RectTransform>();
		}

		public ResourceMessenger Init(SpinWheelWin pInitData, RectTransform pParen)
		{
			initData = pInitData;
			trans.SetParent(pParen);
			trans.position = pParen.position;
			trans.rotation = pParen.rotation;
			trans.localScale = Vector3.one;
			
			SpinWheelWinData data = SpinWheelWinsManager.Instance.GetData(pInitData.win_type);
			if (data != null)
			{
				// nastavit ikonu
				img_icon.sprite = data.Icon;
				// nastavit náhodnou hodnotu
				value = pInitData.values[Random.Range(0, pInitData.values.Length - 1)];
				txt_value.text = value.ToString();
				Reward = new ResourceMessenger(data.Id, value);
				return Reward;
			}
			else
			{
				Debug.Log("no data");
			}

			return null;
		}
	}

	[Serializable]
	public class ResourceMessenger
	{
		public PlayerData.PlayerResourceType Type;
		public int Count;

		public ResourceMessenger(PlayerData.PlayerResourceType pType, int pCount)
		{
			Type = pType;
			Count = pCount;
		}

		public ResourceMessenger(string pType, int pCount)
			: this(ResourceTypeFromString(pType), pCount)
		{
		}

		public static PlayerData.PlayerResourceType ResourceTypeFromString(string pType)
		{
			switch (pType)
			{
				case "coins":
					return PlayerData.PlayerResourceType.coins;
				case "hearts":
					return PlayerData.PlayerResourceType.hearts;
				case "gems":
					return PlayerData.PlayerResourceType.gems;
				default:
					throw new Exception("String to PlayerResourceType Error - " + pType);
			}
		}
	}

}
