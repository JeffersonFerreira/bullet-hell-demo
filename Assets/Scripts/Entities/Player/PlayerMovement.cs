using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[Header("Move")]
	[SerializeField] private float _moveSpeed = 5;

	[Header("Dash")]
	[SerializeField] private float _dashMultiplier = 0.5f;
	[SerializeField] private float _dashDuration = 0.25f;
	[SerializeField] private float _dashCooldown = 0.5f;

	[Header("Slow Effect")]
	[Range(0, 1)]
	[SerializeField] private float _maxSlowAmount = 0.7f;
	[SerializeField] private float _slowDuration = 2f;

	public event Action<DashData> OnDash;
	public event Action OnSlowed;

	private Camera _camera;
	private CharacterController _characterController;

	public bool IsDashing { get; private set; }

	private float _slowAmount;
	private float _lastSlowTime;
	private float _lastDashTime;

	private void Awake()
	{
		_camera = Camera.main;
		_characterController = GetComponent<CharacterController>();
	}

	private void Update()
	{
		UpdateRotation();
		UpdateSlowEffectDecay();

		if (!IsDashing)
		{
			UpdateMovement();

			if (Input.GetKeyDown(KeyCode.Space) && Time.time > _lastDashTime + _dashCooldown)
				StartCoroutine(DashRoutine());
		}
	}

	public void ApplySlowness(float effectValue = 0.2f)
	{
		OnSlowed?.Invoke();
		_lastSlowTime = Time.time;
		_slowAmount = Mathf.Min(_maxSlowAmount, _slowAmount + effectValue);
	}

	private void UpdateSlowEffectDecay()
	{
		if (_slowAmount > 0 && Time.time > _lastSlowTime + _slowDuration)
			_slowAmount = Mathf.Max(0, _slowAmount - Time.deltaTime);
	}

	private void UpdateMovement()
	{
		var x = Input.GetAxisRaw("Horizontal");
		var y = Input.GetAxisRaw("Vertical");

		var moveDir = new Vector3(x, 0, y);
		_characterController.SimpleMove(moveDir * (_moveSpeed * (1 - _slowAmount)));
	}

	private void UpdateRotation()
	{
		Vector2 inputPos = Input.mousePosition;
		Vector2 playerScreenPos = _camera.WorldToScreenPoint(transform.position);
		Vector2 diff = inputPos - playerScreenPos;

		float lookAtAngle = Mathf.Atan2(diff.x, diff.y) * Mathf.Rad2Deg;

		transform.rotation = Quaternion.Euler(new Vector3(0, lookAtAngle, 0));
	}

	private IEnumerator DashRoutine()
	{
		IsDashing = true;

		float t = _dashDuration;
		Vector3 moveDir = _characterController.velocity.normalized;

		OnDash?.Invoke(new DashData(transform.position, moveDir, DashState.Began));

		while ((t -= Time.deltaTime) > 0)
		{
			_characterController.SimpleMove(moveDir * (_moveSpeed * _dashMultiplier));
			yield return null;
		}

		_lastDashTime = Time.time;
		IsDashing = false;

		OnDash?.Invoke(new DashData(transform.position, moveDir, DashState.Completed));
	}
}

public readonly struct DashData
{
	public Vector3 Point { get; }
	public Vector3 Direction { get; }
	public DashState State { get; }

	public DashData(Vector3 point, Vector3 direction, DashState state)
	{
		Point = point;
		Direction = direction;
		State = state;
	}
}

public enum DashState
{
	Began,
	Completed
}