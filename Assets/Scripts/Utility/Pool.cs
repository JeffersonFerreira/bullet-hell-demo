using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
	public class Pool<T> : IDisposable where T : MonoBehaviour
	{
		private readonly T _prefab;
		private readonly Transform _poolRoot;
		private readonly Queue<T> _itemsQueue = new Queue<T>();

		public Pool(T prefab, int initialSize = 10)
		{
			_prefab = prefab;

			_poolRoot = new GameObject($"Pool of {prefab.name}").transform;
			_poolRoot.gameObject.SetActive(false);

			Spawn(initialSize);
		}

		private void Spawn(int count)
		{
			for (var i = 0; i < count; i++)
			{
				T item = GameObject.Instantiate(_prefab, Vector3.zero, Quaternion.identity, _poolRoot);

				Store(item);
			}
		}

		public T Retrieve()
		{
			if (_itemsQueue.Count == 0)
				Spawn(10);

			return _itemsQueue.Dequeue();
		}

		public void Store(T item)
		{
			_itemsQueue.Enqueue(item);

			item.transform.SetParent(_poolRoot);
			item.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
		}

		public void Dispose()
		{
			foreach (T item in _itemsQueue)
				GameObject.Destroy(item.gameObject);
			_itemsQueue.Clear();

			GameObject.Destroy(_poolRoot.gameObject);
		}
	}
}