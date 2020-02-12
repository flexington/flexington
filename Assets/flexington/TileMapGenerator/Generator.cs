// using System.Collections.Generic;
// using System.Linq;
// using UnityEditor;
// using UnityEngine;

// namespace flexington.TileMapGenerator
// {
//     public class Generator : MonoBehaviour
//     {
//         private List<TileEntity> _tiles;
//         public List<TileEntity> Tiles
//         {
//             get
//             {
//                 if (_tiles == null) _tiles = new List<TileEntity>();
//                 return _tiles;
//             }
//             set { _tiles = value; }
//         }

//         private Vector2Int _mapSize;
//         public Vector2Int MapSize
//         {
//             get { return _mapSize; }
//             set { _mapSize = value; }
//         }

//         private TileComponent[,] _map;

//         public void GenerateMap()
//         {
//             _map = new TileComponent[_mapSize.x, _mapSize.y];

//             for (int x = 0; x < _mapSize.x; x++)
//             {
//                 for (int y = 0; y < _mapSize.y; y++)
//                 {
//                     GameObject prefab = _tiles.Single(t => t.StartActivation <= 0 && t.EndActivation >= 0).Prefab;
//                     Vector3 position = new Vector3(-_mapSize.x / 2 + x + 0.5f, 0, -_mapSize.y / 2 + y + 0.5f);
//                     GameObject go = Instantiate(prefab, position, prefab.transform.rotation);

//                     TileComponent tile = go.GetComponent<TileComponent>();
//                     tile.Index = new Vector2Int(x, y);

//                     _map[x, y] = tile;

//                     go.transform.parent = this.transform;

//                 }
//             }
//         }

//         public void ResetMap()
//         {
//             for (int x = 0; x < _mapSize.x; x++)
//             {
//                 for (int y = 0; y < _mapSize.y; y++)
//                 {
//                     TileComponent tile = _map[x, y];
//                     DestroyImmediate(tile.gameObject);
//                 }
//             }

//             _map = null;
//         }
//     }
// }