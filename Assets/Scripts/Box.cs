using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Boxfriend
{
    public class Box : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Rigidbody2D _rb2d;
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private float _speed;
        [SerializeField] private BoxType _type;
        [SerializeField] private Color _defaultColor;


        public static event Action<int> OnBoxClick;
        
        public BoxSpawner Spawner { get; set; }

        public void Construct (BoxType type)
        {
            _type = type;
           
            _rb2d.velocity = _type == BoxType.BETTER ? (Vector2.down * _speed * 1.5f) : (Vector2.down * _speed);

            _renderer.enabled = true;
            _renderer.color = _type == BoxType.DEATH ? Color.black : _defaultColor;
            GetComponent<Collider2D>().enabled = true;
        }
        
        public void OnPointerClick (PointerEventData eventData)
        {
            GimmePoints();
            Spawner.Release(this);
        }

        private void GimmePoints ()
        {
            var points = _type == BoxType.BETTER ? 5 : 1;
            points = _type == BoxType.DEATH ? -1 : points;
            OnBoxClick?.Invoke(points);
        }

        private void Reset ()
        {
            _rb2d = GetComponent<Rigidbody2D>();
            _renderer = GetComponent<SpriteRenderer>();
            _defaultColor = _renderer.color;
            _speed = 1.5f;
        }
        
        public enum BoxType
        {
            NORMAL,
            BETTER,
            DEATH
        }
    }

    
}
