using System;
using _Scripts.Level;
using _Scripts.ScriptableAssets;
using _Scripts.ScriptableAssets.Events;
using Plugins.BlackFramework.MenuSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Scripts.Gui.Munus
{
	public class LevelMenu : SimpleMenu<LevelMenu>
	{
		[SerializeField] [ReadOnly]private LevelSettings level;
		

		public static void Show(LevelSettings pLevel)
		{
			Show();
			Instance.level = pLevel;
		}

		#region Buttons

		public void BtnMenu()
		{
			QuitLevel();
		}

		#endregion
		





		private void QuitLevel()
		{
			// nastavuji PlaySesstionData
			PlaySessionData.Instance.LevelSucceed = false;
			
			// přecházím do scéna mapy
			SceneManager.LoadScene("Map");
		}
	}
}
