using System;
using System.Linq;
using System.Collections.Generic;
using flexington.Tools;
using UnityEngine;

namespace flexington.Voronoi
{
    public class VoronoiComponent : MonoBehaviour
    {
        [SerializeField] private Vector2Int _size;


        [SerializeField] private int _numberOfRegions;


        [SerializeField] private string _seed;
        public string Seed
        {
            get { return _seed; }
            set { _seed = value; }
        }

        private List<VoronoiRegion> _regions;

        private SpriteRenderer _renderer;

        private Texture2D _texture;

        private Sprite _sprite;


        private VoronoiDiagram _voronoi;
        private void Awake()
        {
            if (_renderer == null) _renderer = GetComponent<SpriteRenderer>();
            if (_texture == null) _texture = new Texture2D(_size.x, _size.y);
            if (_renderer.sprite == null) _renderer.sprite = Sprite.Create(_texture, new Rect(0, 0, _size.x, _size.y), Vector2.one * 0.5f);

            _voronoi = new VoronoiDiagram(_numberOfRegions, _renderer.sprite, _size, _seed);
        }

        public void Start()
        {
            if (_voronoi == null) Awake();
            _voronoi.Simulate();
            _texture = _voronoi.GenerateTexture();
            ApplyTexture();

        }

        public void ApplyTexture()
        {
            if (_renderer == null) _renderer = GetComponent<SpriteRenderer>();
            _sprite = Sprite.Create(_texture, new Rect(Vector2.zero, _size), Vector2.one * .5f);
            _renderer.sprite = _sprite;
        }

        public void Reset()
        {
            if (_renderer == null) _renderer = GetComponent<SpriteRenderer>();
            _voronoi = null;
            _size = new Vector2Int(100, 100);
            _renderer.sprite = null;
            _regions = null;
            _seed = string.Empty;
            _texture = null;
            _sprite = null;
        }
    }
}