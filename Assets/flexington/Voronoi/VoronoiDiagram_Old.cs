using UnityEngine;

namespace flexington.Voronoi
{
    public class VoronoiDiagram_Old : MonoBehaviour
    {
        [SerializeField] private Vector2Int _size;
        [SerializeField] private int _regions;

        private void Start()
        {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            Sprite sprite = Sprite.Create(GetDiagram(), new Rect(0, 0, _size.x, _size.y), Vector2.one * 0.5f);
            renderer.sprite = sprite;
        }

        public Texture2D GetDiagram()
        {
            Vector2Int[] centroids = new Vector2Int[_regions];
            Color[] regions = new Color[_regions];

            for (int i = 0; i < _regions; i++)
            {
                centroids[i] = new Vector2Int(Random.Range(0, _size.x), Random.Range(0, _size.y));
                regions[i] = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
            }

            Color[] pixelColors = new Color[_size.x * _size.y];

            for (int x = 0; x < _size.x; x++)
            {
                for (int y = 0; y < _size.y; y++)
                {
                    int index = x * _size.x + y;
                    pixelColors[index] = regions[GetClosestCentroidIndex(new Vector2Int(x, y), centroids)];
                }
            }

            return GetImageFromColorArray(pixelColors);
        }

        public int GetClosestCentroidIndex(Vector2Int pixelPosition, Vector2Int[] centroids)
        {
            float smallestDistance = float.MaxValue;
            int index = 0;

            for (int i = 0; i < centroids.Length; i++)
            {
                float distance = Vector2.Distance(pixelPosition, centroids[i]);
                if (distance < smallestDistance)
                {
                    smallestDistance = distance;
                    index = i;
                }
            }

            return index;
        }

        public Texture2D GetImageFromColorArray(Color[] colors)
        {
            Texture2D texture = new Texture2D(_size.x, _size.y);
            texture.filterMode = FilterMode.Point;
            texture.SetPixels(colors);
            texture.Apply();
            return texture;
        }
    }
}