using System.Collections.Generic;
using System.Diagnostics;
using _Scripts.ScriptableAssets;
using _Scripts.ScriptableAssets.Events;
using Lean.Touch;
using Plugins.BlackFramework.ScriptableArchitecture.Events;
using Plugins.BlackFramework.ScriptableArchitecture.Variables;
using RoboRyanTron.Unite2017.Variables;
using Sirenix.OdinInspector;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace _Scripts.Level
{
    // Todo_PB: zamyslet se nad rozdělením fire_controller a fire_manager - URCITE ROZDELIT - UZ TED JE TO NEPREHLEDNE
    
    public class FireController : MonoBehaviour
    {

        #region Penguins Management variables

        [SerializeField, Required, FoldoutGroup("Settings")]
        private PenguinController penguinPrefab;

        [SerializeField, Required, FoldoutGroup("Settings")]
        private EventPenguin OnActivePenguinChangedEvent, OnNewPenguinOnStart;
        [SerializeField, Required, FoldoutGroup("Settings")]    
        private GameEvent OnAllPenguinsFired;
        
        [SerializeField, ReadOnly, FoldoutGroup("Info")]
        private PenguinController activePenguin;

        [SerializeField, ReadOnly, FoldoutGroup("Info")]
        private List<PenguinController> penguinsFront = new List<PenguinController>();
        
        [ ReadOnly, FoldoutGroup("Info")]
        public List<PenguinController> penguinsInLevel = new List<PenguinController>();

        private List<Area> spawnAreas;

        #endregion

        #region Fire variables

        [SerializeField, ReadOnly, FoldoutGroup("Info Fire")] 
        private Vector2 touchStartPos; // počáteční bod položení prstu pro vektor výstřelu
        [SerializeField,ReadOnly, FoldoutGroup("Info Fire")]
        private Vector2 touchCurrentPos; // aktuální pozice prstu při provádění střelby

        [SerializeField, Required, FoldoutGroup("Info Fire")]
        private FireIndicator fireIndicator;

        
        [SerializeField, Required, FoldoutGroup("Info Fire")]
        private FloatReference firePower;

        [SerializeField, FoldoutGroup("Info Fire")]
        private float fireTouchLenghtMin;

        [SerializeField, FoldoutGroup("Info Fire")]
        private float fireTouchLenghtMax;


        private Vector3 fireVector;
        private Transform startPoint;

        #endregion

        public List<PenguinController> Init(List<PenguinData> pPenguins, List<Area> pSpawnAreas, Transform pStartPoint)
        {
            spawnAreas = pSpawnAreas;
            startPoint = pStartPoint;
            SpawnPenguins(pPenguins);
            return penguinsInLevel;
        }

        #region Spawnování, aktivace a deaktivace penguinů

        private void SpawnPenguins(List<PenguinData> pPenguins)
        {
            foreach (PenguinData data in pPenguins)
            {
                // instancuji penguina dle požadovaných dat
                PenguinController newPenguin = Instantiate(penguinPrefab);
                newPenguin.Init(data);
                penguinsInLevel.Add(newPenguin);
                // deaktivuji penguina a postavím ho do fronty před startem
                DeactivatePenguin(newPenguin);
            }
            // aktivuji prvního penguina
            SetNextPenguin();
        }

        /// <summary>
        /// Nastaví daného penguina jako aktivního - postaví jej na start pro vysřelení
        /// </summary>
        /// <param name="pPenguin"></param>
        public void SetPenguin(PenguinController pPenguin)
        {
            // pokud nedostávám korektní parametr, nic nedělám
            if (pPenguin == null)
            {
                Debug.LogError("FireController.SetActivePenguin() - nedostávám korektně penguina k aktivaci. Nic nenastavuji.");
                return;
            }
            
            // pokud je už nastaven nějaký aktivní penguin, musím jej deaktivovat - odložit do frony před start
            if(activePenguin != null)
            {
                DeactivatePenguin(activePenguin, pPenguin.transform.position);
            }
            
            // aktivovat penguina
            ActivatePenguin(pPenguin);
        }

        public void SetNextPenguin()
        {
            if(penguinsFront.Count>0)
            {
                PenguinController nextPenguin = penguinsFront[0];
                penguinsFront.Remove(nextPenguin);
                SetPenguin(nextPenguin);
            }
            else
            {
                OnAllPenguinsFired.Raise();
            }
        }
        
        private void ActivatePenguin(PenguinController pPenguin)
        {
            pPenguin.transform.position = startPoint.position;
            pPenguin.SetPhysics(true);
            pPenguin.SetPenguinInPlaying(true);
            penguinsFront.Remove(activePenguin);
            OnNewPenguinOnStart.Penguin = pPenguin;
            OnNewPenguinOnStart.Raise();
        }

        // deaktivuje penguina a postaví jej na nějaké místo u startu (spawn areas)
        private void DeactivatePenguin(PenguinController pPenguin)
        {
            // vypočítám pozici kam penguina postavit
            Vector3 position;
            Area area = spawnAreas[Random.Range(0, spawnAreas.Count)];
            Vector3 random = Random.insideUnitCircle*area.Radius;
            position = area.transform.position + random;
            
            // deaktivuji daného penguina a stavím ho na požadovanou pozici
            DeactivatePenguin(pPenguin, position);
        }
        
        // deaktivuje penguina a postaví jen na konkrétní místo
        private void DeactivatePenguin(PenguinController pPenguin, Vector3 pPosition)
        {
            pPenguin.SetPhysics(false);
            pPenguin.SetPenguinInPlaying(false);
            pPenguin.transform.position = pPosition;
            penguinsFront.Add(pPenguin);
        }

        #endregion

        #region Events

        public void OnPenguinSelected(EventPenguin pEvent)
        {
            activePenguin = pEvent.Penguin;
            FireStart();

            OnActivePenguinChangedEvent.Penguin = activePenguin;
            OnActivePenguinChangedEvent.Raise();
        }

        public void OnPenguinDeselected(EventPenguin pEvent)
        {
            activePenguin = null;
            
            OnActivePenguinChangedEvent.Penguin = activePenguin;
            OnActivePenguinChangedEvent.Raise();
        }

        public void OnPenguinStop(EventPenguin pEvent)
        {
            if (pEvent.Penguin.state != PenguinController.EPenguinState.SavePoint)
            {
                activePenguin = null;
                SetNextPenguin();
            }
        }

        #endregion

        #region Touch Fire Implementation

        private void FireStart()
        {
            if(activePenguin == null) return;

            activePenguin.SetPhysics(true);
            fireIndicator.Activate(activePenguin.transform.position);
            touchStartPos = Camera.main.WorldToScreenPoint(activePenguin.gameObject.transform.position);
        }

        private void OnTouchUp(LeanFinger finger)
        {
            if (activePenguin != null && firePower.Value>0)
            {
                activePenguin.Fire(fireVector.normalized, firePower.Value);
            }

            fireIndicator.Deactivate();
        }

        private void OnTouchGesture(List<LeanFinger> fingers)
        {
            if (activePenguin != null)
            {
                touchCurrentPos = fingers[0].LastScreenPosition;
                fireVector = -(touchCurrentPos - touchStartPos);
                SetFirePower(fireVector.magnitude);
                UpdateFireIndicator(firePower.Value);
            }
        }

        // stará se o vizualní zobrazování síly výstřelu
        private void UpdateFireIndicator(float pFirePower)
        {
            fireIndicator.SetSize(pFirePower);
            fireIndicator.SetDirection(fireVector);
        }

        // dle délky touch gesta nastaví sílu výstřelu
        private void SetFirePower(float pVectorLenght)
        {
            if (pVectorLenght > fireTouchLenghtMax)
            {
                firePower.Value = 1f;
                return;
            }

            if (pVectorLenght < fireTouchLenghtMin)
            {
                firePower.Value = 0f;
                return;
            }

            float a = fireTouchLenghtMax - fireTouchLenghtMin;
            float c = pVectorLenght - fireTouchLenghtMin;

            firePower.Value = ((c / a));
        }

        #endregion

        #region Unity Methods
        private void OnEnable()
        {
            Lean.Touch.LeanTouch.OnFingerUp += OnTouchUp;
            Lean.Touch.LeanTouch.OnGesture += OnTouchGesture;
        }

        private void OnDisable()
        {
            Lean.Touch.LeanTouch.OnFingerUp -= OnTouchUp;
            Lean.Touch.LeanTouch.OnGesture -= OnTouchGesture;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {

            if (activePenguin != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(activePenguin.transform.position, activePenguin.transform.position + fireVector.normalized*2);
            }
        }
#endif

        #endregion
    }
}
