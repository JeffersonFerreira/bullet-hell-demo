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

		[Min(1)]
		[SerializeField] private int _roundsPerShot = 1;
		[Range(0, 1)]
		[SerializeField] private float _roundsDelay = 0.2f;

		[SerializeField] private float _fireCooldown = -1;

		public bool IsCoolingDown { get; private set; }
		public bool IsFiring { get; private set; }

		private void OnDisable()
		{
			IsFiring = false;
		}

		public bool Fire(Target target)
		{
			if (!CanFire())
				return false;

			StartCoroutine(FireRoutine(target));
			return true;
		}

		public bool CanFire()
		{
			return !IsFiring && !IsCoolingDown;
		}

		private IEnumerator FireRoutine(Target target)
		{
			IsFiring = true;

			for (var i = 0; i < _roundsPerShot; i++)
			{
				FireOnce(target);

				if (_roundsPerShot > 1)
					yield return new WaitForSeconds(_roundsDelay);
			}

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

		protected virtual void FireOnce(Target target)
		{
			Vector3 origin = _spawnPoint.position;
			Bullet bullet = Instantiate(_bulletPrefab, origin, transform.rotation);
			bullet.Apply(new BulletData(target, origin, 20, 10));
		}
	}
}