using System.Collections;
using UnityEngine;

namespace Guns
{
	public class GunBase : MonoBehaviour
	{
		[SerializeField] private Bullet _bulletPrefab;

		[Header("Fire Settings")]
		[SerializeField] private Transform _spawnPoint;
		[SerializeField] private float _fireRate;
		[SerializeField] private float _cooldown;

		private bool _isFiring;

		public void Fire()
		{
			if (_isFiring)
				return;

			StartCoroutine(FireRoutine());
		}

		private IEnumerator FireRoutine()
		{
			_isFiring = true;

			FireOnce();
			yield return new WaitForSeconds(_cooldown);

			_isFiring = false;
		}

		private void FireOnce()
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