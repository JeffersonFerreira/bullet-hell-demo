using UnityEngine;

namespace Utility
{
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _instance;

		public static T Instance => _instance ??= Object.FindObjectOfType<T>();
	}
}