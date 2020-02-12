using System;
using System.Linq;
using System.Collections.Generic;
using flexington.Tools;
using UnityEngine;

namespace flexington.Voronoi
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class VoronoiComponent : MonoBehaviour
    {
        /// <summary>
        /// The Size of the Sprite
        /// </summary>
        [SerializeField] private Vector2Int _size;

        /// <summary>
        /// Number of different regions
        /// </summary>
        [SerializeField] private int _numberOfRegions;

        /// <summary>
        /// The seed for the random number generator
        /// </summary>
        [SerializeField] private string _seed;

        /// <summary>
        /// List of all Voronoi Regions
        /// </summary>
        private List<VoronoiRegion> _regions;

        /// <summary>
        /// Field to hold the SpriteRenderer
        /// </summary>
        private SpriteRenderer _renderer;

        /// <summary>
        /// Field to hold the Texture
        /// </summary>
        private Texture2D _texture;

        /// <summary>
        /// Field to hold the Sprite
        /// </summary>
        private Sprite _sprite;

        /// <summary>
        /// Reference to the VoronoiDiagram class
        /// </summary>
        private VoronoiDiagram _voronoi;

        /// <summary>
        /// Instantiate all components
        /// </summary>
        private void Awake()
        {
            if (_renderer == null) _renderer = GetComponent<SpriteRenderer>();
            if (_texture == null) _texture = new Texture2D(_size.x, _size.y);
            if (_renderer.sprite == null) _renderer.sprite = Sprite.Create(_texture, new Rect(0, 0, _size.x, _size.y), Vector2.one * 0.5f);

            _voronoi = new VoronoiDiagram(_numberOfRegions, _size, _seed);
        }

        /// <summary>
        /// Simulate expansion of regions 
        /// </summary>
        public void Start()
        {
            if (_voronoi == null) Awake();
            _voronoi.Simulate();
            _texture = _voronoi.GenerateTexture();
            ApplyTexture();
        }

        /// <summary>
        /// Apply the created texture to the SpriteRenderer
        /// </summary>
        public void ApplyTexture()
        {
            if (_renderer == null) _renderer = GetComponent<SpriteRenderer>();
            _sprite = Sprite.Create(_texture, new Rect(Vector2.zero, _size), Vector2.one * .5f);
            _renderer.sprite = _sprite;
        }

        /// <summary>
        /// Reset everything for a fresh restart
        /// </summary>
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