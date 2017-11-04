using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Utility
{

    public struct Point
    {
        public float X;
        public float Y;

        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Point(float[] point)
        {
            X = point[0];
            Y = point[1];
        }

        public Point(Point point)
        {
            X = point.X;
            Y = point.Y;
        }

        public void Normalize()
        {
            var magnitude = Magnitude();
            if (magnitude > 0)
            {
                X = X / magnitude;
                Y = Y / magnitude;
            }
        }

        public float Magnitude()
        {
            return (float)Math.Sqrt(X * X + Y * Y);
        }

        public static Point operator -(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }

        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        public static Point operator /(Point point, float divisor)
        {
            return new Point(point.X / divisor, point.Y / divisor);
        }

        public static Point operator *(Point point, float multiplier)
        {
            return new Point(point.X * multiplier, point.Y * multiplier);
        }

        public override bool Equals(object obj)
        {
            if (obj is Point other)
            {
                return other.X == X && other.Y == Y;
            }
            return false;
        }

        public static bool operator ==(Point a, Point b)
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

        public static bool operator !=(Point a, Point b)
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
        public Point Position;
        public float Rotation;

        // Expects Degrees
        public static float[] RotateVector(float x, float y, float degrees, bool clockwise = true)
        {
            float[] result = new float[2];
            if (clockwise)
            {
                result[0] = (float)(x * Math.Cos(degrees) - y * Math.Sin(degrees));
                result[1] = (float)(x * Math.Sin(degrees) + y * Math.Cos(degrees));
            }
            else
            {
                result[0] = (float)(x * Math.Cos(degrees) + y * Math.Sin(degrees));
                result[1] = (float)(-x * Math.Sin(degrees) + y * Math.Cos(degrees));
            }
            return result;
        }

        // Returns Radians
        public static float AngleBetween(float[] fromVector, float[] toVector)
        {
            float dot = fromVector[0] * toVector[0] + fromVector[1] * toVector[1];
            float mags = (float)Math.Sqrt(fromVector[0] * fromVector[0] + fromVector[1] * fromVector[1]) * 
                (float)Math.Sqrt(toVector[0] * toVector[0] + toVector[1] * toVector[1]);
            if(mags == 0)
            {
                return 0;
            }
            return (float)Math.Acos(dot / mags);
        }

        public Transform(float x, float y, float rotation)
        {
            Position.X = x;
            Position.Y = y;
            Rotation = rotation;
        }

        public Transform(Point position, float rotation)
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

        public float[] Map(float x, float y)
        {

            double new_x = ((x * Math.Cos(Rotation) + y * Math.Sin(Rotation)));
            double new_y = ((- x * Math.Sin(Rotation) + y * Math.Cos(Rotation)));

            float mag = x;
            if (y > x)
            {
                mag = y;
            }

            new_x = (new_x / Math.Sqrt(new_x * new_x + new_y * new_y)) * mag;
            new_y = (new_y / Math.Sqrt(new_x * new_x + new_y * new_y)) * mag;

            if (double.IsNaN(new_x))
            {
                new_x = 0;
            }
            if (double.IsNaN(new_y))
            {
                new_y = 0;
            }

            return new float[] {
                Position.X + (float)new_x, // (float)(Math.Ceiling(Math.Abs(Math.Round(new_x, 3))) * Math.Sign(new_x)),
                Position.Y + (float)new_y // (float)(Math.Ceiling(Math.Abs(Math.Round(new_y, 3))) * Math.Sign(new_y))
            };
        }

        public float[] Map(float[] point)
        {
            return Map(point[0], point[1]);
        }

        public float[] Map(Point point)
        {
            return Map(point.X, point.Y);
        }

        public override string ToString()
        {
            return string.Format("{0}|{1}", Position.ToString(), Rotation);
        }
    }
}
