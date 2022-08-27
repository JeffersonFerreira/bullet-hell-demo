using UnityEngine;

namespace Player
{
	public class PlayerEditorCheats : MonoBehaviour
	{
		private PlayerHealth _health;
		private PlayerMovement _movement;
		private PlayerGunController _gunController;

		private void Awake()
		{
			_health = GetComponent<PlayerHealth>();
			_movement = GetComponent<PlayerMovement>();
			_gunController = GetComponent<PlayerGunController>();
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.LeftBracket))
				_gunController.NextGun();

			if (Input.GetKeyDown(KeyCode.RightBracket))
				_movement.ApplySlowness();

			if (Input.GetKeyDown(KeyCode.Equals))
				_health.TakeDamage();
		}
	}
}