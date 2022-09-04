using System;
using Entity;
using UnityEngine;

namespace Guns
{
	public class Bullet : MonoBehaviour
	{
		[SerializeField] private BulletHitEffect _hitEffect;

		public BulletData Data { get; private set; }

		private Rigidbody _rigidbody;

		private const int LAYER_HIT_ENEMY = 11;
		private const int LAYER_HIT_PLAYER = 10;

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody>();
		}

		public void Apply(BulletData data)
		{
			Data = data;
			_rigidbody.velocity = transform.forward * data.Speed;
			Invoke(nameof(Dispose), data.Timeout);

			gameObject.layer = data.Target == Target.Player
				? LAYER_HIT_PLAYER
				: LAYER_HIT_ENEMY;
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

	public enum Target
	{
		Enemy, Player
	}

	// For debugging
	[Serializable]
	public struct BulletData
	{
		public float Timeout;
		public float Speed;
		public Vector3 Origin;
		public Target Target;

		public BulletData(Target target, Vector3 origin, float speed, float timeout)
		{
			Timeout = timeout;
			Speed = speed;
			Origin = origin;
			Target = target;
		}
	}
}