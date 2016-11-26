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
public struct Matrix3x3 {
    /*-------------------------------------
     * PUBLIC FIELDS
     *-----------------------------------*/

    public float M11;
    public float M12;
    public float M13;
    public float M21;
    public float M22;
    public float M23;
    public float M31;
    public float M32;
    public float M33;

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public Matrix3x3(float m11, float m12, float m13,
                     float m21, float m22, float m23,
                     float m31, float m32, float m33)
    {
        M11 = m11;
        M12 = m12;
        M13 = m13;
        M21 = m21;
        M22 = m22;
        M23 = m23;
        M31 = m31;
        M32 = m32;
        M33 = m33;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public static Matrix3x3 Identity() {
        return new Matrix3x3(1.0f, 0.0f, 0.0f,
                             0.0f, 1.0f, 0.0f,
                             0.0f, 0.0f, 1.0f);
    }

    public static Matrix3x3 RotateX(float a) {
        var cosa = (float)Math.Cos(a);
        var sina = (float)Math.Sin(a);

        var m = new Matrix3x3(1.0f, 0.0f, 0.0f,
                              0.0f, cosa, -sina,
                              0.0f, sina,  cosa);

        return m;
    }

    public static Matrix3x3 RotateY(float a) {
        var cosa = (float)Math.Cos(a);
        var sina = (float)Math.Sin(a);

        var m = new Matrix3x3(cosa , 0.0f, sina,
                              0.0f , 1.0f, 0.0f,
                             -sina, 0.0f, cosa);

        return m;
    }

    public static Matrix3x3 RotateZ(float a) {
        var cosa = (float)Math.Cos(a);
        var sina = (float)Math.Sin(a);

        var m = new Matrix3x3(cosa, -sina, 0.0f,
                              sina,  cosa, 0.0f,
                              0.0f,  0.0f, 1.0f);

        return m;
    }

    public static Matrix3x3 Scale(float x, float y) {
        var m = new Matrix3x3(   x, 0.0f, 0.0f,
                              0.0f,    y, 0.0f,
                              0.0f, 0.0f, 1.0f);

        return m;
    }

    public static Matrix3x3 Translate(float x, float y) {
        var m = new Matrix3x3(1.0f, 0.0f,    x,
                              0.0f, 1.0f,    y,
                              0.0f, 0.0f, 1.0f);

        return m;
    }

    public void Transpose() {
        var m11 = M11;
        var m12 = M21;
        var m13 = M31;
        var m21 = M12;
        var m22 = M22;
        var m23 = M32;
        var m31 = M13;
        var m32 = M23;
        var m33 = M33;

        M11 = m11;
        M12 = m12;
        M13 = m13;
        M21 = m21;
        M22 = m22;
        M23 = m23;
        M31 = m31;
        M32 = m32;
        M33 = m33;
    }

    /*-------------------------------------
     * OPERATORS
     *-----------------------------------*/

    public static Matrix3x3 operator *(Matrix3x3 a, Matrix3x3 b) {
        var m11 = a.M11*b.M11 + a.M12*b.M21 + a.M13*b.M31;
        var m12 = a.M11*b.M12 + a.M12*b.M22 + a.M13*b.M32;
        var m13 = a.M11*b.M13 + a.M12*b.M23 + a.M13*b.M33;
        var m21 = a.M21*b.M11 + a.M22*b.M21 + a.M23*b.M31;
        var m22 = a.M21*b.M12 + a.M22*b.M22 + a.M23*b.M32;
        var m23 = a.M21*b.M13 + a.M22*b.M23 + a.M23*b.M33;
        var m31 = a.M31*b.M11 + a.M32*b.M21 + a.M33*b.M31;
        var m32 = a.M31*b.M12 + a.M32*b.M22 + a.M33*b.M32;
        var m33 = a.M31*b.M13 + a.M32*b.M23 + a.M33*b.M33;

        return new Matrix3x3(m11, m12, m13, m21, m22, m23, m31, m32, m33);
    }

    public static Vector3 operator *(Matrix3x3 m, Vector3 v) {
        var x = m.M11*v.X + m.M12*v.Y + m.M13*v.Z;
        var y = m.M21*v.X + m.M22*v.Y + m.M23*v.Z;
        var z = m.M31*v.X + m.M32*v.Y + m.M33*v.Z;

        return new Vector3(x, y, z);
    }
}

}
