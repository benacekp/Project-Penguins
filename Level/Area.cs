using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;

namespace _Scripts.Level
{
    /// <summary>
    /// Definuje oblast v levelu, ve které je možné vyinstancovat tučňáky na startu levelu.
    /// </summary>
    public class Area : MonoBehaviour
    {

        [Range(0.2f, 2f)]
        public float Radius;




#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.color = Color.magenta;
            Handles.DrawWireDisc(transform.position, -Vector3.forward, Radius );
        }
#endif
    }
}
