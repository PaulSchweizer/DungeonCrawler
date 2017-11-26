using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Utility
{

    public struct Vector
    {
        public float X;
        public float Y;

        public Vector(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector(float[] vector)
        {
            X = vector[0];
            Y = vector[1];
        }

        public Vector(Vector vector)
        {
            X = vector.X;
            Y = vector.Y;
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

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.X - b.X, a.Y - b.Y);
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.X + b.X, a.Y + b.Y);
        }

        public static Vector operator /(Vector vector, float divisor)
        {
            return new Vector(vector.X / divisor, vector.Y / divisor);
        }

        public static Vector operator *(Vector vector, float multiplier)
        {
            return new Vector(vector.X * multiplier, vector.Y * multiplier);
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector other)
            {
                return other.X == X && other.Y == Y;
            }
            return false;
        }

        public static bool operator ==(Vector a, Vector b)
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

        public static bool operator !=(Vector a, Vector b)
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
        public Vector Position;
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

        public Transform(Vector position, float rotation)
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

        public float[] Map(Vector point)
        {
            return Map(point.X, point.Y);
        }

        public override string ToString()
        {
            return string.Format("{0}|{1}", Position.ToString(), Rotation);
        }
    }
}
