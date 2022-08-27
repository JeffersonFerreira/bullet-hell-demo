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

		public bool IsFiring { get; private set; }

		private void OnDisable()
		{
			IsFiring = false;
		}

		public bool Fire()
		{
			if (IsFiring)
				return false;

			StartCoroutine(FireRoutine());
			return true;
		}

		private IEnumerator FireRoutine()
		{
			IsFiring = true;

			FireOnce();

			if (_fireRate > 0)
				yield return new WaitForSeconds(1f / _fireRate);

			IsFiring = false;
		}

		protected virtual void FireOnce()
		{
			Bullet bullet = Instantiate(_bulletPrefab, _spawnPoint.position, transform.rotation);

			bullet.Apply(new BulletData {
				Speed = 20,
				Timeout = 10,
				SourceInstanceID = gameObject.GetInstanceID()
			});
		}
	}
}