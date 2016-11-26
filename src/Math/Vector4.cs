namespace GecsEng.Math {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System.Runtime.InteropServices;

/*-------------------------------------
 * STRUCTS
 *-----------------------------------*/

[StructLayout(LayoutKind.Sequential)]
public struct Vector4 {
        /*-------------------------------------
         * PUBLIC FIELDS
         *-----------------------------------*/

        public float X;
    public float Y;
    public float Z;
    public float W;

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public Vector4(float x=0.0f, float y=0.0f, float z=0.0f, float w=1.0f) {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public Vector4(Vector2 xy, float z=0.0f, float w=1.0f) {
        X = xy.X;
        Y = xy.Y;
        Z = z;
        W = w;
    }

    public Vector4(Vector3 xyz, float w=1.0f) {
        X = xyz.X;
        Y = xyz.Y;
        Z = xyz.Z;
        W = w;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public float Dot(Vector4 v) {
        return X*v.X + Y*v.Y + Z*v.Z + W*v.W;
    }
}

}
