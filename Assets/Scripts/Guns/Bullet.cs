using System;
using Entity;
using UnityEngine;

namespace Guns
{
	public class Bullet : MonoBehaviour
	{
		private Rigidbody _rigidbody;
		private BulletData _data;

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody>();
		}

		public void Apply(BulletData data)
		{
			_data = data;
			_rigidbody.velocity = transform.forward * data.Speed;

			Invoke(nameof(Dispose), data.Timeout);
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (collision.gameObject.TryGetComponent(out BaseHealthSystem healthSystem))
				healthSystem.TakeDamage();

			Dispose();
		}

		private void Dispose()
		{
			Destroy(gameObject);
		}
	}

	// For debugging
	[Serializable]
	public struct BulletData
	{
		public int SourceInstanceID;

		[Space]
		public float Timeout;
		public float Speed;
	}
}