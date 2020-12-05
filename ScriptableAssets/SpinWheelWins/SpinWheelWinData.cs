using System;
using _Scripts.Gui.GuiItems.SpinWheel;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.ScriptableAssets.SpinWheelWins
{
    [Serializable]
    public class SpinWheelWinData : ScriptableObject
    {
        public string Id;
        [PreviewField]public Sprite Icon;

        public ResourceMessenger JsonTest;
        public string TestSourceJson;
        
        [Button()]
        private void TextJson()
        {
            ResourceMessenger resource = new ResourceMessenger(PlayerData.PlayerResourceType.coins, 10);
            Debug.Log("ToJson: " + JsonUtility.ToJson(resource));
        }

        [Button]
        private void FromJson()
        {
            JsonTest = JsonUtility.FromJson<ResourceMessenger>(TestSourceJson);
        }
    }
}
