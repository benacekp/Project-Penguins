using System;
using BlackFramework.MenuSystem;

namespace _Scripts.Gui.Munus
{
    public class MapMenuManager : MenuManager
    {
        public MapMenu MapMenuPrefab;
        public StartLevelMenu StartLevelMenuPrefab;

        private void Start()
        {
            MapMenu.Show();
        }
    }
}
