using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;

namespace Guns
{
	public class BulletPool : Singleton<BulletPool>
	{
		[SerializeField] private Bullet[] _prefabList = Array.Empty<Bullet>();

		private Dictionary<BulletType, Pool<Bullet>> _poolDict;

		// private void Awake()
		// {
		// 	_poolDict = _prefabList.ToDictionary(b => b.Type, b => new Pool<Bullet>(b));
		// }
		//
		// public Bullet Retrieve(BulletType bulletType)
		// {
		// 	if (_poolDict.TryGetValue(bulletType, out var pool))
		// 		return pool.Retrieve();
		//
		// 	throw new NotImplementedException($"Bullet pool of type \"{bulletType}\" has not setup");
		// }
		//
		// public void Store(Bullet bullet)
		// {
		// 	if (_poolDict.TryGetValue(bullet.Type, out var pool))
		// 		pool.Store(bullet);
		// 	else
		// 		throw new NotImplementedException($"Bullet pool of type \"{bullet.Type}\" has not setup");
		// }
	}
}