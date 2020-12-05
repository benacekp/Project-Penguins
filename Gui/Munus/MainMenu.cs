using _Scripts.Analitics;
using BlackFramework.MenuSystem;
using Plugins.BlackFramework.MenuSystem;
using UnityEngine.SceneManagement;

namespace _Scripts.Gui
{
	public class MainMenu : SimpleMenu<MainMenu>
	{
		public void PlayButton()
		{
			AnalyticsController.SendFirstInteraction();
			SceneManager.LoadScene("Map");
		}
	}
}
