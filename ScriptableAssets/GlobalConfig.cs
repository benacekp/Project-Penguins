using BlackFramework.ScriptableArchitecture;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;

namespace _Scripts.ScriptableAssets
{
    [TypeInfoBox("Konfigurační data hry.")]
    [CreateAssetMenu(fileName = "NewGlobalConfig", menuName = "Penguins/New Global Config")]
    public class GlobalConfig : SingletonScriptableObject<GlobalConfig>
    {
        [Title("Fire Penguin")]
        [OdinSerialize]
        public float FireForceMin { get; private set; }
        [OdinSerialize]
        public float FireForceMax { get; private set; }

#if UNITY_EDITOR
        [MenuItem("Penguins/Global Config")]
        public static void ShowGlobalSettings()
        {
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = Instance;
        }
#endif
    }
}

