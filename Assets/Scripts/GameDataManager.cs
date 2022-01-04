using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Boxfriend.Utils;
using MessagePack;
using UnityEngine;

namespace Boxfriend
{
    public class GameDataManager : MonoBehaviour
    {
        public int Score { get; private set; }
        public int HighScore { get; private set; }
        public int Lives
        {
            get => _lives;
            private set 
            {
                _lives = value;
                if(_lives < 0)
                    OnGameOver?.Invoke();
            }
        }

        public static event Action<SessionData> OnUpdateStats;
        public static event Action OnGameOver;

        private static readonly SessionData _defaultData = new(0,0,3);
        private SessionData _data;
        private int _lives = 3;

        private void OnEnable ()
        {
            Box.OnBoxClick += ProcessClick;
            OnGameOver += GameOver;
        }
        private void OnDisable ()
        {
            Box.OnBoxClick -= ProcessClick;
            OnGameOver -= GameOver;
        }

        private async void Start ()
        {
            await LoadData();

            if (_data.Lives <= 0)
            {
                var data = new SessionData(0, _data.HighScore, 3);
                _data = data;
            }
            
            Lives = _data.Lives;
            Score = _data.Score;
            HighScore = _data.HighScore;
            OnUpdateStats?.Invoke(_data);
        }

        private void ProcessClick (int i)
        {
            if (i < 0)
            {
                Lives--;
            } else
            {
                Score += i;
                HighScore = Score > HighScore ? Score : HighScore;
            }
            _data = new SessionData(Score, HighScore, Lives);
            OnUpdateStats?.Invoke(_data);
        }

        private async void GameOver ()
        {
            Debug.Log("Game Over");
            await WriteData(_data);
            //TODO: add restart stuff probably
        }

        private async Task LoadData (string fileName = "SaveData.box")
        {
            var file = $@"{Application.persistentDataPath}\{fileName}";
            
            if (!File.Exists(file)) await WriteData(_defaultData);

            await using var fileStream = new FileStream(file, FileMode.Open);
            using var streamReader = new StreamReader(fileStream);
            var data = MessagePackSerializer.DeserializeAsync<SessionData>(fileStream);
            _data = data.Result;
        }
        
        private async Task WriteData (SessionData data, string fileName = "SaveData.box")
        {
            var path = $"{Application.persistentDataPath}";
            var file = $@"{path}\{fileName}";
            
            await using var fileStream = new FileStream(file, FileMode.OpenOrCreate);
            var json = MessagePackSerializer.Serialize(data);
            await fileStream.WriteAsync(json);
            fileStream.Close();
        }
        
    }
}
