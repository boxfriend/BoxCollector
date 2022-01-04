using MessagePack;
namespace Boxfriend
{
	[MessagePackObject(keyAsPropertyName: true)]
	public struct SessionData
	{
		public int Score { get; }
		public int Lives { get; }
		public int HighScore { get; }

		public SessionData (int score, int highScore, int lives)
		{
			Score = score;
			HighScore = highScore;
			Lives = lives;
		}
	}
}