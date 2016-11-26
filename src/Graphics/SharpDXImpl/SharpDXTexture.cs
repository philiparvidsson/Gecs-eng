namespace PrimusGE.Graphics.SharpDXImpl {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

using Textures;

using SharpDX.Direct3D;
using SharpDX.Direct3D11;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

internal class SharpDXTexture: IDisposable, ITexture {
    /*-------------------------------------
     * PUBLIC FIELDS
     *-----------------------------------*/

    public SharpDXGraphicsMgr Graphics;


    public Texture2D Texture;

    /*-------------------------------------
     * NON-PUBLIC FIELDS
     *-----------------------------------*/

    private ShaderResourceView m_ShaderResource;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public int Height { get; private set; }

    public bool IsMultisampled {
        get { return Texture.Description.SampleDescription.Count > 1; }
    }

    public ShaderResourceView ShaderResource {
        get {
            if (m_ShaderResource == null) {
                m_ShaderResource = CreateShaderResource();
            }

            return m_ShaderResource;
        }
    }

    public int Width { get; private set; }

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public SharpDXTexture(SharpDXGraphicsMgr graphics,
                          Texture2D       texture,
                          int             width,
                          int             height)
    {
        Graphics = graphics;
        Texture  = texture;
        
        Height = height;
        Width  = width;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /*-------------------------------------
     * NON-PUBLIC
     *-----------------------------------*/

    private ShaderResourceView CreateShaderResource() {
        var description = new ShaderResourceViewDescription {
            Dimension = ShaderResourceViewDimension.Texture2D,
            Format    = Texture.Description.Format,
            Texture2D = new ShaderResourceViewDescription.Texture2DResource {
                MipLevels       = 1,
                MostDetailedMip = 0
            }
        };

        return new ShaderResourceView(Graphics.Device, Texture, description);
    }

    protected virtual void Dispose(bool disposing) {
        if (disposing) {
            if (m_ShaderResource != null) {
                m_ShaderResource.Dispose();
                m_ShaderResource = null;
            }

            if (Texture != null) {
                Texture.Dispose();
                Texture = null;
            }

            Graphics = null;

            Width  = 0;
            Height = 0;
        }
    }
}

}
