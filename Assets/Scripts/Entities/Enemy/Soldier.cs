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
		[SerializeField] private MovementState.StateProps _movementProps = new();

		private BaseHealthSystem _healthSystem;
		private CharacterController _characterController;

		private GunBase _weapon;
		private Transform _playerTransform;
		private float _moveSpeedWithVariation;
		private BasicStateMachine _stateMachine;

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

			SetupStateMachine();
		}

		private void SetupStateMachine()
		{
			var sharedData = new SharedData {
				Transform = transform,
				PlayerTransform = _playerTransform,
				CharacterController = _characterController,
				Gun = _weapon,
				MoveSpeed = _moveSpeedWithVariation
			};

			// Setup state machine behaviors
			_stateMachine = new BasicStateMachine(sharedData, StartCoroutine);
			_stateMachine.AddState<AttackState>();
			_stateMachine.AddState<MovementState, MovementState.StateProps>(_movementProps);
			_stateMachine.SetState<MovementState>();
		}

		private void Update()
		{
			_stateMachine.Tick();
		}

		private void OnDrawGizmosSelected()
		{
#if UNITY_EDITOR
			UnityEditor.Handles.color = Color.green;
			UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, _movementProps.ClosestDistance);

			UnityEditor.Handles.color = Color.red;
			UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, _movementProps.FarestDistance);

			UnityEditor.Handles.color = Color.white;
#endif
		}
	}
}