using System.Collections;
using Guns;
using UnityEngine;

namespace Entity.Enemy
{
	[RequireComponent(typeof(BaseHealthSystem))]
	[RequireComponent(typeof(CharacterController))]
	public class BasicEnemy : MonoBehaviour
	{
		[Header("Movement")]
		[Range(0, 10)]
		[SerializeField] private float _moveSpeedVariationRange;
		[SerializeField] private float _moveSpeedBase = 5;
		[SerializeField] private MovementProps _movementProps = new();

		[Header("OnHit KnockBack")]
		[SerializeField] private float _knockBackDuration = 0.2f;
		[SerializeField] private float _knockBackForce = 0.01f;

		private BaseHealthSystem _healthSystem;
		private CharacterController _characterController;

		private Transform _playerTransform;
		private float _moveSpeedWithVariation;

		private GunBase _weapon;
		private Coroutine _knockback;
		private BasicStateMachine _stateMachine;

		private void Awake()
		{
			_weapon = GetComponentInChildren<GunBase>();
			_healthSystem = GetComponent<BaseHealthSystem>();
			_characterController = GetComponent<CharacterController>();

			_playerTransform = GameObject.FindWithTag("Player").transform;
		}

		private void Start()
		{
			_moveSpeedWithVariation = Mathf.Max(
				0.5f,
				_moveSpeedBase + Random.Range(-_moveSpeedVariationRange, _moveSpeedVariationRange)
			);

			_healthSystem.OnTakeDamage += HealthSystem_OnTakeDamage;
			SetupStateMachine();
		}

		private void HealthSystem_OnTakeDamage(DamageData damageData)
		{
			if (damageData.CurrentHP <= 0)
				Destroy(gameObject);
		}

		private void Update()
		{
			_stateMachine.Tick();
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (!collision.gameObject.TryGetComponent(out Bullet bullet))
				return;

			Vector3 hitDirection = transform.position - bullet.Data.Origin;
			hitDirection.y = 0;
			hitDirection.Normalize();

			_stateMachine.SetStateWithProps<GotHitState, GotHitProps>(new GotHitProps(hitDirection, _knockBackDuration, _knockBackForce));
		}

		private void OnDrawGizmosSelected()
		{
#if UNITY_EDITOR
			UnityEditor.Handles.color = Color.green;
			UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, _movementProps.ClosestDistance);

			UnityEditor.Handles.color = Color.red;
			UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, _movementProps.FarestDistance);
#endif
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
			_stateMachine = new BasicStateMachine(sharedData, this);
			_stateMachine.AddState<AttackState>();
			_stateMachine.AddState<GotHitState>();
			_stateMachine.AddState<MovementState, MovementProps>(_movementProps);

			_stateMachine.SetState<MovementState>();
		}
	}
}