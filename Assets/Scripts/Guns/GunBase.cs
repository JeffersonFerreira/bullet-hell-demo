using System.Collections;
using UnityEngine;

namespace Guns
{
	public class GunBase : MonoBehaviour
	{
		[SerializeField] protected Bullet _bulletPrefab;

		[Header("Fire Settings")]
		[SerializeField] protected Transform _spawnPoint;
		[SerializeField] protected float _fireRate;

		[SerializeField] private float _fireCooldown = -1;

		public bool IsCoolingDown { get; private set; }
		public bool IsFiring { get; private set; }

		private void OnDisable()
		{
			IsFiring = false;
		}

		public bool Fire()
		{
			if (!CanFire())
				return false;

			StartCoroutine(FireRoutine());
			return true;
		}

		public bool CanFire()
		{
			return !IsFiring && !IsCoolingDown;
		}

		private IEnumerator FireRoutine()
		{
			IsFiring = true;

			FireOnce();

			if (_fireRate > 0)
				yield return new WaitForSeconds(1f / _fireRate);

			if (_fireCooldown > 0)
			{
				IsCoolingDown = true;
				yield return new WaitForSeconds(_fireCooldown);
				IsCoolingDown = false;
			}

			IsFiring = false;
		}

		protected virtual void FireOnce()
		{
			Bullet bullet = Instantiate(_bulletPrefab, _spawnPoint.position, transform.rotation);

			bullet.Apply(new BulletData {
				Speed = 20,
				Timeout = 10,
				Origin = _spawnPoint.position,
				SourceInstanceID = gameObject.GetInstanceID()
			});
		}
	}
}