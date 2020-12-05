using System;
using _Scripts.ScriptableAssets;
using Plugins.BlackFramework.MenuSystem;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Scripts.Gui.Munus
{
	public class StartLevelMenu : SimpleMenu<StartLevelMenu>
	{
		[SerializeField] private Text txtLevelName;

		private void OnEnable()
		{
			InitWindow();
		}


		public void BtnPlayLevel()
		{
			SceneManager.LoadScene("Level");
		}

		private void InitWindow()
		{
			txtLevelName.text = PlaySessionData.Instance.Level.LevelName;
		}
	}
}
