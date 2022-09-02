using System.Collections;
using UnityEngine;

namespace Utility
{
	public interface ICoroutineHelper
	{
		void StopCoroutine(Coroutine coroutine);
		Coroutine StartCoroutine(IEnumerator routine);
	}

	public class CoroutineHelper : ICoroutineHelper
	{
		private readonly MonoBehaviour _wrapper;

		public CoroutineHelper(MonoBehaviour wrapper)
		{
			_wrapper = wrapper;
		}


		public Coroutine StartCoroutine(IEnumerator routine)
		{
			return _wrapper.StartCoroutine(routine);
		}

		public void StopCoroutine(Coroutine coroutine)
		{
			_wrapper.StopCoroutine(coroutine);
		}
	}
}