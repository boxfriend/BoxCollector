using System;
namespace Boxfriend.Utils
{
	public abstract class Singleton<T> where T : class, new()
	{
		public static T Instance { get; } = _instance ?? InitializeSingleton();
		private static T _instance;

		private static T InitializeSingleton (T obj = null)
		{
			return obj ?? new T();
		}

		public Singleton():this(null){}
		public Singleton (T obj = null)
		{
			InitializeSingleton(obj);
		}
	}
}
