namespace PrimusGE.Graphics {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

using Math;

/*-------------------------------------
 * STRUCTS
 *-----------------------------------*/

public struct Color {
    /*-------------------------------------
     * PUBLIC FIELDS
     *-----------------------------------*/

    public static readonly Color Black = new Color(0.0f, 0.0f, 0.0f, 1.0f);

    public static readonly Color White = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/
    
    public float A { get; set; }
    public float B { get; set; }
    public float G { get; set; }
    public float R { get; set; }


    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public Color(float r=0.0f, float g=0.0f, float b=0.0f, float a=1.0f) {
        R = Math.Min(Math.Max(0.0f, r), 1.0f);
        G = Math.Min(Math.Max(0.0f, g), 1.0f); ;
        B = Math.Min(Math.Max(0.0f, b), 1.0f); ;
        A = Math.Min(Math.Max(0.0f, a), 1.0f); ;
    }

    public Color(byte r, byte g, byte b, byte a)
        : this(r/255.0f, g/255.0f, b/255.0f, a/255.0f)
    {
    }

    public Color(uint value)
        : this((byte)((value>>16) & 0xff),
               (byte)((value>>8)  & 0xff),
               (byte) (value      & 0xff),
               (byte)((value>>24) & 0xff))
    {
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public static Color FromVector4(Vector4 v) {
        return new Color(v.X, v.Y, v.Z, v.W);
    }

    public int ToIntABGR() {
        byte r = (byte)(R*255.0f);
        byte g = (byte)(G*255.0f);
        byte b = (byte)(B*255.0f);
        byte a = (byte)(A*255.0f);

        return (a<<24)|(b<<16)|(g<<8)|r;
    }

    public int ToIntARGB() {
        byte r = (byte)(R*255.0f);
        byte g = (byte)(G*255.0f);
        byte b = (byte)(B*255.0f);
        byte a = (byte)(A*255.0f);

        return (a<<24)|(r<<16)|(g<<8)|b;
    }

    public int ToIntBGRA() {
        byte r = (byte)(R*255.0f);
        byte g = (byte)(G*255.0f);
        byte b = (byte)(B*255.0f);
        byte a = (byte)(A*255.0f);

        return (b<<24)|(g<<16)|(r<<8)|a;
    }

    public int ToIntRGBA() {
        byte r = (byte)(R*255.0f);
        byte g = (byte)(G*255.0f);
        byte b = (byte)(B*255.0f);
        byte a = (byte)(A*255.0f);

        return (r<<24)|(g<<16)|(b<<8)|a;
    }

    public Vector4 ToVector4() {
        return new Vector4(R, G, B, A);
    }

}

}
