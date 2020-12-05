using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Analitics;
using _Scripts.Gui.Munus;
using _Scripts.ScriptableAssets;
using _Scripts.ScriptableAssets.Events;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace _Scripts.Level
{
    public class LevelController : MonoBehaviour
    {
        public enum LevelStateType {InProgress, Won, Lost, Quit}

        #region Settings variables

        [SerializeField, Required] [FoldoutGroup("Settings")]
        private FireController fireController;

        [SerializeField, Required] [FoldoutGroup("Settings")]
        private LevelMenuManager menuManager;

        [SerializeField, Required] [FoldoutGroup("Settings")]
        private CinemachineVirtualCamera virtualCamera;

        [SerializeField, ReadOnly, FoldoutGroup("Info")]
        private List<PenguinController> PenguinsInLevel;
        
        #endregion

        #region Info variables

        [ShowInInspector] [ReadOnly] [InlineEditor()] [FoldoutGroup("Info")]
        private LevelSettings levelSettings;

        private LevelStateType state;
        private float secundsInLevel = 0f;
        
        #endregion

        #region Events

        [SerializeField, TitleGroup("Events")]
        private UnityEvent OnStarting, OnStartFinished;

        #endregion

        private void Start()
        {
            if (TryInitLevel()) // jen pokud jsme úspěšně načetli data levelu, můžeme provádět další věci
            {
                SetVirtualCameraConfiner();
                PenguinsInLevel = fireController.Init(PlayerData.Instance.PenguinsManager.Penguins, levelSettings.SpawnAreas, levelSettings.StartPoint);
                InitGui();
                state = LevelStateType.InProgress;
                AnalyticsController.SendLevelStart(levelSettings.LevelName);
                OnStarting.Invoke();
                OnStartFinished.Invoke();
            }
            else
            {
                Debug.LogError("Nemám level, vracím se do Mapy!");
                SceneManager.LoadScene("Map");
            }
        }

        private void Update()
        {
            secundsInLevel += Time.deltaTime;
        }

        private void SetVirtualCameraConfiner()
        {
            CinemachineConfiner cinemachineConfiner = virtualCamera.gameObject.GetComponent<CinemachineConfiner>();
            if (cinemachineConfiner != null)
            {
                cinemachineConfiner.m_BoundingShape2D = levelSettings.VirtualCameraConfiner;
                cinemachineConfiner.InvalidatePathCache();
            }
        }

        private bool TryInitLevel()
        {
            #region try use debug level

#if UNITY_EDITOR
            if (DevelopmentConfig.Instance.UseDebugLevel)
            {
                if (TryInitDebugLevel())
                {
                    Debug.Log("Poustim testovací level");
                    return true;
                }

                // pokud mám požít debug level a nemám ho, vracím se do Mapy
                Debug.LogError("DebugLevel není nastaven!");
                return false;
            }
#endif

            #endregion

            #region try use PlaySesstionData level
            

            if (PlaySessionData.Instance.Level != null)
            {
                levelSettings = PlaySessionData.Instance.Level;
                Instantiate(levelSettings);
                Debug.Log("Nacitam level z PlaySessionData");
                return true;
            }
            
            return false;
            
            #endregion
        }

        private void InitGui()
        {
            menuManager.Init(levelSettings);
        }

        public void AllPenguinsFired()
        {
            LevelFinished();
        }

        private void LevelFinished()
        {
            state = AnyPenguinSurvived() ? LevelStateType.Won : LevelStateType.Lost;
            
            PlaySessionData.Instance.LevelSucceed = state == LevelStateType.Won;
      
            LevelFinishedMenu.Show(new LevelFinishData(levelSettings.LevelName, levelSettings.LevelIndex, state, PenguinsInLevel)); 
        }
        

#if UNITY_EDITOR


        private bool TryInitDebugLevel()
        {
            LevelSettings debugLevel = DevelopmentConfig.Instance.DebugLevel;
            if (debugLevel != null)
            {
                levelSettings = debugLevel;
                Instantiate(debugLevel);
                return true;
            }

            return false;
        }

#endif
        

        private bool AnyPenguinSurvived()
        {
            foreach (var penguin in PenguinsInLevel)
            {
                if (penguin.state == PenguinController.EPenguinState.Survived) return true;
            }

            return false;
        }


        private void OnDestroy()
        {
            if (state == LevelStateType.InProgress || state == LevelStateType.Quit)
            {
                AnalyticsController.SendLevelQuit(levelSettings.LevelName, secundsInLevel, GetPenguinsDataByState(PenguinsInLevel, PenguinController.EPenguinState.Lost).Count );
            }
        }
        
        public static List<PenguinData> GetPenguinsDataByState( List<PenguinController> pPenguinsInLevel, PenguinController.EPenguinState pState)
        {
            List<PenguinData> result = new List<PenguinData>();
            foreach (var penguin in pPenguinsInLevel)
            {
                if(penguin.state == pState)
                    result.Add(penguin.Data);
            }
         
            return result;
        }
    }

    public struct LevelFinishData
    {
        public string LevelName;
        public int LevelIndex;
        public LevelController.LevelStateType LevelState;
        public List<PenguinData> PenguinsSurvivals;
        public List<PenguinData> PenguinsLost;
        public float CoinsFoundPercent;
        public float HeartsFoundPercent;
        public int HeartsUsed;
        public float SecundsInLevel;

        public LevelFinishData(string levelName, int levelIndex, LevelController.LevelStateType state, List<PenguinController> penguins )
        {
            LevelName = levelName;
            LevelIndex = levelIndex;
            LevelState = state;
            PenguinsSurvivals = LevelController.GetPenguinsDataByState(penguins, PenguinController.EPenguinState.Survived);
            PenguinsLost = LevelController.GetPenguinsDataByState(penguins, PenguinController.EPenguinState.Lost);
            CoinsFoundPercent = 0;
            HeartsFoundPercent = 0;
            HeartsUsed = 0;
            SecundsInLevel = 0;
        }
        

    }
}
