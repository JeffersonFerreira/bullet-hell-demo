using System.Collections;
using UnityEngine;

namespace Utility
{
	public interface ICoroutineHelper
	{
		void StopCoroutine(Coroutine coroutine);
		Coroutine StartCoroutine(IEnumerator enumerator);
	}

	public class CoroutineHelper : MonoBehaviour, ICoroutineHelper
	{
		private static CoroutineHelper _instance;

		public static ICoroutineHelper Instance
		{
			get
			{
				if (_instance == null) 
					_instance = new GameObject("Coroutine Helper").AddComponent<CoroutineHelper>();

				return _instance;
			}
		}
	}
}