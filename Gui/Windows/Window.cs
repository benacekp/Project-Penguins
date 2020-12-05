using System.Collections;
using BlackFramework.MenuSystem;
using Plugins.BlackFramework.MenuSystem;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;

namespace _Scripts.Gui.Windows
{
	public class Window : SimpleMenu<Window>
	{
		[SerializeField] private UnityEvent OnClose;



		public override void OnBackPressed()
		{
			OnClose.Invoke();
		}

		public void CloseWithDelay(float pSecunds)
		{
			StartCoroutine(CloseAfter(pSecunds));
		}

		private IEnumerator CloseAfter(float pDelay)
		{
			yield return new WaitForSeconds(pDelay);
			MenuManager.Instance.CloseMenu(this);
		}
	}
}
