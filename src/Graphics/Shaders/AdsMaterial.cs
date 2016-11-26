namespace GecsEng.Graphics.Shaders {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System.Runtime.InteropServices;

using Core;
using Math;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

[StructLayout(LayoutKind.Sequential)]
public struct AdsMaterial {
    /*-------------------------------------
     * PUBLIC FIELDS
     *-----------------------------------*/
    
    public Vector4 Ambient;
    public Vector4 Diffuse;
    public Vector4 Specular;
    public bool UseTexture;
}

}
