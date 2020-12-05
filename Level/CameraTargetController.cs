using System;
using System.Collections.Generic;
using _Scripts.ScriptableAssets.Events;
using Cinemachine;
using Lean.Touch;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.Level
{
    [TypeInfoBox("Objekt, který je sledován kamerou a stará se o ovládání kamery.")]
    public class CameraTargetController : MonoBehaviour
    {
        [SerializeField, ReadOnly] private bool locked;
        
        
        [SerializeField, Required, TitleGroup("Zoom")] 
        private CinemachineVirtualCamera virCamera;

        [SerializeField, Required, TitleGroup("Zoom"), MinMaxSlider(0f, 10f)]
        private Vector2 zoomRange;

        [SerializeField, Required, TitleGroup("Zoom"), Range(0.1f, 4f)]
        private float zoomToTargetSpeed;
        

        [TitleGroup("Fallow")]
        [SerializeField, Required]private float fallowSpeed;
        [SerializeField, ReadOnly, TitleGroup("Fallow")] private Transform fallowTarget;
        private Vector3 fallowVector;
        
        [Title("Touch")]
        public LeanFingerFilter Use = new LeanFingerFilter(true);

        // touch move
        private List<LeanFinger> fingers = new List<LeanFinger>();
        private Vector2 moveVector;
        private float moveSpeed = 1f;
        private Vector3 screenPoint;
        private Vector3 position;

        private void Update()
        {
            if(!locked)
                UpdateGesture();

            if (fallowTarget != null)
            {
                FallowTarget();
                ZoomToTarget();
            }
        }

        public void SetLock(bool pValue)
        {
            locked = pValue;
        }

        public void SetFallower(EventPenguin pEvent)
        {
            Debug.Log("Set Fallower");
            fallowTarget = pEvent.Penguin.transform;
        }

        public void ClearFallower()
        {
            fallowTarget = null;
        }

        private void UpdateGesture()
        {
            fingers = Use.GetFingers(); 
            
            // move camera
            
            moveVector = -LeanGesture.GetScreenDelta(fingers);
            if (moveVector != Vector2.zero) TouchMove(moveVector);
            
            // pitch zoom
            //UpdateZoom(LeanGesture.GetPinchScale(fingers));
        }

        private void UpdateZoom(float pValue)
        {
            virCamera.m_Lens.OrthographicSize = Mathf.Clamp(virCamera.m_Lens.OrthographicSize /= pValue, zoomRange.x, zoomRange.y);
        }

        private void ZoomToTarget()
        {
            if (virCamera.m_Lens.OrthographicSize > zoomRange.x)
            {
                virCamera.m_Lens.OrthographicSize -= zoomToTargetSpeed * Time.deltaTime;
            }
        }

        private void TouchMove(Vector2 pMoveVector)
        {
            pMoveVector *= moveSpeed;
            
            // Screen position of the transform
            screenPoint = Camera.main.WorldToScreenPoint(transform.position);

            // Add the deltaPosition
            screenPoint += (Vector3)pMoveVector;

            // Convert back to world space
            position = Camera.main.ScreenToWorldPoint(screenPoint);
            transform.position = position;
        }

        private void FallowTarget()
        {
            fallowVector = fallowTarget.position - transform.position;
            transform.Translate(fallowVector * Time.deltaTime * fallowSpeed);
        }
    }
}
