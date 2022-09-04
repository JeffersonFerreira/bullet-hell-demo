using Entity;
using Entity.Enemy.Boss;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class HeathBar : MonoBehaviour
	{
		[SerializeField] private Slider _slider;
		[SerializeField] private BaseHealthSystem _healthSystem;

		private void Start()
		{
			_healthSystem.OnTakeDamage += HealthSystem_OnTakeDamage;
			_slider.value = 1;
		}

		private void HealthSystem_OnTakeDamage(DamageData damageData)
		{
			_slider.value = damageData.CurrentPercent;
		}
	}
}