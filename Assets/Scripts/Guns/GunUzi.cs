using UnityEngine;

namespace Guns
{
	public class GunUzi : GunBase
	{
		[SerializeField] private GunBase _gunLeft, _gunRight;

		private bool _flag;

		protected override void FireOnce()
		{
			bool success = _flag ? _gunLeft.Fire() : _gunRight.Fire();

			if (success)
				_flag ^= true;
		}
	}
}