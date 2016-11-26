namespace PrimusGE.Graphics {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System.Runtime.InteropServices;

using Math;

/*-------------------------------------
 * STRUCTS
 *-----------------------------------*/

[StructLayout(LayoutKind.Sequential)]
public struct Vertex {
    /*-------------------------------------
     * PUBLIC FIELDS
     *-----------------------------------*/

    public Vector4 Position;

    public Vector2 TexCoord;

}

}
