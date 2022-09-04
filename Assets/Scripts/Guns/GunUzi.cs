using UnityEngine;

namespace Guns
{
	public class GunUzi : GunBase
	{
		[SerializeField] private GunBase _gunLeft, _gunRight;

		private bool _flag;

		protected override void FireOnce(Target target)
		{
			bool success = _flag ? _gunLeft.Fire(target) : _gunRight.Fire(target);

			if (success)
				_flag ^= true;
		}
	}
}