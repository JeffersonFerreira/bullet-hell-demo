using System;
using UnityEngine;

namespace Entity
{
	public class BaseHealthSystem : MonoBehaviour
	{
		[SerializeField] private int _initialHealth = 3;
		[SerializeField] private float _invulnerabilityDuration = 0.2f;

		public event Action<DamageData> OnTakeDamage;

		public int InitialHealth => _initialHealth;
		public int Health { get; private set; }

		private float _lastDamageTime = float.NegativeInfinity;

		public bool IsInvulnerable { get; private set; }

		private void Awake()
		{
			Health = _initialHealth;
		}

		public void TakeDamage()
		{
			if (CanTakeDamage())
			{
				Health = Mathf.Max(0, Health - 1);
				_lastDamageTime = Time.time;
				OnTakeDamage?.Invoke(new DamageData(Health, _initialHealth));
			}
		}

		public bool CanTakeDamage()
		{
			if (IsInvulnerable || Health <= 0)
				return false;

			return Time.time > _lastDamageTime + _invulnerabilityDuration;
		}

		public void GrantInvulnerability()
		{
			IsInvulnerable = true;
		}

		public void RevokeInvulnerability()
		{
			IsInvulnerable = false;
		}
	}

	public readonly struct DamageData
	{
		public int CurrentHP { get; }
		public int TotalHP { get; }
		public float CurrentPercent => CurrentHP / (float) TotalHP;

		public DamageData(int currentHp, int totalHp)
		{
			TotalHP = totalHp;
			CurrentHP = currentHp;
		}
	}
}