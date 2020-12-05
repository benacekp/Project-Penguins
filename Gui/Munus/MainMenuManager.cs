using _Scripts.ScriptableAssets;
using BlackFramework.MenuSystem;
using UnityEngine;

namespace _Scripts.Gui.Munus
{
	public class MainMenuManager : MenuManager
	{
		public MainMenu MainMenuPrefab;

		
		private void Start()
		{
			PlaySessionData.Instance.Reset();
			//MainMenu.Show();
		}
	}
}
