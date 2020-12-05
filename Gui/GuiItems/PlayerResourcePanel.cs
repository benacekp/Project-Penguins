using Plugins.BlackFramework.ScriptableArchitecture.Variables;
using TMPro;
using UnityEngine;

namespace _Scripts.Gui.GuiItems
{
    public class PlayerResourcePanel : MonoBehaviour
    {
        [SerializeField] private IntVarialble value;
        [SerializeField] private TextMeshProUGUI txt_value;
        
        
        private void Awake()
        {
            OnValueChange();
        }

        private void OnEnable()
        {
            value.ChangeEvent.AddListener(OnValueChange);
        }

        private void OnDisable()
        {
            value.ChangeEvent.RemoveListener(OnValueChange);
        }

        public void OnValueChange()
        {
            txt_value.text = value.Value.ToString();
        }
    }
}
