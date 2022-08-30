using Guns;
using UnityEngine;

namespace Entity.Enemy
{
	[RequireComponent(typeof(BaseHealthSystem))]
	[RequireComponent(typeof(CharacterController))]
	public class Soldier : MonoBehaviour
	{
		[Range(0, 10)]
		[SerializeField] private float _moveSpeedVariationRange;
		[SerializeField] private float _moveSpeedBase = 5;

		private BaseHealthSystem _healthSystem;
		private CharacterController _characterController;

		private GunBase _weapon;
		private Transform _playerTransform;
		private float _moveSpeedWithVariation;
		private SoldierStateMachine _stateMachine;

		private void Awake()
		{
			_weapon = GetComponentInChildren<GunBase>();
			_healthSystem = GetComponent<BaseHealthSystem>();
			_characterController = GetComponent<CharacterController>();

			_playerTransform = GameObject.FindWithTag("Player").transform;
			_moveSpeedWithVariation = Mathf.Max(
				0.5f,
				_moveSpeedBase + Random.Range(-_moveSpeedVariationRange, _moveSpeedVariationRange)
			);

			var sharedData = new SharedData {
				Transform = transform,
				PlayerTransform = _playerTransform,
				CharacterController = _characterController,
				Gun = _weapon,
				MoveSpeed = _moveSpeedWithVariation
			};

			_stateMachine = new SoldierStateMachine(sharedData, StartCoroutine);
		}

		private void Update()
		{
			// UpdateMovement();

			_stateMachine.Tick();

		}

		private void UpdateMovement()
		{
			Vector3 playerDir = (_playerTransform.position - transform.position).normalized;
			_characterController.SimpleMove(playerDir * _moveSpeedWithVariation);
		}
	}
}