using Entity;
using Entity.Enemy.Boss;
using Guns;
using UnityEngine;

namespace UI
{
	public class GameWinLoseView : MonoBehaviour
	{
		[SerializeField] private GameObject _gameWinView;
		[SerializeField] private GameObject _gameLostView;

		private BaseHealthSystem _playerHeath;
		private BaseHealthSystem _bossHeath;

		private void Awake()
		{
			_playerHeath = GameObject.FindWithTag("Player").GetComponent<BaseHealthSystem>();
			_bossHeath = FindObjectOfType<EnemyBoss>().GetComponent<BaseHealthSystem>();
		}

		private void Start()
		{
			_playerHeath.OnTakeDamage += damageData =>
			{
				if (damageData.CurrentHP <= 0)
					GameEnd(Target.Enemy);
			};

			_bossHeath.OnTakeDamage += damageData =>
			{
				if (damageData.CurrentHP <= 0)
					GameEnd(Target.Player);
			};
		}

		private void GameEnd(Target winner)
		{
			GameObject showView = winner == Target.Player ? _gameWinView : _gameLostView;

			Time.timeScale = 0;
			showView.SetActive(true);
		}
	}
}