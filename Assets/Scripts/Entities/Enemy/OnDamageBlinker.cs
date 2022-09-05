using System.Collections;
using UnityEngine;

namespace Entity.Enemy
{
	public class OnDamageBlinker : MonoBehaviour
	{
		[SerializeField] private Color _blinkColor = Color.white;

		private static readonly int _propColor = Shader.PropertyToID("_EmissionColor");

		private Renderer[] _renderers;
		private BaseHealthSystem _healthSystem;
		private MaterialPropertyBlock _propertyBlock;

		private bool _blink;

		private void Awake()
		{
			_healthSystem = GetComponent<BaseHealthSystem>();
			_renderers = GetComponentsInChildren<Renderer>();
			_propertyBlock = new MaterialPropertyBlock();
		}

		private void Start()
		{
			_healthSystem.OnTakeDamage += _ => _blink = true;

			StartCoroutine(BlinkRoutine());
		}

		private IEnumerator BlinkRoutine()
		{
			var waitFrame = new WaitForEndOfFrame();

			IEnumerator Wait()
			{
				yield return waitFrame;
				yield return waitFrame;
				yield return waitFrame;
			}

			while (true)
			{
				if (!_blink)
				{
					yield return null;
					continue;
				}

				// Turn on, then off
				for (int i = 0; i < 2; i++)
				{
					Color color = i == 0 ? _blinkColor : Color.black;

					_propertyBlock.SetColor(_propColor, color);

					foreach (Renderer r in _renderers)
						r.SetPropertyBlock(_propertyBlock);

					yield return Wait();
				}

				// Reset
				_blink = false;
			}
		}
	}
}