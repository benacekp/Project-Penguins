using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.Gui.GuiItems.SpinWheel
{
	public class SpinWheerRotation : MonoBehaviour
	{
		[SerializeField] private UnityEvent OnSpinEnd;
		[SerializeField]private bool spinOn;
		private RectTransform trans;
		[SerializeField] private int fullSpinCount;

		[SerializeField]private float rotationsSpeed;
		[SerializeField] private AnimationCurve curve;

		private float rotateStep;
		[SerializeField, ReadOnly]
		private float spinDegreseCounter;
		private float soundCounter;
		private int lastPrizeIndex = 0;
		[SerializeField]private AudioSource audioSource;

		private void Awake()
		{
			trans = gameObject.GetComponent<RectTransform>();
		}

		private void Update()
		{
			if (spinOn)
			{

				if (spinDegreseCounter < 360)
				{
					// redukuji rychlost otáčení v čase
					rotateStep = rotationsSpeed*curve.Evaluate(spinDegreseCounter/360) * Time.deltaTime;
					
				}
				else
				{
					rotateStep = rotationsSpeed * Time.deltaTime;
				}
				
				if (rotateStep < 1f) rotateStep = 1f;
				
				spinDegreseCounter -= rotateStep;
				
				

				// korekce, abych se nedostával pod nulu
				if (spinDegreseCounter < 0)
					rotateStep += spinDegreseCounter;
				
				trans.RotateAround(trans.position, Vector3.forward, rotateStep );

				UpdateSound(rotateStep);
					
				if(spinDegreseCounter <= 0f) SpinEnd();
			}
		}

		private void UpdateSound(float pRotateStep)
		{
			soundCounter += pRotateStep;
			if (soundCounter > 45)
			{
				if(audioSource != null)
					audioSource.Play();
				soundCounter = 0f;
			}
		}

		public void Spin(int pPrizeIndex)
		{
			spinDegreseCounter = (360 * fullSpinCount)+ ((8-lastPrizeIndex)*45) + (45*pPrizeIndex); // plné spiny + korekce do 0 + otočení do prize
			lastPrizeIndex = pPrizeIndex;
			spinOn = true;
		}

		private void SpinEnd()
		{
			spinOn = false;
			OnSpinEnd.Invoke();
		}
	}
}
