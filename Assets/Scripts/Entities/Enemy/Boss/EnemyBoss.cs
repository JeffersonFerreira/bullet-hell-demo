using UnityEngine;

namespace Entity.Enemy.Boss
{
	public class EnemyBoss : MonoBehaviour
	{
		public BaseHealthSystem HealthSystem { get; private set; }
		private BossAttackBehaviours _attackBehaviours;

		private void Awake()
		{
			HealthSystem = GetComponent<BaseHealthSystem>();
			_attackBehaviours = GetComponent<BossAttackBehaviours>();
		}

		private void Start()
		{
			HealthSystem.OnTakeDamage += BaseHealthSystem_OnTakeDamage;
		}

		private void BaseHealthSystem_OnTakeDamage(DamageData damageData)
		{
			if (damageData.CurrentHP <= 0)
			{
				Destroy(gameObject);
				Debug.Log("You Won");
			}
		}
	}
}