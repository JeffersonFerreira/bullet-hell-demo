using System.Collections;
using UnityEngine;

namespace Entity.Enemy
{
	public class GotHitState : StateBase<GotHitProps>
	{
		private Coroutine _knockback;

		public override void OnEnter()
		{
			if (_knockback != null)
				StopCoroutine(_knockback);

			_knockback = StartCoroutine(KnockBackRoutine(Props.HitDirection * Props.KnockBackForce, Props.KnockBackDuration));
		}

		private IEnumerator KnockBackRoutine(Vector3 speed, float secs = 0.2f)
		{
			while ((secs -= Time.deltaTime) > 0)
			{
				Data.CharacterController.Move(speed);
				yield return null;
			}

			_knockback = null;

			SetState<MovementState>();
		}
	}

	public class GotHitProps
	{
		public Vector3 HitDirection;
		public float KnockBackDuration = 0.2f;
		public float KnockBackForce = 0.01f;

		public GotHitProps(Vector3 hitDirection, float knockBackDuration, float knockBackForce)
		{
			HitDirection = hitDirection;
			KnockBackDuration = knockBackDuration;
			KnockBackForce = knockBackForce;
		}
	}
}