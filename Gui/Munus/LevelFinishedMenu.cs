using System.Collections.Generic;
using System.Linq;
using _Scripts.Analitics;
using _Scripts.Level;
using _Scripts.ScriptableAssets;
using Plugins.BlackFramework.MenuSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Scripts.Gui.Munus
{
	public class LevelFinishedMenu : SimpleMenu<LevelFinishedMenu>
	{
		[SerializeField, Required] private Text txt_Label, txt_Survivals, txt_Losts;
		
		public static void Show(LevelFinishData pFinishData)
		{
			Show();
			
			
			// vymazu hraci ztracené penguiny
			SeizeLostPenguins(pFinishData);
			
			
			if (pFinishData.LevelState == LevelController.LevelStateType.Won)
			{
				Instance.ShowSuccess();
				AnalyticsController.SendLevelCompleted(pFinishData, PlayerData.Instance.GetPlayerProgress());
				PlayerData.Instance.LastFinishedLevel = pFinishData.LevelIndex;
			}
			else
			{
				Instance.ShowFail();
				AnalyticsController.SendLevelFail(pFinishData.LevelName, pFinishData.SecundsInLevel);
			}

			// zobrazím seznamy v gui
			ShowFinishData(pFinishData);
		}

		private static void SeizeLostPenguins(LevelFinishData pFinishData)
		{
			foreach (PenguinData lost in pFinishData.PenguinsLost)
			{
				PlayerData.Instance.PenguinsManager.Penguins.Remove(lost);
			}
		}

		private static void ShowFinishData(LevelFinishData pFinishData)
		{
			Instance.ShowPenguinsByState(pFinishData.PenguinsSurvivals, PenguinController.EPenguinState.Survived,
				Instance.txt_Survivals);
			Instance.ShowPenguinsByState(pFinishData.PenguinsLost, PenguinController.EPenguinState.Lost, Instance.txt_Losts);
		}

		private void ShowPenguinsByState(List<PenguinData> pPenguins, PenguinController.EPenguinState pState, Text pText)
		{
			string text;
			
			switch (pState)
			{
				case PenguinController.EPenguinState.Survived:
					text = "Survivals: ";
					break;
				case PenguinController.EPenguinState.Lost:
					text = "Losts: ";
					break;
				default:
					text = "";
					break;
			}
			
			foreach (var penguin in pPenguins)
			{
				text += ", " + penguin.Name;
			}

			pText.text = text;
		}

		public void BtnMainMenu()
		{
			// nastavuji PlaySessionData
			PlaySessionData.Instance.LevelSucceed = true;
			
			// přecházím do scény mapy
			//SceneManager.LoadScene("Map");
		}

		private void ShowSuccess()
		{
			//txt_Label.color = Color.green;
			//txt_Label.text = "Level Success";
		}

		private void ShowFail()
		{
			//txt_Label.color = Color.red;
			//txt_Label.text = "Level Fail";
		}
	}
}
