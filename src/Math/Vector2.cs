namespace GecsEng.Math {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Runtime.InteropServices;

/*-------------------------------------
 * STRUCTS
 *-----------------------------------*/

[StructLayout(LayoutKind.Sequential)]
public struct Vector2: IEquatable<Vector2> {
    /*-------------------------------------
     * PUBLIC FIELDS
     *-----------------------------------*/

    public float X;
    public float Y;

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public Vector2(float x=0.0f, float y=0.0f) {
        X = x;
        Y = y;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public float Dot(Vector2 v) {
        return X*v.X + Y*v.Y;
    }

    public override bool Equals(object obj) {
        if (!(obj is Vector2)) {
            return false;
        }

        var vector2 = (Vector2)obj;

        return vector2.X == X && vector2.Y == Y;
    }

    public bool Equals(Vector2 obj) {
        return obj.X == X && obj.Y == Y;
    }

    public float Length() {
        return (float)Math.Sqrt(X*X + Y*Y);
    }

    public void Normalize() {
        var r = (float)Math.Sqrt(X*X + Y*Y);
        if (r > 0.0f) {
            X /= r;
            Y /= r;
        }
    }

    public float PerpDot(Vector2 a) {
        return X*a.Y - Y*a.X;
    }

    public Vector2 Perp() {
        return new Vector2(-Y, X);
    }

    /*-------------------------------------
     * OPERATORS
     *-----------------------------------*/

    public static bool operator ==(Vector2 a, Vector2 b) {
        return a.X == b.X && a.Y == b.Y;
    }

    public static bool operator !=(Vector2 a, Vector2 b) {
        return !(a == b);
    }

    public static Vector2 operator -(Vector2 a, Vector2 b) {
        return new Vector2(a.X - b.X, a.Y - b.Y);
    }

    public static Vector2 operator +(Vector2 a, Vector2 b) {
        return new Vector2(a.X + b.X, a.Y + b.Y);
    }

    public static Vector2 operator *(Vector2 a, Vector2 b) {
        return new Vector2(a.X*b.X, a.Y*b.Y);
    }

    public static Vector2 operator *(float a, Vector2 b) {
        return new Vector2(a*b.X, a*b.Y);
    }

    public static Vector2 operator *(Vector2 a, float b) {
        return new Vector2(a.X*b, a.Y*b);
    }
}

}
