using System.Collections;
using UnityEngine;

namespace Entity.Player
{
	public class PlayerHitTimeShift : MonoBehaviour
	{
		[SerializeField] private float _duration = 0.2f;
		[SerializeField] private float _smoothTime = 0.1f;

		[Range(0, 1)]
		[SerializeField] private float _slowScale;

		private BaseHealthSystem _healthSystem;
		private Coroutine _coroutine;

		private void Awake()
		{
			_healthSystem = GetComponent<BaseHealthSystem>();
		}

		private void Start()
		{
			_healthSystem.OnTakeDamage += HealthSystem_OnTakeDamage;
		}

		private void HealthSystem_OnTakeDamage(DamageData _)
		{
			if (_coroutine != null)
				StopCoroutine(_coroutine);

			_coroutine = StartCoroutine(TimeShiftRoutine());
		}

		private IEnumerator TimeShiftRoutine()
		{
			/*
			 * Warning: Must use "fixedUnscaledDeltaTime" within this method.
			 * "deltaTime" definitely will not work since it is timeScale dependant
			 * and for some reason, "unscaledDeltaTime" also didn't (I don't know why)
			 */
			float vel = 0;

			// Perform slowness
			while (Time.timeScale > _slowScale)
			{
				Time.timeScale = Mathf.SmoothDamp(Time.timeScale, _slowScale, ref vel, _smoothTime, float.PositiveInfinity, Time.fixedUnscaledDeltaTime);
				yield return null;
			}

			// Effect duration
			float t = _duration;
			while (t > 0)
			{
				t -= Time.fixedUnscaledDeltaTime;
				yield return null;
			}

			vel = 0;
			// Perform time normalization
			while (Time.timeScale < 1)
			{
				Time.timeScale = Mathf.SmoothDamp(Time.timeScale, 1, ref vel, _smoothTime, float.PositiveInfinity, Time.fixedUnscaledDeltaTime);
				yield return null;
			}

			_coroutine = null;
		}
	}
}