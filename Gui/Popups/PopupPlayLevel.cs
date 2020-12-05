using System;
using _Scripts.ScriptableAssets;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Gui.Popups
{
    public class PopupPlayLevel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        private void OnEnable()
        {
            Init();
        }

        private void Init()
        {
            text.text = PlaySessionData.Instance.Level.LevelName;
        }

        public void GoToLevelScene()
        {
            SceneManager.LoadScene("Level");
        }
    }
}
