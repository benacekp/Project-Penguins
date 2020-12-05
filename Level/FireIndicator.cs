using System;
using UnityEngine;
#pragma warning disable 108,114

namespace _Scripts.Level
{
    public class FireIndicator : MonoBehaviour
    {
        [SerializeField]
        private float maxHeight;
        private SpriteRenderer renderer;
        
        private void Awake()
        {
            renderer = gameObject.GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            Deactivate();
        }

        /// <summary>
        /// Aktivuje indikátor na danou pozici.
        /// </summary>
        /// <param name="pValue"></param>
        public void Activate(Vector3 pPosition)
        {
            gameObject.transform.position = pPosition;
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Nastaví délku identivikátoru.
        /// </summary>
        /// <param name="pValue">(%) 0->1</param>
        public void SetSize(float pValue)
        {
            float sizeX = renderer.size.x;
            renderer.size = new Vector2(sizeX, maxHeight*pValue);
        }

        public void SetDirection(Vector2 pDir)
        {
            float uhel = -Vector2.Angle(Vector2.up, pDir);
            if (pDir.x < 0) uhel = -uhel;
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, uhel);

        }
        
    }
}
