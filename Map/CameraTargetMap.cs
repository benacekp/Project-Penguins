using System;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

namespace _Scripts.Map
{
    public class CameraTargetMap : MonoBehaviour
    {
        [SerializeField]private Vector2 YRange;
        [SerializeField]private float moveSpeed;

        private LeanFingerFilter Use = new LeanFingerFilter(true);
        private List<LeanFinger> fingers = new List<LeanFinger>();
        private Vector2 moveVector;
        private Vector3 position;
        private Vector3 screenPoint;

        private void Update()
        {
            UpdateGesture();
        }

        private void UpdateGesture()
        {
            fingers = Use.GetFingers();

            moveVector = -LeanGesture.GetScreenDelta(fingers);
            if (moveVector != Vector2.zero)
                TouchMove(moveVector);
        }

        private void TouchMove(Vector2 pMoveVector)
        {
            pMoveVector.Set(0f, pMoveVector.y);
            pMoveVector *= moveSpeed;
            
            // Screen position of the transform
            screenPoint = Camera.main.WorldToScreenPoint(transform.position);

            // Add the deltaPosition
            screenPoint += (Vector3)pMoveVector;

            // Convert back to world space
            position = Camera.main.ScreenToWorldPoint(screenPoint);
            if(position.y < YRange.x)
                position.Set(0f,YRange.x, 0f);
            transform.position = position;
        }
    }
}
