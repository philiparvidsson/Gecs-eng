namespace PrimusGE.Graphics.SharpDXImpl {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Runtime.InteropServices;

using D3D11 = SharpDX.Direct3D11;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

internal class SharpDXTriMesh: IDisposable, ITriMesh {
    /*-------------------------------------
     * PUBLIC FIELDS
     *-----------------------------------*/
    public SharpDXGraphicsMgr Graphics;

    public D3D11.Buffer IndexBuffer;

    public D3D11.Buffer VertexBuffer;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public int NumTris { get; private set; }

    public int NumVerts { get; private set; }

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public SharpDXTriMesh(SharpDXGraphicsMgr graphics,
                          D3D11.Buffer       indexBuffer,
                          D3D11.Buffer       vertexBuffer,
                          int                numTris,
                          int                numVerts)
    {
        Graphics     = graphics;
        IndexBuffer  = indexBuffer;
        VertexBuffer = vertexBuffer;

        NumTris  = numTris;
        NumVerts = numVerts;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /*-------------------------------------
     * NON-PUBLIC METHODS
     *-----------------------------------*/

    protected virtual void Dispose(bool disposing) {
        if (disposing) {
            if (IndexBuffer != null) {
                IndexBuffer.Dispose();
                IndexBuffer = null;
            }

            if (VertexBuffer != null) {
                VertexBuffer.Dispose();
                VertexBuffer = null;
            }

            Graphics = null;
        }
    }
}

}
