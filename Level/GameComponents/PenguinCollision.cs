using System;
using _Scripts.ScriptableAssets.Events;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.Level.GameComponents
{
    [AddComponentMenu("Penguins/Level/Penguin Collision")]
    public class PenguinCollision : MonoBehaviour
    {
        [SerializeField]
        private PenguinEvent OnTriggerEnter, OnTriggerStay, OnTriggerExit, OnCollisionEnter, OnCollisionStay, OnCollisionExit;

        private PenguinController currentPenguin;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (IsPenguin(other))
            {
                OnTriggerEnter.Invoke(currentPenguin);
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (IsPenguin(other))
            {
                OnTriggerStay.Invoke(currentPenguin);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (IsPenguin(other))
            {
                OnTriggerExit.Invoke(currentPenguin);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (IsPenguin(other.collider))
            {
                OnCollisionEnter.Invoke(currentPenguin);   
            }
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (IsPenguin(other.collider))
            {
                OnCollisionStay.Invoke(currentPenguin);
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (IsPenguin(other.collider))
            {
                OnCollisionExit.Invoke(currentPenguin);
            }
        }

        private bool IsPenguin(Collider2D pCollider)
        {
            currentPenguin = pCollider.gameObject.GetComponent<PenguinController>();
            return currentPenguin != null;
        }
    }
}
