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
        public int Lives { get; private set; } = 3;

        public static event Action<SessionData> OnUpdateStats;

        private static readonly SessionData _defaultData = new(0,0,3);
        
        private void OnEnable ()
        {
            Box.OnBoxClick += ProcessClick;
        }
        private void OnDisable ()
        {
            Box.OnBoxClick -= ProcessClick;
        }

        private void Start ()
        {
            var load = LoadSettings();
            HighScore = load.Result.HighScore;
            Score = load.Result.Score;
            Lives = load.Result.Lives;
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
            var data = new SessionData(Score, HighScore, Lives);
            OnUpdateStats?.Invoke(data);
        }

        private async Task<SessionData> LoadSettings (string fileName = "SaveData.box")
        {
            var file = $@"{Application.persistentDataPath}\{fileName}";
            
            if (!File.Exists(file)) await WriteSettings(_defaultData);

            await using var fileStream = new FileStream(file, FileMode.Open);
            using var streamReader = new StreamReader(fileStream);
            var str = streamReader.ReadToEndAsync();
            var json = MessagePackSerializer.ConvertFromJson(str.Result);
            var mPack = MessagePackSerializer.Deserialize<SessionData>(json);
            
            OnUpdateStats?.Invoke(mPack);
            return mPack;
        }
        
        private async Task WriteSettings (SessionData data, string fileName = "SaveData.box")
        {
            var path = $"{Application.persistentDataPath}";
            var file = $@"{path}\{fileName}";
            
            await using var fileStream = new FileStream(file, FileMode.OpenOrCreate);
            await using var streamWriter = new StreamWriter(fileStream);
            var json = MessagePackSerializer.SerializeToJson(data);
            await streamWriter.WriteAsync(json);
            streamWriter.Close();
            fileStream.Close();
        }
        
    }
}
