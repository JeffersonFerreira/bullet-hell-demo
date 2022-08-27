using UnityEngine;

namespace Guns
{
	[CreateAssetMenu(fileName = "GunConfig", menuName = "Game/Gun config", order = 0)]
	public class GunConfig : ScriptableObject
	{
		public float speed = 20;
		public float timeout = 20;
	}
}