using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Utility
{

    public struct GridPoint
    {
        public int X;
        public int Y;

        public GridPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public GridPoint(int[] point)
        {
            X = point[0];
            Y = point[1];
        }

        public override bool Equals(object obj)
        {
            if (obj is GridPoint)
            {
                GridPoint other = (GridPoint)obj;
                return other.X == X && other.Y == Y;
            }
            return false;
        }

        public static bool operator ==(GridPoint a, GridPoint b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(GridPoint a, GridPoint b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("({0}/{1})", X, Y);
        }
    }

    public class Transform
    {
        public GridPoint Position = new GridPoint(0, 0);
        public float Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = (float)(Math.Floor(((value * 4) / Math.PI)) * Math.PI * 0.25);
            }
        }
        private float _rotation;

        public Transform(int x, int y, float rotation)
        {
            Position.X = x;
            Position.Y = y;
            Rotation = rotation;
        }

        public Transform(GridPoint position, float rotation)
        {
            Position.X = position.X;
            Position.Y = position.Y;
            Rotation = rotation;
        }

        public Transform(Transform transform)
        {
            Position.X = transform.Position.X;
            Position.Y = transform.Position.Y;
            Rotation = transform.Rotation;
        }

        public int[] Map(int x, int y)
        {
            double new_x = ((x * Math.Cos(Rotation) + y * Math.Sin(Rotation)));
            double new_y = ((- x * Math.Sin(Rotation) + y * Math.Cos(Rotation)));

            int mag = x;
            if (y > x)
            {
                mag = y;
            }

            new_x = (new_x / Math.Sqrt(new_x * new_x + new_y * new_y)) * mag;
            new_y = (new_y / Math.Sqrt(new_x * new_x + new_y * new_y)) * mag;

            return new int[] {
                Position.X + (int)(Math.Ceiling(Math.Abs(Math.Round(new_x, 3))) * Math.Sign(new_x)),
                Position.Y + (int)(Math.Ceiling(Math.Abs(Math.Round(new_y, 3))) * Math.Sign(new_y))
            };
        }

        public int[] Map(int[] point)
        {
            return Map(point[0], point[1]);
        }

        public override string ToString()
        {
            return string.Format("{0}|{1}", Position.ToString(), Rotation);
        }
    }
}
