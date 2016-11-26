namespace PrimusGE.Graphics.SharpDXImpl {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Collections.Generic;

using Math;
using Math.Geom;

using D3D11 = SharpDX.Direct3D11;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

internal class SharpDXTriMeshMgr: IDisposable, ITriMeshMgr {
    /*-------------------------------------
     * NON-PUBLIC FIELDS
     *-----------------------------------*/

    private SharpDXGraphicsMgr m_Graphics;

    private List<SharpDXTriMesh> m_TriMeshes = new List<SharpDXTriMesh>();

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public SharpDXTriMeshMgr(SharpDXGraphicsMgr graphics) {
        m_Graphics = graphics;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public ITriMesh CreateQuad(float width, float height) {
        var hh = 0.5f*height;
        var hw = 0.5f*width;

        var tris = new [] {
            0, 1, 2,
            0, 2, 3
        };

        var verts = new [] {
            new Vertex { Position=new Vector4(-hw, -hh), TexCoord=new Vector2(0.0f, 1.0f) },
            new Vertex { Position=new Vector4(-hw,  hh), TexCoord=new Vector2(0.0f, 0.0f) },
            new Vertex { Position=new Vector4( hw,  hh), TexCoord=new Vector2(1.0f, 0.0f) },
            new Vertex { Position=new Vector4( hw, -hh), TexCoord=new Vector2(1.0f, 1.0f) },
        };

        var iBuf = D3D11.Buffer.Create(m_Graphics.Device,
                                       D3D11.BindFlags.IndexBuffer,
                                       tris);

        var vBuf = D3D11.Buffer.Create(m_Graphics.Device,
                                       D3D11.BindFlags.VertexBuffer,
                                       verts);

        var triMesh = new SharpDXTriMesh(m_Graphics, iBuf, vBuf, tris.Length, verts.Length);

        m_TriMeshes.Add(triMesh);

        return triMesh;
    }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public ITriMesh FromShape(Shape shape) {
        var tris = new int[3*(shape.Points.Length - 1)];
        for (var i = 0; i < shape.Points.Length - 1; i++) {
            tris[3*i  ] = 0;
            tris[3*i+1] = i + 1;
            tris[3*i+2] = i;
        }

        var verts = new Vertex[shape.Points.Length];
        for (var i = 0; i < shape.Points.Length; i++) {
            verts[i] = new Vertex { Position = new Vector4(shape.Points[i]) };
        }

        var iBuf = D3D11.Buffer.Create(m_Graphics.Device,
                                       D3D11.BindFlags.IndexBuffer,
                                       tris);

        var vBuf = D3D11.Buffer.Create(m_Graphics.Device,
                                       D3D11.BindFlags.VertexBuffer,
                                       verts);

        var triMesh = new SharpDXTriMesh(m_Graphics, iBuf, vBuf, tris.Length, verts.Length);

        m_TriMeshes.Add(triMesh);

        return triMesh;
    }

    /*-------------------------------------
     * NON-PUBLIC METHODS
     *-----------------------------------*/

    protected virtual void Dispose(bool disposing) {
        if (disposing) {
            foreach (var triMesh in m_TriMeshes) {
                triMesh.Dispose();
            }

            m_TriMeshes.Clear();
            m_TriMeshes = null;

            m_Graphics = null;
        }
    }
}

}
