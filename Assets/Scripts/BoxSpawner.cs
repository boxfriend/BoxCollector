using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace Boxfriend
{
    public class BoxSpawner : MonoBehaviour
    {
        private ObjectPool<Box> _boxPool;

        [SerializeField] private Box _boxPrefab;
        
        [SerializeField] private int _delay = 2000;

        private bool canSpawn = true;
        private readonly List<Box> _spawned = new();

        private void Awake ()
        {
            _boxPool = new ObjectPool<Box>(CreateBox,GetFromPool,ReleaseFromPool);
        }

        private void OnEnable ()
        {
            GameDataManager.OnGameOver += GameOver;
        }
        private void OnDisable ()
        {
            GameDataManager.OnGameOver -= GameOver;
        }

        private async void GameOver ()
        {
            canSpawn = false;
            
            await Task.Delay(20); //Waits 0.02 seconds (same as Fixed Delta Time)
            
            for (var i = _spawned.Count - 1; i >= 0; i--)
                _boxPool.Release(_spawned[i]);
        }

        private Box CreateBox ()
        {
            var box = Instantiate(_boxPrefab);
            box.Spawner = this;
            return box;
        }

        private void Start ()
        {
            var spawning = SpawnBoxes();
        }

        private void GetFromPool (Box box)
        {
            box.Construct((Box.BoxType)Random.Range(0,3));
            box.transform.position = new Vector2(Random.Range(-6,6), transform.position.y);
            _spawned.Add(box);
        }
        private void ReleaseFromPool (Box box)
        {
            _spawned.Remove(box);
            box.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            box.GetComponent<Renderer>().enabled = false;
            box.GetComponent<Collider2D>().enabled = false;
            box.transform.position = transform.position;
        }

        public void Release (Box box) => _boxPool.Release(box);

        private async Task SpawnBoxes ()
        {
            var delay = _delay;
            while (canSpawn)
            {
                _boxPool.Get();
                await Task.Delay(delay);
                delay -= 5;
            }
        }
    }
}
