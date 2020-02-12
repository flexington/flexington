using UnityEngine;

namespace flexington.Voronoi2D
{
    public class VoronoiRegion
    {
        /// <summary>
        /// The rectangle that defined the centroid
        /// </summary>
        public Rect Rect
        {
            get { return _rect; }
            set { _rect = value; }
        }
        private Rect _rect;

        /// <summary>
        /// The color of the cetroid
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Defines if the centroid can expand in positive Y direction
        /// </summary>
        public bool GrowTop { get; set; } = true;

        /// <summary>
        /// Defines if the centroid can expand in negative Y direction
        /// </summary>
        public bool GrowBottom { get; set; } = true;

        /// <summary>
        /// Defines if the centroid can expand in negative X direction
        /// </summary>
        public bool GrowLeft { get; set; } = true;

        /// <summary>
        /// Defines if the centroid can expand in positive X direction
        /// </summary>
        public bool GrowRight { get; set; } = true;

        /// <summary>
        /// Indicates if this region can grow or not.
        /// </summary>
        private bool _canGrow = true;
        public bool CanGrow
        {
            get { return _canGrow; }
        }

        /// <summary>
        /// Expands this region by the [[absoluteGrowth]] amount.
        /// The [[absoluteGrowth]] will be added to all four sides as long as this side can grow. 
        /// </summary>
        /// <param name="absoluteGrowth">The amount the region should grow in each direction.</param>
        /// <param name="rect">The area in which the region can grow.</param>
        public void GrowUniform(float absoluteGrowth)
        {
            if (!CanGrow) return;
            if (!GrowTop && !GrowBottom && !GrowLeft && !GrowRight)
            {
                _canGrow = false;
                return;
            }

            if (GrowTop) _rect.xMax += absoluteGrowth;
            if (GrowBottom) _rect.xMin -= absoluteGrowth;

            if (GrowLeft) _rect.yMin -= absoluteGrowth;
            if (GrowRight) _rect.yMax += absoluteGrowth;
        }

        /// <summary>
        /// Verifies if the current Region overlaps with the other ragion.
        /// If one or more of the sides overlaps, the corresponding property 
        /// (GrowTop, GrowBottom, GrowLeft, GrowRight) will be set to false
        /// </summary>
        public void Overlaps(VoronoiRegion other)
        {
            // If regions don't overlap, leave
            if (!Rect.Overlaps(other.Rect)) return;
            // If rect can't grow on any side anymore, leave
            if (!GrowTop && !GrowBottom && !GrowLeft && !GrowRight) return;

            Vector2 topCenter = new Vector2(Rect.xMax, Rect.center.y);
            Vector2 bottomCenter = new Vector2(Rect.xMin, Rect.center.y);
            Vector2 leftCenter = new Vector2(Rect.center.x, Rect.yMin);
            Vector2 rightCenter = new Vector2(Rect.center.x, Rect.yMax);

            float topDistance = Vector2.Distance(topCenter, other.Rect.center);
            float bottomDistance = Vector2.Distance(bottomCenter, other.Rect.center);
            float leftDistance = Vector2.Distance(leftCenter, other.Rect.center);
            float rightDistance = Vector2.Distance(rightCenter, other.Rect.center);
            float minDistance = float.MaxValue;
            string side = string.Empty;

            if (topDistance < minDistance)
            {
                minDistance = topDistance;
                side = "top";
            }
            if (bottomDistance < minDistance)
            {
                minDistance = bottomDistance;
                side = "bottom";
            }
            if (leftDistance < minDistance)
            {
                minDistance = leftDistance;
                side = "left";
            }
            if (rightDistance < minDistance)
            {
                minDistance = rightDistance;
                side = "right";
            }

            if (side == "top")
            {
                GrowTop = false;
                _rect.xMax--;
            }
            if (side == "bottom")
            {
                GrowBottom = false;
                _rect.xMin++;
            }
            if (side == "left")
            {
                GrowLeft = false;
                _rect.yMin++;
            }
            if (side == "right")
            {
                GrowRight = false;
                _rect.yMax--;
            }

        }

        /// <summary>
        /// Verfifies if the current region overlaps with an outer border.
        /// If one or more of the sides overlap, the corresponding property
        /// (GrowTop, GrowBottom, GrowLeft, GrowRight) will be set to false
        /// </summary>
        public void Overlaps(Rect rect)
        {
            // Top
            if (_rect.xMax > rect.xMax)
            {
                GrowTop = false;
                _rect.xMax--;
            }

            //Bottom
            if (_rect.xMin < rect.xMin)
            {
                GrowBottom = false;
                _rect.xMin++;
            }

            // Left
            if (_rect.yMin < rect.yMin)
            {
                GrowLeft = false;
                _rect.yMin++;
            }
            if (_rect.yMax > rect.xMax)
            {
                GrowRight = false;
                _rect.yMax--;
            }
        }
    }
}