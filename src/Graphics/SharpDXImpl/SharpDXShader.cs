namespace GecsEng.Graphics.SharpDXImpl {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Runtime.InteropServices;

using Shaders;
using Textures;

using SharpDX;

using D3D11 = SharpDX.Direct3D11;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

internal class SharpDXShader: IDisposable, IShader {
    /*-------------------------------------
     * PUBLIC FIELDS
     *-----------------------------------*/

    public D3D11.Buffer ConstantBuffer;

    public SharpDXGraphicsMgr Graphics;

    public D3D11.InputLayout InputLayout;

    public D3D11.DeviceChild Shader;


    /*-------------------------------------
     * NON-PUBLIC FIELDS
     *-----------------------------------*/

    private D3D11.Texture2D[] m_NonMultisampledTextures;

    private D3D11.ShaderResourceView[] m_ShaderResources;

    private D3D11.Texture2D[] m_Textures;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public bool HasTextures {
        get { return m_Textures != null; }
    }

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public SharpDXShader(SharpDXGraphicsMgr   graphics,
                         D3D11.DeviceChild shader,
                         D3D11.InputLayout inputLayout,
                         Type              constantsType)
    {
        Graphics    = graphics;
        InputLayout = inputLayout;
        Shader      = shader;

        if (constantsType != null) {
            ConstantBuffer = CreateConstantBuffer(constantsType);
        }
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void SetConstants<T>(T constants) where T: struct {
        const D3D11.MapMode  MODE  = D3D11.MapMode.WriteDiscard;
        const D3D11.MapFlags FLAGS = D3D11.MapFlags.None;

        var context = Graphics.Device.ImmediateContext;

        DataStream dataStream;
        context.MapSubresource(ConstantBuffer, MODE, FLAGS, out dataStream);
        dataStream.Write(constants);
        dataStream.Dispose();
        context.UnmapSubresource(ConstantBuffer, 0);
    }

    public D3D11.ShaderResourceView[] GetShaderResources() {
        var n = m_ShaderResources.Length;

        for (var i = 0; i < n; i++) {
            Graphics.Device.ImmediateContext.ResolveSubresource(m_Textures[i], 0, m_NonMultisampledTextures[i], 0, SharpDX.DXGI.Format.R16G16B16A16_Float);
        }

        return m_ShaderResources;
    }

    public void SetTextures(params ITexture[] textures) {
        if (textures == null || textures.Length == 0) {
            m_ShaderResources         = null;
            m_NonMultisampledTextures = null;
            m_Textures                = null;
        }

        var numTextures = textures.Length;

        m_ShaderResources         = new D3D11.ShaderResourceView[numTextures];
        m_NonMultisampledTextures = new D3D11.Texture2D[numTextures];
        m_Textures                = new D3D11.Texture2D[numTextures];

        for (var i = 0; i < numTextures; i++) {
            var tex = ((SharpDXTexture)textures[i]);

            m_Textures[i] = tex.Texture;

            if (tex.IsMultisampled) {
                tex = ((SharpDXTextureMgr)Graphics.TextureMgr).CreateTexture(tex.Width, tex.Height);
            }

            m_NonMultisampledTextures[i] = tex.Texture;
            m_ShaderResources        [i] = tex.ShaderResource;
        }
    }

    /*-------------------------------------
     * NON-PUBLIC METHODS
     *-----------------------------------*/

    private D3D11.Buffer CreateConstantBuffer(Type type) {
        int size = Marshal.SizeOf(type);

        if ((size % 16) != 0) {
            size += 16 - (size % 16);
        }

        var bufferDescription = new D3D11.BufferDescription() {
            BindFlags      = D3D11.BindFlags.ConstantBuffer,
            CpuAccessFlags = D3D11.CpuAccessFlags.Write,
            SizeInBytes    = size,
            Usage          = D3D11.ResourceUsage.Dynamic,
        };

        var data   = new byte[size];
        var buffer = D3D11.Buffer.Create(Graphics.Device,
                                         ref data[0],
                                         bufferDescription);

        return buffer;
    }

    protected virtual void Dispose(bool disposing) {
        if (disposing) {
            Graphics = null;

            if (ConstantBuffer != null) {
                ConstantBuffer.Dispose();
                ConstantBuffer = null;
            }

            if (InputLayout != null) {
                InputLayout.Dispose();
                InputLayout = null;
            }

            if (Shader != null) {
                Shader.Dispose();
                Shader = null;
            }
        }
    }
}

}
