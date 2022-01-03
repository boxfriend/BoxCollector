using UnityEngine;

namespace Boxfriend.Utils
{
	public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
	{
		private static T _instance;
		[SerializeField] protected bool _dontDestroy;

		public static T Instance
		{
			get => _instance;
			private set {
				if (_instance == null)
					_instance = value;
				else if (value != _instance)
					Destroy(value.gameObject);
			}
		}
        
		protected virtual void Awake ()
		{
			Instance = this as T;
            
			if(_dontDestroy)
				DontDestroyOnLoad(gameObject);
		}
	}
}
