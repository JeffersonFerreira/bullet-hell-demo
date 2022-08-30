using System;
using UnityEngine;

namespace Entity
{
	public class BaseHealthSystem : MonoBehaviour
	{
		[SerializeField] private int _initialHealth = 3;
		[SerializeField] private float _invulnerabilityDuration = 0.2f;

		public event Action<int> OnTakeDamage;

		public int Health { get; private set; }

		private float _lastDamageTime = float.NegativeInfinity;

		private void Awake()
		{
			Health = _initialHealth;
		}

		public void TakeDamage()
		{
			if (CanTakeDamage())
			{
				Health--;
				_lastDamageTime = Time.time;
				OnTakeDamage?.Invoke(Health);
			}
		}

		public bool CanTakeDamage()
		{
			return Time.time > _lastDamageTime + _invulnerabilityDuration;
		}
	}
}