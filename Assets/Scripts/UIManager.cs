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
			_score.text = $"Score: {data.Score}";
			_lives.text = $"Lives: {data.Lives}";
			_highScore.text = $"High Score: {data.HighScore}";
		}
	}
}
