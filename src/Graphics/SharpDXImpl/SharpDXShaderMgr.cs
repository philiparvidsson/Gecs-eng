namespace PrimusGE.Graphics.SharpDXImpl {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Collections.Generic;

using Shaders;

using SharpDX.D3DCompiler;
using SharpDX.DXGI;

using D3D11 = SharpDX.Direct3D11;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

internal class SharpDXShaderMgr: IDisposable, IShaderMgr {
#if DEBUG
        private const ShaderFlags DEBUG_FLAG = ShaderFlags.Debug;
#else
        private const ShaderFlags DEBUG_FLAG = ShaderFlags.None;
#endif

        /*-------------------------------------
         * PUBLIC FIELDS
         *-----------------------------------*/

        public SharpDXGraphicsMgr Graphics;

    /*-------------------------------------
     * NON-PUBLIC FIELDS
     *-----------------------------------*/

    private List<SharpDXShader> m_Shaders = new List<SharpDXShader>();

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public SharpDXShaderMgr(SharpDXGraphicsMgr graphics) {
        Graphics = graphics;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public IShader LoadPS<T>(string path) where T: struct {
        return LoadPS(path, typeof (T));
    }

    public IShader LoadPS(string path, Type constantsType=null) {
        using (var byteCode = ShaderBytecode.CompileFromFile(path, "main", "ps_5_0", DEBUG_FLAG)) {
            var gpuShader = new D3D11.PixelShader(Graphics.Device, byteCode);

            var shader = new SharpDXShader(Graphics, gpuShader, null, constantsType);

            m_Shaders.Add(shader);

            return shader;
        }
    }

    public IShader LoadVS<T>(string path) where T: struct {
        return LoadVS(path, typeof (T));
    }

    public IShader LoadVS(string path, Type constantsType=null) {
        using (var byteCode = ShaderBytecode.CompileFromFile(path, "main", "vs_5_0", DEBUG_FLAG)) {
            var gpuShader = new D3D11.VertexShader(Graphics.Device, byteCode);

            var inputElements = new D3D11.InputElement[] {
                new D3D11.InputElement("POSITION", 0, Format.R32G32B32A32_Float,  0, 0),
                new D3D11.InputElement("TEXCOORD", 0, Format.R32G32_Float      , 16, 0)
            };

            var inputSignature = ShaderSignature.GetInputSignature(byteCode);
            var inputLayout = new D3D11.InputLayout(Graphics.Device, inputSignature, inputElements);

            var shader = new SharpDXShader(Graphics, gpuShader, inputLayout, constantsType);

            m_Shaders.Add(shader);

            return shader;
        }
    }

    /*-------------------------------------
     * NON-PUBLIC METHODS
     *-----------------------------------*/

    protected virtual void Dispose(bool disposing) {
        if (disposing) {
            foreach (var shader in m_Shaders) {
                shader.Dispose();
            }

            m_Shaders.Clear();
            m_Shaders = null;

            Graphics = null;
        }
    }
}

}
