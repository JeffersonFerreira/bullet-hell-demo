using System.Collections;
using UnityEngine;

namespace Entity.Enemy
{
	public class EnemySpawn : MonoBehaviour
	{
		[SerializeField] private float _spawnEverySeconds = 10;

		[Space]
		[SerializeField] private Transform[] _spawnPoints;
		[SerializeField] private BasicEnemy[] _enemies;

		private IEnumerator Start()
		{
			int _spawnIndex = -1;
			while (true)
			{
				_spawnIndex = (_spawnIndex + 1) % _spawnPoints.Length;

				int i = Random.Range(0, _enemies.Length);

				Transform spawnPoint = _spawnPoints[_spawnIndex];
				BasicEnemy prefab = _enemies[i];

				Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
				yield return new WaitForSeconds(_spawnEverySeconds);
			}
		}
	}
}