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
        public Vector2Int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        [SerializeField] private int _numberOfRegions;
        public int NumberOfRegions
        {
            get { return _numberOfRegions; }
            set { _numberOfRegions = value; }
        }

        [SerializeField] private string _seed;
        public string Seed
        {
            get { return _seed; }
            set { _seed = value; }
        }

        private List<VoronoiRegion> _regions;
        public List<VoronoiRegion> Regions
        {
            get { return _regions; }
            set { _regions = value; }
        }

        private System.Random _rng;

        private SpriteRenderer _renderer;

        private Texture2D _texture;

        private Sprite _sprite;

        private void Start()
        {
            if (_renderer == null) _renderer = GetComponent<SpriteRenderer>();

            InitRegions();
            GenerateTexture();
            ApplyTexture();
        }

        public void InitRegions()
        {
            if (_seed == null || _seed == string.Empty) _seed = DateTime.Now.ToString();
            if (_regions == null || _regions.Count == 0) _regions = new List<VoronoiRegion>(_numberOfRegions);

            _rng = new System.Random(_seed.GetHashCode());
            for (int i = 0; i < _numberOfRegions; i++)
            {
                _regions.Add(new VoronoiRegion
                {
                    Rect = new Rect(_rng.Next(0, _size.x - 1), _rng.Next(0, _size.y - 1), 1, 1),
                    Color = GetRandomColor()
                });
            }
        }

        public void Grow()
        {
            for (int i = 0; i < _regions.Count; i++)
            {
                VoronoiRegion region = _regions[i];
                region.GrowUniform(1f);
                for (int j = 0; j < _regions.Count; j++)
                {
                    if (j == i) continue;
                    region.Overlaps(_regions[j]);
                }
                _regions[i].Overlaps(_sprite.rect);
            }

            int canGrow = _regions.Count(r => r.CanGrow);
            Debug.Log(canGrow);
        }

        public void GenerateTexture()
        {
            Color[] pixelColors = new Color[_size.x * _size.y];
            _texture = new Texture2D(_size.x, _size.y);
            _texture.filterMode = FilterMode.Point;

            for (int y = 0; y < _size.y; y++)
            {
                for (int x = 0; x < _size.x; x++)
                {
                    int index = x * _size.x + y;
                    pixelColors[index] = Color.white;
                }
            }

            for (int y = 0; y < _size.y; y++)
            {
                for (int x = 0; x < _size.x; x++)
                {
                    int index = x * _size.x + y;
                    for (int i = 0; i < _regions.Count; i++)
                    {
                        if (_regions[i].Rect.Contains(new Vector2(x, y)))
                        {
                            pixelColors[index] = _regions[i].Color;
                        }
                    }
                }
            }
            _texture.SetPixels(pixelColors);
            _texture.Apply();
        }

        public void ApplyTexture()
        {
            if (_renderer == null) _renderer = GetComponent<SpriteRenderer>();
            _sprite = Sprite.Create(_texture, new Rect(Vector2.zero, _size), Vector2.one * .5f);
            _renderer.sprite = _sprite;
        }

        private Color GetRandomColor()
        {
            return new Color(
                _rng.Next(0, 255) / 255f,
                _rng.Next(0, 255) / 255f,
                _rng.Next(0, 255) / 255f
            );
        }

        public void AddNewRegion(Vector2 position)
        {
            if (_renderer == null) _renderer = GetComponent<SpriteRenderer>();
            Rect rect = SpriteUtil.GetWorldRect(transform, _renderer.sprite);
            if (rect.Contains(position))
            {
                Vector2 pixelPosition = SpriteUtil.WorldToPixelCoordinates(position, transform, _renderer.sprite);
                _regions.Add(new VoronoiRegion
                {
                    Rect = new Rect(pixelPosition, Vector2.one),
                    Color = GetRandomColor()
                });
            }
            GenerateTexture();
            ApplyTexture();
        }

        public void Reset()
        {
            if (_renderer == null) _renderer = GetComponent<SpriteRenderer>();
            _size = new Vector2Int(100, 100);
            _renderer.sprite = null;
            _regions = null;
            _seed = string.Empty;
            _texture = null;
            _sprite = null;
        }
    }
}