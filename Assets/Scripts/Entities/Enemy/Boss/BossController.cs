using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Guns;
using UnityEngine;

namespace Entity.Enemy.Boss
{
	public class BossController : MonoBehaviour
	{
		[SerializeField] private Bullet _bossBullet;

		private Transform _playerTransform;

		private IEnumerator Start()
		{
			_playerTransform = GameObject.FindWithTag("Player").transform;
			yield return SpawnArcRoutine(new[] {9, 10}, 5);
			yield return SpawnArcRoutine(new[] {10}, 5);
			yield return VolleyRoutine(50);

			yield return VolleyArc(20, 5);
			yield return VolleyArc(20, 10, 0.01f);
		}

		private IEnumerator VolleyRoutine(int amount)
		{
			for (var i = 0; i < amount; i++)
			{
				Vector3 pos = transform.position;
				Vector3 playerDir = (_playerTransform.position - pos).normalized;
				Vector3 origin = pos + playerDir * 3;

				Fire(origin, playerDir, 10);
				yield return new WaitForSeconds(0.15f);
			}
		}

		private IEnumerator VolleyArc(int bulletsPerRow, int rows = 1, float delay = 0.05f)
		{
			Vector3 pos = transform.position;
			Vector3 forward = transform.forward;

			var arcDirs = new List<Vector3>();

			for (var i = 0; i < rows; i++)
			{
				bool reverse = i % 2 == 0;

				arcDirs.AddRange(!reverse
					? ComputeArcPoints(bulletsPerRow, forward)
					: ComputeArcPoints(bulletsPerRow, forward).Reverse()
				);
			}

			var wait = new WaitForSeconds(delay);
			foreach (Vector3 dir in arcDirs)
			{
				Vector3 origin = pos + dir * 3;
				Fire(origin, dir, 5);
				yield return wait;
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

		private IEnumerator SpawnArcRoutine(int[] pattern, int repeatCount)
		{
			Vector3 pos = transform.position;

			foreach (int spawnCount in Enumerable.Repeat(pattern, repeatCount).SelectMany(arr => arr))
			{
				foreach (Vector3 dir in ComputeArcPoints(spawnCount, transform.forward))
				{
					Vector3 origin = pos + dir * 3;
					Bullet bullet = Instantiate(_bossBullet, origin, Quaternion.LookRotation(dir));

					bullet.Apply(new BulletData {
						Speed = 3,
						Origin = origin,
						SourceInstanceID = gameObject.GetInstanceID(),
						Timeout = 10
					});
				}

				yield return new WaitForSeconds(0.5f);
			}
		}

		private static IEnumerable<Vector3> ComputeArcPoints(int amount, Vector3 dir)
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