using Guns;
using UnityEngine;

namespace Entity.Player
{
	public class PlayerGunController : MonoBehaviour
	{
		private GunBase _gun;

		private void Start()
		{
			_gun = GetComponentInChildren<GunBase>();
		}

		private void Update()
		{
			if (Input.GetMouseButton(0))
				_gun.Fire(Target.Enemy);
		}
	}
}