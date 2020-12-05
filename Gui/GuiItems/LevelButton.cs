using System;
using _Scripts.Gui.Munus;
using _Scripts.Level;
using _Scripts.ScriptableAssets;
using BlackFramework.MenuSystem;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace _Scripts.Gui.GuiItems
{
	public class LevelButton : MonoBehaviour
	{
		private enum LevelButonState { Finished, Locked, Opened }

		[SerializeField, EnumToggleButtons, OnValueChanged("SetState")] private LevelButonState state;
		[SerializeField] private TextMeshPro txt_LevelIndex;
		
		[SerializeField, TitleGroup("Stete Efects:")] private GameObject lockEfect;
		[SerializeField, TitleGroup("Stete Efects:")] private GameObject openedEfect;
		[SerializeField, TitleGroup("Stete Efects:")] private GameObject finishedEffect;

		[SerializeField, TitleGroup("Next Windows:")] private Menu PopupFinishedPrefab;
		[SerializeField, TitleGroup("Next Windows:")] private Menu PopupLockedPrefab;
		[SerializeField, TitleGroup("Next Windows:")] private Menu PopupPlayPrefab;
		
		
		[SerializeField][AssetSelector(Paths = "Assets/_Prefabs/Levels")] [Required, InlineEditor(), OnValueChanged("Init")]
		private LevelSettings levelPrefab;

		private void Awake()
		{
#if UNITY_EDITOR
			TestDataSet();
#endif
			Init();
		}

		private void Init()
		{
			if(levelPrefab == null)
				return;

			// cislo levelu
			txt_LevelIndex.text = levelPrefab.LevelIndex.ToString();

			// nastav state
			if (levelPrefab.LevelIndex < PlayerData.Instance.LastFinishedLevel+1)
				SetState(LevelButonState.Finished);
			else if (levelPrefab.LevelIndex > PlayerData.Instance.LastFinishedLevel+1)
				SetState(LevelButonState.Locked);
			else if (levelPrefab.LevelIndex == PlayerData.Instance.LastFinishedLevel+1)
				SetState(LevelButonState.Opened);
			
		}

		public void OnClick()
		{
			Debug.Log("LevelButton.OnClick()");
			switch (state)
			{
				case LevelButonState.Opened:
					PlaySessionData.Instance.LevelSucceed = false;
					PlaySessionData.Instance.Level = levelPrefab;
					MenuManager.Instance.OpenMenu(MenuManager.Instance.CreateInstance(PopupPlayPrefab));
					break;
				case LevelButonState.Locked:
					MenuManager.Instance.OpenMenu(MenuManager.Instance.CreateInstance(PopupLockedPrefab));
					break;
				case LevelButonState.Finished:
					MenuManager.Instance.OpenMenu(MenuManager.Instance.CreateInstance(PopupFinishedPrefab));
					break;
			}
		}

		private void SetState(LevelButonState pState)
		{
			state = pState;
			
			switch (state)
			{
				case LevelButonState.Opened:
					lockEfect.SetActive(false);
					finishedEffect.SetActive(false);
					openedEfect.SetActive(true);
					break;
				case LevelButonState.Locked: 
					lockEfect.SetActive(true);
					finishedEffect.SetActive(false);
					openedEfect.SetActive(true);
					break;
				case LevelButonState.Finished: 
					lockEfect.SetActive(false);
					finishedEffect.SetActive(true);
					openedEfect.SetActive(false);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			
		}
		
#if UNITY_EDITOR
		private void TestDataSet()
		{
			if (levelPrefab == null) Debug.LogError("LevelButton obj_"+gameObject.name+" nemá nastaven level prefab;");
		}
#endif

	}
}
