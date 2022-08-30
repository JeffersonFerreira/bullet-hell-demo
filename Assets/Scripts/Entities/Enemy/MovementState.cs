using UnityEngine;

namespace Entity.Enemy
{
	public class MovementState : StateBase
	{
		public override void Tick()
		{
			Vector3 playerDir = (Data.PlayerTransform.position - Data.Transform.position).normalized;

			Data.Transform.LookAt(Data.PlayerTransform);
			Data.CharacterController.SimpleMove(playerDir * Data.MoveSpeed);

			if (Data.Gun != null && Data.Gun.CanFire())
				SetState<AttackState>();
		}
	}
}