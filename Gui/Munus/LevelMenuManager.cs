using _Scripts.Level;
using BlackFramework.MenuSystem;
using UnityEngine;

namespace _Scripts.Gui.Munus
{
	public class LevelMenuManager : MenuManager
	{
		public LevelMenu LevelMenuPrefab;
		public LevelFinishedMenu levelFinishedMenuPrefab;

		public void Init(LevelSettings pLevel)
		{
			LevelMenu.Show(pLevel);
		}
	}
}

