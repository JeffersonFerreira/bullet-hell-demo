using Entity;
using Entity.Enemy.Boss;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class BossHeathBar : MonoBehaviour
	{
		[SerializeField] private Slider _slider;

		private EnemyBoss _boss;

		private void Awake()
		{
			_boss = FindObjectOfType<EnemyBoss>();
		}

		private void Start()
		{
			_boss.HealthSystem.OnTakeDamage += HealthSystem_OnTakeDamage;
			_slider.value = 1;
		}

		private void HealthSystem_OnTakeDamage(DamageData damageData)
		{
			_slider.value = damageData.CurrentPercent;
		}
	}
}