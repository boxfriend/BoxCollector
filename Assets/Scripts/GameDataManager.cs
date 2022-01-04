using System;
using System.Collections;
using System.Collections.Generic;
using Boxfriend.Utils;
using UnityEngine;

namespace Boxfriend
{
    public class GameDataManager : MonoBehaviour
    {
        public int Score { get; private set; }
        public int HighSchore { get; private set; }
        public int Lives 
        { 
            get => _lives; 
            private set => _lives = value; 
        }

        private int _lives = 3;

        public static event Action<SessionData> OnUpdateStats;
        
        private void OnEnable ()
        {
            Box.OnBoxClick += ProcessClick;
        }
        private void OnDisable ()
        {
            Box.OnBoxClick -= ProcessClick;
        }

        private void ProcessClick (int i)
        {
            if (i < 0)
            {
                Lives--;
            } else
            {
                Score += i;
                HighSchore = Score > HighSchore ? Score : HighSchore;
            }
            var data = new SessionData(Score, HighSchore, Lives);
            OnUpdateStats?.Invoke(data);
        }
        
        
    }
}
