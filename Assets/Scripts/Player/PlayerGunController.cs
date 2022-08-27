using Guns;
using UnityEngine;

namespace Player
{
	public class PlayerGunController : MonoBehaviour
	{
		public GunBase ActiveGun;

		private void Update()
		{
			if (Input.GetMouseButton(0))
				ActiveGun.Fire();
		}
	}
}