using _Scripts.Level;
using BlackFramework.ScriptableArchitecture;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.ScriptableAssets
{
    [TypeInfoBox("Asset, který nastavuje, jak se má hra chovat testování v editoru.")]
    [CreateAssetMenu(fileName = "DevelopmentConfig", menuName = "Penguins/New Development Config")]
    public class DevelopmentConfig : SingletonScriptableObject<DevelopmentConfig>
    {
        [Title("Debug Level")]
        public bool UseDebugLevel;

        [ShowIf("UseDebugLevel"), AssetSelector(Paths = "Assets/_Prefabs/Levels"), ValidateInput("IsTestingLevelSetCorrect","Testing level není nastaven!", InfoMessageType.Error)]
        public LevelSettings DebugLevel;
        
        private bool IsTestingLevelSetCorrect(LevelSettings pTestingLevel)
        {
            if (pTestingLevel == null) return false;

            return true;
        }
    }
}
