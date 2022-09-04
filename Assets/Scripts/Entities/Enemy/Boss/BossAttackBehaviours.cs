using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Guns;
using UnityEngine;

namespace Entity.Enemy.Boss
{
	public class BossAttackBehaviours : MonoBehaviour
	{
		[SerializeField] private Bullet _bossBullet;

		private Transform _playerTransform;

		private void Awake()
		{
			_playerTransform = GameObject.FindWithTag("Player").transform;
		}

		private IEnumerator Start()
		{
			int volleyCount = 0;

			while (true)
			{
				if (volleyCount >= 2 || Random.Range(0, 3) == 0)
				{
					volleyCount = Mathf.Max(0, volleyCount - 1);
					yield return UseRandomPattern();
				}
				else
				{
					volleyCount++;
					int amount = Random.Range(25, 40);
					float bulletSpeed = Random.Range(8f, 18f);
					yield return VolleyRoutine(amount, bulletSpeed);
				}

				yield return new WaitForSeconds(1f);
			}
		}

		private IEnumerator UseRandomPattern()
		{
			int patternToUse = Random.Range(0, 4);

			switch (patternToUse) {
				case 0: yield return FullArcRoutine(new[] {9, 10}, 5); break;
				case 1: yield return FullArcRoutine(new[] {10}, 5); break;
				case 2: yield return VolleyArcRoutine(20, 5); break;
				case 3: yield return VolleyArcRoutine(20, 10, 0.01f); break;
			}
		}

		private void Fire(Vector3 pos, Vector3 dir, float speed = 5)
		{
			Bullet bullet = Instantiate(_bossBullet, pos, Quaternion.LookRotation(dir));

			bullet.Apply(new BulletData {
				Speed = speed,
				Origin = pos,
				SourceInstanceID = gameObject.GetInstanceID(),
				Timeout = 10
			});
		}

		private IEnumerator VolleyRoutine(int amount, float bulletSpeed)
		{
			for (var i = 0; i < amount; i++)
			{
				Vector3 pos = transform.position;
				Vector3 playerDir = (_playerTransform.position - pos).normalized;
				Vector3 origin = pos + playerDir * 3;

				Fire(origin, playerDir, bulletSpeed);
				yield return new WaitForSeconds(0.15f);
			}
		}

		private IEnumerator VolleyArcRoutine(int bulletsPerRow, int rows = 1, float fireDelay = 0.05f)
		{
			Vector3 pos = transform.position;
			Vector3 forward = transform.forward;

			var volleyArcDirections = Enumerable
				.Range(0, rows)
				.SelectMany(index => {
					bool reverse = index % 2 == 0;
					return !reverse
						? ComputeArcDirections(bulletsPerRow, forward)
						: ComputeArcDirections(bulletsPerRow, forward).Reverse();
				});

			var wait = new WaitForSeconds(fireDelay);
			foreach (Vector3 dir in volleyArcDirections)
			{
				Vector3 origin = pos + dir * 3;
				Fire(origin, dir, 5);
				yield return wait;
			}
		}

		private IEnumerator FullArcRoutine(int[] pattern, int repeatCount)
		{
			Vector3 pos = transform.position;

			foreach (int spawnCount in Enumerable.Repeat(pattern, repeatCount).SelectMany(arr => arr))
			{
				foreach (Vector3 dir in ComputeArcDirections(spawnCount, transform.forward))
				{
					Vector3 origin = pos + dir * 3;
					Fire(origin, dir, 3);
				}

				yield return new WaitForSeconds(0.5f);
			}
		}

		private static IEnumerable<Vector3> ComputeArcDirections(int amount, Vector3 dir)
		{
			amount++;

			float forwardAngle = Mathf.Atan2(dir.z, dir.x);

			for (int i = 1; i < amount; i++)
			{
				float angle = forwardAngle + (Mathf.PI / amount) * i;
				angle -= (Mathf.Deg2Rad * (180 / 2f));

				var vec = new Vector3(
					Mathf.Cos(angle),
					0,
					Mathf.Sin(angle)
				);

				yield return vec;
			}
		}
	}
}