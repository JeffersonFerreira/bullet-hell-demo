using System;
using Entity;
using UnityEngine;

namespace Guns
{
	public class Bullet : MonoBehaviour
	{
		[SerializeField] private BulletHitEffect _hitEffect;
		[SerializeField] private float _onHitDestroyDelay = 0.2f;

		public BulletData Data { get; private set; }

		private Rigidbody _rigidbody;
		private TrailRenderer _trailRenderer;

		private const int LAYER_HIT_ENEMY = 11;
		private const int LAYER_HIT_PLAYER = 10;

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody>();
			_trailRenderer = GetComponentInChildren<TrailRenderer>();
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
			enabled = false;

			if (_onHitDestroyDelay <= 0)
				Dispose();
			else
				Invoke(nameof(Dispose), _onHitDestroyDelay);

			switch (_hitEffect)
			{
				case BulletHitEffect.Damage:
				{
					if (collision.gameObject.TryGetComponent(out BaseHealthSystem healthSystem))
					{
						healthSystem.TakeDamage();
						_trailRenderer.enabled = false;
					}

					break;
				}
				case BulletHitEffect.Slowness:
				{
					if (collision.gameObject.TryGetComponent(out PlayerMovement movement))
					{
						movement.ApplySlowness();
						Dispose();
					}

					break;
				}
				default:
					throw new ArgumentOutOfRangeException();
			}
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
		Enemy,
		Player
	}

	public readonly struct BulletData
	{
		public float Timeout { get; }
		public float Speed { get; }
		public Vector3 Origin { get; }
		public Target Target { get; }

		public BulletData(Target target, Vector3 origin, float speed, float timeout)
		{
			Timeout = timeout;
			Speed = speed;
			Origin = origin;
			Target = target;
		}
	}
}