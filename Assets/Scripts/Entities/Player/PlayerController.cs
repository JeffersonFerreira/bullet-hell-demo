using UnityEngine;

namespace Entity.Player
{
	public class PlayerController : MonoBehaviour
	{
		private PlayerMovement _movement;
		private BaseHealthSystem _healthSystem;
		private PlayerGunController _gunController;

		private void Awake()
		{
			_movement = GetComponent<PlayerMovement>();
			_healthSystem = GetComponent<BaseHealthSystem>();
			_gunController = GetComponent<PlayerGunController>();
		}

		private void Start()
		{
			_movement.OnDash += Movement_OnDash;
			_healthSystem.OnTakeDamage += HealthSystem_OnTakeDamage;
		}

		private void HealthSystem_OnTakeDamage(DamageData damageData)
		{
			if (damageData.CurrentHP > 0)
				return;

			_movement.enabled = false;
			_gunController.enabled = false;
		}

		private void Movement_OnDash(DashData dashData)
		{
			switch (dashData.State)
			{
				case DashState.Began:
					_healthSystem.GrantInvulnerability();
					break;

				case DashState.Completed:
					_healthSystem.RevokeInvulnerability();
					break;
			}
		}
	}
}