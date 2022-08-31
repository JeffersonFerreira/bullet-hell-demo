using System;
using Entity;
using UnityEngine;

namespace Guns
{
	public class Bullet : MonoBehaviour
	{
		[SerializeField] private BulletHitEffect _hitEffect;

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
			switch (_hitEffect)
			{
				case BulletHitEffect.Damage:
				{
					if (collision.gameObject.TryGetComponent(out BaseHealthSystem healthSystem))
						healthSystem.TakeDamage();

					break;
				}
				case BulletHitEffect.Slowness:
				{
					if (collision.gameObject.TryGetComponent(out PlayerMovement movement))
						movement.ApplySlowness();

					break;
				}
				default:
					throw new ArgumentOutOfRangeException();
			}

			Dispose();
		}

		private void Dispose()
		{
			Destroy(gameObject);
		}
	}

	public enum BulletHitEffect
	{
		Damage,
		Slowness
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