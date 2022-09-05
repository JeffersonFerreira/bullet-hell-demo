using Entity;
using Entity.Enemy.Boss;
using Entity.Player;
using Guns;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
	public class GameWinLoseView : MonoBehaviour
	{
		[SerializeField] private GameObject _gameWinView;
		[SerializeField] private GameObject _gameLostView;

		[Space]
		[SerializeField] private Button[] _playAgainButtons;

		private BaseHealthSystem _playerHeath;
		private BaseHealthSystem _bossHeath;

		private void Awake()
		{
			_bossHeath = FindObjectOfType<EnemyBoss>().GetComponent<BaseHealthSystem>();
			_playerHeath = FindObjectOfType<PlayerController>().GetComponent<BaseHealthSystem>();
		}

		private void Start()
		{
			foreach (Button button in _playAgainButtons)
				button.onClick.AddListener(OnClickPlayAgain);

			_bossHeath.OnTakeDamage += damageData => SomeoneTookDamageWrapper(damageData, Target.Player);
			_playerHeath.OnTakeDamage += damageData => SomeoneTookDamageWrapper(damageData, Target.Enemy);
		}

		private void SomeoneTookDamageWrapper(DamageData damageData, Target winner)
		{
			if (damageData.CurrentHP > 0)
				return;

			GameEnd(winner);
			_playerHeath.GrantInvulnerability();
		}

		private void OnClickPlayAgain()
		{
			SceneManager.LoadScene("MainScene");
		}

		private void GameEnd(Target winner)
		{
			GameObject showView = winner == Target.Player ? _gameWinView : _gameLostView;

			Time.timeScale = 0;
			showView.SetActive(true);
		}
	}
}