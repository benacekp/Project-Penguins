using System;
using System.Collections.Generic;
using _Scripts.ScriptableAssets;
using _Scripts.ScriptableAssets.Events;
using RoboRyanTron.Unite2017.Sets;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace _Scripts.Level
{
    public class PenguinController : MonoBehaviour
    {
        public enum EPenguinState { Survived, SavePoint, Lost, UnUsed }

        [EnumToggleButtons, ReadOnly]
        public EPenguinState state = EPenguinState.UnUsed;
        
        [SerializeField, Required, FoldoutGroup("Events")]
        private EventPenguin OnPenguinSelected, OnPenguinStop, OnPenguinDeselected, OnPenguinFired;


        [ShowInInspector][ReadOnly]
        public PenguinData Data;
        private Rigidbody2D rb;
        [SerializeField, ReadOnly]
        private List<Collider2D> colliders; // collidery, které chci vypínat pri deaktivaci fyziky

        [SerializeField]
        private bool checkingStopMoving, playing;

        private void Awake()
        {
            state = EPenguinState.UnUsed;
            rb = gameObject.GetComponent<Rigidbody2D>();
            colliders = GetColliders();
        }

        private void FixedUpdate()
        {
            if(checkingStopMoving)
            {
                if (rb.velocity.magnitude < 0.1f)
                {
                    MovingStoped(EPenguinState.Lost);
                }
            }
        }
        
        public void Init(PenguinData pData)
        {
            Data = pData;
            gameObject.name = "Penguin - " + Data.Name;
            //GetComponent<SpriteRenderer>().color = Data.Color;
        }

        public void Fire(Vector3 direction, float pPower)
        {
            rb.AddForce(direction * (GlobalConfig.Instance.FireForceMax*pPower), ForceMode2D.Impulse);
            checkingStopMoving = true;

            OnPenguinFired.Penguin = this;
            OnPenguinFired.Raise();
        }
        
        public void MovingStoped(EPenguinState pState)
        {
            checkingStopMoving = false;
            state = pState;
            
            if(state == EPenguinState.Survived)
                SetPenguinInPlaying(false);

            // vyvolám event
            OnPenguinStop.Penguin = this;
            OnPenguinStop.Raise();
        }
        
        public void SetPhysics(bool pValue)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = !pValue;
            foreach (Collider2D coll in colliders)
            {
                coll.enabled = pValue;
            }
        }

        // nastavuje, jestli je penguin ve hře aktivní, jestli se s ním může hrát
        public void SetPenguinInPlaying(bool pValue)
        {
            playing = pValue;
        }

        /// <summary>
        /// Methoda, která je vyvolána, když je penguin vybrán (Lean Selection)
        /// </summary>
        public void OnSelect()
        {
            if(!playing) return;

            if (OnPenguinSelected != null)
            {
                OnPenguinSelected.Penguin = this;
                OnPenguinSelected.Raise();
            }
        }

        public void OnDeselect()
        {
            if(!playing) return;

            if (OnPenguinDeselected != null)
            {
                OnPenguinDeselected.Penguin = this;
                OnPenguinDeselected.Raise();
            }
        }

        private List<Collider2D> GetColliders()
        {
            List<Collider2D> result = new List<Collider2D>();
            Collider2D[] allColliders = GetComponentsInChildren<Collider2D>();
            foreach (Collider2D coll in allColliders)
            {
                if(coll.gameObject.layer != 10) result.Add(coll);
            }

            return result;
        }
    }
}
