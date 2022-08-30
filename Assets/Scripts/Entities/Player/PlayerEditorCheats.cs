using UnityEngine;

namespace Player
{
	public class PlayerEditorCheats : MonoBehaviour
	{
		private BaseHealthSystem _healthSystem;
		private PlayerMovement _movement;
		private PlayerGunController _gunController;

		private void Awake()
		{
			_healthSystem = GetComponent<BaseHealthSystem>();
			_movement = GetComponent<PlayerMovement>();
			_gunController = GetComponent<PlayerGunController>();
		}

		private void Update()
		{
			if (Input.GetMouseButtonDown(1))
				_gunController.NextGun();

			if (Input.GetKeyDown(KeyCode.RightBracket))
				_movement.ApplySlowness();

			if (Input.GetKeyDown(KeyCode.Equals))
				_healthSystem.TakeDamage();
		}
	}
}