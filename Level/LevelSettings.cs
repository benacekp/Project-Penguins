using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;

namespace _Scripts.Level
{
	/// <summary>
	/// Třída nesoucí data pro konkrétní level.
	/// </summary>
	public class LevelSettings : MonoBehaviour
	{
		[Required]
		public string LevelName;
		[Required] 
		public int LevelIndex;
		[Required]
		public Transform StartPoint;

		[Required] public Collider2D VirtualCameraConfiner;

		[SerializeField, FoldoutGroup("Spawn Areas")] 
		private Transform spawnInstanceParent;
		[SerializeField, FoldoutGroup("Spawn Areas")]
		private Area areaPrefab;

		[SerializeField, FoldoutGroup("Finish Areas")]
		private Transform finishAreaParent;

		[InlineButton("CreateSpawnArea", "Create Spawn Area"), FoldoutGroup("Spawn Areas")] [ValidateInput("IsAreasSetCorrect","Spawn areas nejsou správně nastavené!", InfoMessageType.Error)]
		public List<Area> SpawnAreas;

		[SerializeField, PreviewField(250, ObjectFieldAlignment.Center), HideLabel, ValidateInput("IsPreviewSet", "Preview Levelu není nastaveno. - Přetáhni do náhledu prefab tohoto levelu.", InfoMessageType.Warning)]
		private GameObject levelPreview; // slouží o vykreslení náhledu na level v inspektoru

		private void OnEnable()
		{
			// upozorním na případně špatně nastavené SpawnAreas
			if(!IsAreasSetCorrect(SpawnAreas)) Debug.LogError("Nejsou korektně nastavené Areas!");
		}
		
#if UNITY_EDITOR
		private void CreateSpawnArea()
		{
			Area newArea = Instantiate(areaPrefab, Vector3.zero, Quaternion.identity);
			newArea.gameObject.transform.SetParent(spawnInstanceParent);
			SpawnAreas.Add(newArea);
			Selection.activeObject = newArea.gameObject;
		}
#endif

		private bool IsAreasSetCorrect(List<Area> pAreas)
		{
			if (pAreas == null) return false;		// testuje, že je seznam existuje
			if (pAreas.Count == 0) return false;	// testuji, že je nastaven alespoň jeden spawn area
			
			foreach (Area area in pAreas)		// testuji, jestli nemám null prvek v seznamu spawn areas
			{
				if (area == null) return false;
			}

			return true;
		}

		private bool IsNameSetCorrect(string pName)
		{
			if (string.IsNullOrEmpty(pName)) return false;

			return true;
		}

		private bool IsPreviewSet(GameObject pPreview)
		{
			if (pPreview == null) return false;

			return true;
		}
	}
}
