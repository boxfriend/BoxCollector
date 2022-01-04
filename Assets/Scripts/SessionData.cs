namespace Boxfriend
{
	
	public readonly struct SessionData
	{
		private readonly int _score, _lives, _highScore;
		public int Score => _score;
		public int Lives => _lives;
		public int HighScore => _highScore;

		public SessionData (int score, int highScore, int lives)
		{
			_score = score;
			_highScore = highScore;
			_lives = lives;
		}
	}
}