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

	[Header("Slow Effect")]
	[Range(0, 1)]
	[SerializeField] private float _maxSlowAmount = 0.7f;
	[SerializeField] private float _slowDuration = 2f;

	public event Action OnDash;
	public event Action OnSlowed;

	private Camera _camera;
	private CharacterController _characterController;

	private bool _isDashing;
	private float _slowAmount;
	private float _lastSlowTime;

	private void Awake()
	{
		_camera = Camera.main;
		_characterController = GetComponent<CharacterController>();
	}

	private void Update()
	{
		if (!_isDashing)
		{
			UpdateMovement();
			UpdateRotation();
			UpdateSlowDecay();

			if (Input.GetKeyDown(KeyCode.LeftShift))
				StartCoroutine(DashRoutine());
		}
	}

	public void ApplySlowness(float effectValue = 0.2f)
	{
		OnSlowed?.Invoke();
		_lastSlowTime = Time.time;
		_slowAmount = Mathf.Min(_maxSlowAmount, _slowAmount + effectValue);
	}

	private void UpdateSlowDecay()
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
		_isDashing = true;

		OnDash?.Invoke();
		float t = _dashDuration;
		Vector3 moveDir = _characterController.velocity.normalized;

		while ((t -= Time.deltaTime) > 0)
		{
			_characterController.SimpleMove(moveDir * (_moveSpeed * _dashMultiplier));
			yield return null;
		}

		_isDashing = false;
	}
}