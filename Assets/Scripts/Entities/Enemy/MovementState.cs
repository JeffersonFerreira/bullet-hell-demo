using System;
using UnityEngine;

namespace Entity.Enemy
{
	public class MovementState : StateBase<MovementProps>
	{
		public override void Tick()
		{
			Vector3 playerDir = Data.PlayerTransform.position - Data.Transform.position;

			Data.Transform.LookAt(Data.PlayerTransform);
			Data.CharacterController.SimpleMove(playerDir.normalized * (Data.MoveSpeed * MoveDir(playerDir)));

			if (Data.Gun != null && Data.Gun.CanFire())
				SetState<AttackState>();
		}

		private int MoveDir(Vector3 playerDir)
		{
			float mag = playerDir.sqrMagnitude;

			// Move closer
			if (mag > Props.FarestDistance * Props.FarestDistance)
				return 1;

			// Move far
			if (mag < Props.ClosestDistance * Props.ClosestDistance)
				return -1;

			// Stay still
			return 0;
		}
	}

	[Serializable]
	public class MovementProps
	{
		public float ClosestDistance = 1;
		public float FarestDistance = 5;
	}
}