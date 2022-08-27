using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private float _moveSpeed = 5;

	private Camera _camera;
	private CharacterController _characterController;

	private void Awake()
	{
		_camera = Camera.main;
		_characterController = GetComponent<CharacterController>();
	}

	private void Update()
	{
		UpdateMovement();
		UpdateRotation();
	}

	private void UpdateMovement()
	{
		var x = Input.GetAxisRaw("Horizontal");
		var y = Input.GetAxisRaw("Vertical");

		var moveDir = new Vector3(x, 0, y);
		_characterController.SimpleMove(moveDir * _moveSpeed);
	}

	private void UpdateRotation()
	{
		Vector2 inputPos = Input.mousePosition;
		Vector2 playerScreenPos = _camera.WorldToScreenPoint(transform.position);
		Vector2 diff = inputPos - playerScreenPos;

		float lookAtAngle = Mathf.Atan2(diff.x, diff.y) * Mathf.Rad2Deg;

		transform.rotation = Quaternion.Euler(new Vector3(0, lookAtAngle, 0));
	}
}