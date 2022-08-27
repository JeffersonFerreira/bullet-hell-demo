using Guns;
using UnityEngine;

namespace Player
{
	public class PlayerGunController : MonoBehaviour
	{
		[SerializeField] private GunBase[] _guns;

		private int _activeGunIndex = -1;
		public GunBase ActiveGun => _guns[_activeGunIndex];

		private void Start()
		{
			NextGun();
		}

		private void Update()
		{
			if (Input.GetMouseButton(0))
				ActiveGun.Fire();
		}

		public void NextGun()
		{
			foreach (GunBase g in _guns)
				g.gameObject.SetActive(false);

			_activeGunIndex = (_activeGunIndex + 1) % _guns.Length;
			ActiveGun.gameObject.SetActive(true);
		}
	}
}