using System.Collections;
using System.Collections.Generic;
using Guns;
using UnityEngine;

namespace Entity.Enemy
{
	public class AttackState : StateBase
	{
		private Coroutine _coroutine;

		public override void OnEnter()
		{
			_coroutine = StartCoroutine(AttackRoutine());
		}

		public override void OnExit()
		{
			_coroutine = null;
		}

		private IEnumerator AttackRoutine()
		{
			yield return WaitLookingToPlayer(1.5f);

			// TODO: Implement custom "enemy fire pattern"
			if (Data.Gun != null)
				Data.Gun.Fire(Target.Player);

			yield return WaitLookingToPlayer(1f);

			SetState<MovementState>();
		}

		private IEnumerator WaitLookingToPlayer(float waitSecs)
		{
			while ((waitSecs -= Time.deltaTime) > 0)
			{
				Data.Transform.LookAt(Data.PlayerTransform);
				yield return null;
			}
		}
	}
}