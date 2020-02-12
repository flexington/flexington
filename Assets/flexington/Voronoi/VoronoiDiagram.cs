using System;
using System.Linq;
using System.Collections.Generic;
using flexington.Tools;
using UnityEngine;

namespace flexington.Voronoi
{
    public class VoronoiDiagram
    {
        private Vector2Int _size;
        public Vector2Int Size
        {
            get { return _size; }
        }

        private string _seed;
        public string Seed
        {
            get { return _seed; }
        }

        private int _numberOfRegions;
        private List<VoronoiRegion> _regions;
        private System.Random _rng;
        Texture2D _texture;
        private Sprite _sprite;

        public VoronoiDiagram(int numberOfRegions, Sprite sprite, Vector2Int size = default, string seed = default)
        {
            _numberOfRegions = numberOfRegions;

            _sprite = sprite;

            if (size == default) size = new Vector2Int(_numberOfRegions * 15, _numberOfRegions * 15);
            _size = size;

            if (seed == null || seed == string.Empty) seed = DateTime.Now.ToString();
            _seed = seed;
            _rng = new System.Random(_seed.GetHashCode());

            _regions = new List<VoronoiRegion>();

            GenerateRegions();
        }

        private void GenerateRegions()
        {
            for (int i = 0; i < _numberOfRegions; i++)
            {
                _regions.Add(new VoronoiRegion
                {
                    Rect = new Rect(_rng.Next(0, _size.x - 1), _rng.Next(0, _size.y - 1), 1, 1),
                    Color = ColorUtil.GetRandomColor()
                });
            }
        }

        public void Simulate()
        {
            while (_regions.Count(r => r.CanGrow) > 0)
            {
                for (int i = 0; i < _regions.Count; i++)
                {
                    VoronoiRegion region = _regions[i];
                    region.GrowUniform(1f);
                    for (int j = 0; j < _regions.Count; j++)
                    {
                        if (i == j) continue;
                        region.Overlaps(_regions[j]);
                    }
                    region.Overlaps(_sprite.rect);
                }
            }
        }

        public Texture2D GenerateTexture()
        {
            Color[] pixelColors = new Color[_size.x * _size.y];
            _texture = new Texture2D(_size.x, _size.y);
            _texture.filterMode = FilterMode.Point;

            for (int i = 0; i < pixelColors.Length; i++)
            {
                pixelColors[i] = Color.white;
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
            return _texture;
        }
    }
}