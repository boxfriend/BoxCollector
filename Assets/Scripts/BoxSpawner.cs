using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Boxfriend.Utils;
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
        

        private void Awake ()
        {
            _boxPool = new ObjectPool<Box>(CreateBox,GetFromPool,ReleaseFromPool);
        }
        
        private Box CreateBox ()
        {
            var box = Instantiate(_boxPrefab);
            box.Spawner = this;
            return box;
        }

        private void Start ()
        {
            SpawnBoxes();
        }

        private void GetFromPool (Box box)
        {
            box.Construct((Box.BoxType)Random.Range(0,3));
            box.transform.position = new Vector2(Random.Range(-6,6), transform.position.y);
        }
        private void ReleaseFromPool (Box box)
        {
            box.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            box.GetComponent<Renderer>().enabled = false;
            box.GetComponent<Collider2D>().enabled = false;
            box.transform.position = transform.position;
        }

        public void Release (Box box) => _boxPool.Release(box);

        private async Task SpawnBoxes ()
        {
            var delay = _delay;
            while (true)
            {
                _boxPool.Get();
                await Task.Delay(delay);
                delay -= 5;
            }
        }
    }
}
