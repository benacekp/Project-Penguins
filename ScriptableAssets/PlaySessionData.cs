using _Scripts.Level;
using BlackFramework.ScriptableArchitecture;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.ScriptableAssets
{
    [TypeInfoBox("Asset, který uchovává data během herní sešny.")]
    [CreateAssetMenu(fileName = "NewPlaySessionData", menuName = "Penguins/New PlaySession Data")]
    public class PlaySessionData : SingletonScriptableObject<PlaySessionData>
    {
        // Todo_PB: Pro přechod z levelu si vytvořit kontejner, který bude přenášet data o dokončením levelu
        [ReadOnly]
        public bool LevelSucceed;    // uchovává informaci, že se hráč vrací z dokončeného levelu

        [ReadOnly]
        public LevelSettings Level; // prefab levelu pro vyinstancování prostředí levelu

        /// <summary>
        /// Resetuje data do výchozích hodnot.
        /// </summary>
        public void Reset()
        {
            Debug.Log("PlaySessionData.Reset()");
            LevelSucceed = false;
        }
        
    }
}
