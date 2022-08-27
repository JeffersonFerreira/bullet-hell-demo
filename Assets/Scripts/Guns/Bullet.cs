using System;
using UnityEngine;

namespace Guns
{
	public class Bullet : MonoBehaviour
	{
		private Rigidbody _rigidbody;

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody>();
		}

		public void Apply(BulletData data)
		{
			Vector3 dir = transform.forward;
			_rigidbody.velocity = dir * data.Speed;

			Destroy(gameObject, data.Timeout);
		}
	}

	// For debugging
	[Serializable]
	public struct BulletData
	{
		public int SourceInstanceID;

		[Space]
		public float Damage;
		public float Timeout;
		public float Speed;
	}
}