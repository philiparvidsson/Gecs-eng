namespace PrimusGE.Math {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Runtime.InteropServices;

/*-------------------------------------
 * STRUCTS
 *-----------------------------------*/

[StructLayout(LayoutKind.Sequential)]
public struct Matrix2x2 {
    /*-------------------------------------
     * PUBLIC FIELDS
     *-----------------------------------*/

    public float M11;
    public float M12;
    public float M21;
    public float M22;

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public Matrix2x2(float m11, float m12,
                     float m21, float m22)
    {
        M11 = m11;
        M12 = m12;
        M21 = m21;
        M22 = m22;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public static Matrix2x2 Identity() {
        return new Matrix2x2(1.0f, 0.0f,
                             0.0f, 1.0f);
    }

    public static Matrix2x2 RotateZ(float a) {
        var cosa = (float)Math.Cos(a);
        var sina = (float)Math.Sin(a);

        var m = new Matrix2x2(cosa, -sina,
                              sina,  cosa);

        return m;
    }

    public static Matrix2x2 Scale(float x, float y) {
        var m = new Matrix2x2(   x, 0.0f,
                              0.0f,    y);

        return m;
    }

    public void Transpose() {
        var m11 = M11;
        var m12 = M21;
        var m21 = M12;
        var m22 = M22;

        M11 = m11;
        M12 = m12;
        M21 = m21;
        M22 = m22;
    }

    /*-------------------------------------
     * OPERATORS
     *-----------------------------------*/

    public static Matrix2x2 operator *(Matrix2x2 a, Matrix2x2 b) {
        var m11 = a.M11*b.M11 + a.M12*b.M21;
        var m12 = a.M11*b.M12 + a.M12*b.M22;
        var m21 = a.M21*b.M11 + a.M22*b.M21;
        var m22 = a.M21*b.M12 + a.M22*b.M22;

        return new Matrix2x2(m11, m12, m21, m22);
    }

    public static Vector2 operator *(Matrix2x2 m, Vector2 v) {
        var x = m.M11*v.X + m.M12*v.Y;
        var y = m.M21*v.X + m.M22*v.Y;

        return new Vector2(x, y);
    }
}

}
