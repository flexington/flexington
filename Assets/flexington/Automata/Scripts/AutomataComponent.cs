using System;
using System.Linq;
using UnityEngine;

namespace flexington.Automata
{
    public class AutomataComponent : MonoBehaviour
    {
        [SerializeField] private Vector2Int _mapSize;
        public Vector2Int MapSize
        {
            get { return _mapSize; }
            set { _mapSize = value; }
        }

        [SerializeField, Range(0, 100)] private int _threshold;
        public int Threshold
        {
            get { return _threshold; }
            set { _threshold = value; }
        }

        [SerializeField] private int _iterations;
        public int Iterations
        {
            get { return _iterations; }
            set { _iterations = value; }
        }

        [SerializeField] private string _seed;
        public string Seed
        {
            get { return _seed; }
            set { _seed = value; }
        }

        [SerializeField] private GameObject _deadPrefab;
        public GameObject DeadPrefab
        {
            get { return _deadPrefab; }
            set { _deadPrefab = value; }
        }

        [SerializeField] private GameObject _alivePrefab;
        public GameObject AlivePrefab
        {
            get { return _alivePrefab; }
            set { _alivePrefab = value; }
        }

        private int[,] _currentGeneration;
        private int[,] _nextGeneration;
        private GameObject[,] _tiles;

        public void Generate()
        {
            Reset();

            if (_deadPrefab == null) throw new NullReferenceException("Dead Prefab must be set.");
            if (_alivePrefab == null) throw new NullReferenceException("Alive Prefab must be set.");

            _currentGeneration = Automata.Generate(_mapSize, _threshold, _seed);

            for (int x = 0; x < _currentGeneration.GetUpperBound(0); x++)
            {
                for (int y = 0; y < _currentGeneration.GetUpperBound(1); y++)
                {
                    Vector3 position = new Vector3(-_mapSize.x / 2 + x + 0.5f, 0, -_mapSize.y / 2 + y + 0.5f);
                    GameObject tile;
                    if (_currentGeneration[x, y] == 1) tile = Instantiate(_alivePrefab, position, _alivePrefab.transform.rotation);
                    else tile = Instantiate(_deadPrefab, position, _deadPrefab.transform.rotation);
                    tile.transform.parent = this.transform;
                    _tiles[x, y] = tile;
                }
            }
        }

        public void GenerateAndSimulate()
        {
            Generate();
            for (int i = 0; i < _iterations; i++)
            {
                NextGeneration();
            }
        }

        public void NextGeneration()
        {
            _nextGeneration = Automata.Simulate(_currentGeneration, 1);

            for (int x = 0; x < _currentGeneration.GetUpperBound(0); x++)
            {
                for (int y = 0; y < _currentGeneration.GetUpperBound(1); y++)
                {
                    if (_currentGeneration[x, y] != _nextGeneration[x, y])
                    {
                        _currentGeneration[x, y] = _nextGeneration[x, y];
                        DestroyImmediate(_tiles[x, y]);
                        Vector3 position = new Vector3(-_mapSize.x / 2 + x + 0.5f, 0, -_mapSize.y / 2 + y + 0.5f);
                        GameObject tile;
                        if (_currentGeneration[x, y] == 1) tile = Instantiate(_alivePrefab, position, _alivePrefab.transform.rotation);
                        else tile = Instantiate(_deadPrefab, position, _deadPrefab.transform.rotation);
                        tile.transform.parent = this.transform;
                        _tiles[x, y] = tile;
                    }
                }
            }
        }

        public void Reset()
        {
            if (_tiles != null)
            {
                for (int x = 0; x < _tiles.GetUpperBound(0); x++)
                {
                    for (int y = 0; y < _tiles.GetUpperBound(1); y++)
                    {
                        DestroyImmediate(_tiles[x, y]);
                    }
                }
            }


            _tiles = new GameObject[_mapSize.x, _mapSize.y];
            _currentGeneration = new int[_mapSize.x, _mapSize.y];
        }
    }
}