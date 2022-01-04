using System;
using TMPro;
using UnityEngine;
namespace Boxfriend
{
	public class UIManager : MonoBehaviour
	{
		[SerializeField] private TMP_Text _score, _lives, _highScore;

		private void OnEnable ()
		{
			GameDataManager.OnUpdateStats += UpdateUI;
		}

		private void OnDisable ()
		{
			GameDataManager.OnUpdateStats += UpdateUI;
		}

		private void UpdateUI (SessionData data)
		{
			_score.text = data.Score.ToString();
			_lives.text = data.Lives.ToString();
			_highScore.text = data.HighScore.ToString();

		}
	}
}
