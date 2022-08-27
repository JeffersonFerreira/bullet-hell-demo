using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform PlayerTransform;
	[SerializeField] private float _smoothTime;

	private Vector3 _offset;
	private Vector3 _currVel;

	private void Start()
	{
		ComputeCurrentOffset();
	}

	private void LateUpdate()
	{
		Vector3 current = transform.position;
		Vector3 target = PlayerTransform.position + _offset;

		transform.position = Vector3.SmoothDamp(current, target, ref _currVel, _smoothTime);
	}

	private void ComputeCurrentOffset()
	{
		_offset = transform.position - PlayerTransform.position;
	}
}