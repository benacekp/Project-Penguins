using System;
using System.Collections.Generic;
using Plugins.BlackFramework.ScriptableArchitecture.Events;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Level
{
	public class SavePointController : MonoBehaviour
	{
		[SerializeField, TitleGroup("Finish:")] private bool isFinish;

		[SerializeField, TitleGroup("Areas:")]
		private Area areaPrefab;

		[SerializeField, InlineButton("CreateArea", "Create Area"), TitleGroup("Areas:")]
		private List<Area> SaveAreas;


		[SerializeField, ReadOnly, TitleGroup("Info")] private PenguinController currentPenguin;

#if UNITY_EDITOR
		private void CreateArea()
		{
			Area newArea = Instantiate(areaPrefab, Vector3.zero, Quaternion.identity);
			newArea.Radius = 0.33f;
			newArea.gameObject.transform.SetParent(transform);
			SaveAreas.Add(newArea);
			Selection.activeObject = newArea.gameObject;
		}
#endif

		// provede prijetí penguina na savepoint
		public void OnPenguinInSavePoint(PenguinController pPenguin)
		{
			if(pPenguin == currentPenguin) return;
			
			pPenguin.MovingStoped(isFinish
				? PenguinController.EPenguinState.Survived
				: PenguinController.EPenguinState.SavePoint);

			pPenguin.SetPhysics(false);
			MovePenguinOnSaveArea(pPenguin.gameObject.transform);
			currentPenguin = pPenguin;
		}

		private void MovePenguinOnSaveArea(Transform pPenguin)
		{
			// vypočítám pozici kam penguina postavit
			Area area = GetNearestArea(pPenguin.position, SaveAreas);
			Vector3 random = Random.insideUnitCircle*area.Radius;
			pPenguin.position = area.transform.position + random;
		}

		// najde vrací area, která je nejblíze k dané pozici
		private Area GetNearestArea(Vector3 pFrom, List<Area> pAreas)
		{
			Area result = pAreas[0];
			for (int i = 1; i < pAreas.Count-1; i++)
			{
				if (Vector3.Distance(pFrom, pAreas[i].transform.position) < Vector3.Distance(pFrom, result.transform.position))
					result = pAreas[i];
			}

			return result;
		}
		
/*
		private void OnCollisionEnter2D(Collision2D other)
		{
			Debug.Log("collision");
			PenguinController penguin = other.gameObject.GetComponent<PenguinController>();

			if (penguin != null && penguin != currentPenguin)
			{
				OnPenguinInSavePoint(penguin);
			}
			else
			{
				Debug.Log("trefilo mě něco jiného");
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			PenguinController penguin = other.gameObject.GetComponent<PenguinController>();

			if (penguin != null && penguin != currentPenguin)
			{
				OnPenguinInSavePoint(penguin);
			}
			else
			{
				Debug.Log("trefilo mě něco jiného");
			}
		}
		*/
	}
}

