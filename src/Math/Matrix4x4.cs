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
public struct Matrix4x4 {
    /*-------------------------------------
     * PUBLIC FIELDS
     *-----------------------------------*/

    public float M11;
    public float M12;
    public float M13;
    public float M14;
    public float M21;
    public float M22;
    public float M23;
    public float M24;
    public float M31;
    public float M32;
    public float M33;
    public float M34;
    public float M41;
    public float M42;
    public float M43;
    public float M44;

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public Matrix4x4(float m11, float m12, float m13, float m14,
                     float m21, float m22, float m23, float m24,
                     float m31, float m32, float m33, float m34,
                     float m41, float m42, float m43, float m44)
    {
        M11 = m11;
        M12 = m12;
        M13 = m13;
        M14 = m14;
        M21 = m21;
        M22 = m22;
        M23 = m23;
        M24 = m24;
        M31 = m31;
        M32 = m32;
        M33 = m33;
        M34 = m34;
        M41 = m41;
        M42 = m42;
        M43 = m43;
        M44 = m44;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public static Matrix4x4 Identity() {
        return new Matrix4x4(1.0f, 0.0f, 0.0f, 0.0f,
                             0.0f, 1.0f, 0.0f, 0.0f,
                             0.0f, 0.0f, 1.0f, 0.0f,
                             0.0f, 0.0f, 0.0f, 1.0f);
    }

    public static Matrix4x4 RotateX(float a) {
        var cosa = (float)Math.Cos(a);
        var sina = (float)Math.Sin(a);

        var m = new Matrix4x4(1.0f, 0.0f, 0.0f , 0.0f,
                              0.0f, cosa, -sina, 0.0f,
                              0.0f, sina, cosa , 0.0f,
                              0.0f, 0.0f, 0.0f , 1.0f);

        return m;
    }

    public static Matrix4x4 RotateY(float a) {
        var cosa = (float)Math.Cos(a);
        var sina = (float)Math.Sin(a);

        var m = new Matrix4x4(cosa , 0.0f, sina, 0.0f,
                              0.0f , 1.0f, 0.0f, 0.0f,
                              -sina, 0.0f, cosa, 0.0f,
                              0.0f , 0.0f, 0.0f, 1.0f);

        return m;
    }

    public static Matrix4x4 RotateZ(float a) {
        var cosa = (float)Math.Cos(a);
        var sina = (float)Math.Sin(a);

        var m = new Matrix4x4(cosa, -sina, 0.0f, 0.0f,
                              sina, cosa , 0.0f, 0.0f,
                              0.0f, 0.0f , 1.0f, 0.0f,
                              0.0f, 0.0f , 0.0f, 1.0f);

        return m;
    }

    public static Matrix4x4 Scale(float x, float y, float z) {
        var m = new Matrix4x4(x   , 0.0f, 0.0f, 0.0f,
                              0.0f, y   , 0.0f, 0.0f,
                              0.0f, 0.0f, z   , 0.0f,
                              0.0f, 0.0f, 0.0f, 1.0f);

        return m;
    }

    public static Matrix4x4 Translate(float x, float y, float z) {
        var m = new Matrix4x4(1.0f, 0.0f, 0.0f, x,
                              0.0f, 1.0f, 0.0f, y,
                              0.0f, 0.0f, 1.0f, z,
                              0.0f, 0.0f, 0.0f, 1.0f);

        return m;
    }

    public void Transpose() {
        var m11 = M11;
        var m12 = M21;
        var m13 = M31;
        var m14 = M41;
        var m21 = M12;
        var m22 = M22;
        var m23 = M32;
        var m24 = M42;
        var m31 = M13;
        var m32 = M23;
        var m33 = M33;
        var m34 = M43;
        var m41 = M14;
        var m42 = M24;
        var m43 = M34;
        var m44 = M44;

        M11 = m11;
        M12 = m12;
        M13 = m13;
        M14 = m14;
        M21 = m21;
        M22 = m22;
        M23 = m23;
        M24 = m24;
        M31 = m31;
        M32 = m32;
        M33 = m33;
        M34 = m34;
        M41 = m41;
        M42 = m42;
        M43 = m43;
        M44 = m44;
    }

    /*-------------------------------------
     * OPERATORS
     *-----------------------------------*/

    public static Matrix4x4 operator *(Matrix4x4 a, Matrix4x4 b) {
        var m11 = a.M11*b.M11 + a.M12*b.M21 + a.M13*b.M31 + a.M14*b.M41;
        var m12 = a.M11*b.M12 + a.M12*b.M22 + a.M13*b.M32 + a.M14*b.M42;
        var m13 = a.M11*b.M13 + a.M12*b.M23 + a.M13*b.M33 + a.M14*b.M43;
        var m14 = a.M11*b.M14 + a.M12*b.M24 + a.M13*b.M34 + a.M14*b.M44;
        var m21 = a.M21*b.M11 + a.M22*b.M21 + a.M23*b.M31 + a.M24*b.M41;
        var m22 = a.M21*b.M12 + a.M22*b.M22 + a.M23*b.M32 + a.M24*b.M42;
        var m23 = a.M21*b.M13 + a.M22*b.M23 + a.M23*b.M33 + a.M24*b.M43;
        var m24 = a.M21*b.M14 + a.M22*b.M24 + a.M23*b.M34 + a.M24*b.M44;
        var m31 = a.M31*b.M11 + a.M32*b.M21 + a.M33*b.M31 + a.M34*b.M41;
        var m32 = a.M31*b.M12 + a.M32*b.M22 + a.M33*b.M32 + a.M34*b.M42;
        var m33 = a.M31*b.M13 + a.M32*b.M23 + a.M33*b.M33 + a.M34*b.M43;
        var m34 = a.M31*b.M14 + a.M32*b.M24 + a.M33*b.M34 + a.M34*b.M44;
        var m41 = a.M41*b.M11 + a.M42*b.M21 + a.M43*b.M31 + a.M44*b.M41;
        var m42 = a.M41*b.M12 + a.M42*b.M22 + a.M43*b.M32 + a.M44*b.M42;
        var m43 = a.M41*b.M13 + a.M42*b.M23 + a.M43*b.M33 + a.M44*b.M43;
        var m44 = a.M41*b.M14 + a.M42*b.M24 + a.M43*b.M34 + a.M44*b.M44;

        return new Matrix4x4(m11, m12, m13, m14,
                             m21, m22, m23, m24,
                             m31, m32, m33, m34,
                             m41, m42, m43, m44);
    }

    public static Vector4 operator *(Matrix4x4 m, Vector4 v) {
        var x = m.M11*v.X + m.M12*v.Y + m.M13*v.Z + m.M14*v.W;
        var y = m.M21*v.X + m.M22*v.Y + m.M23*v.Z + m.M14*v.W;
        var z = m.M31*v.X + m.M32*v.Y + m.M33*v.Z + m.M14*v.W;

        return new Vector4(x, y, z);
    }
}

}
