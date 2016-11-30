namespace PrimusGE.Graphics {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System.Windows.Forms;

using Math;
using Shaders;
using Textures;

/*-------------------------------------
 * INTERFACES
 *-----------------------------------*/

public interface IGraphicsMgr {
    /*-------------------------------------
     * PROPERTIES
     *-----------------------------------*/

    IShader DefaultPixelShader { get; }

    IShader DefaultVertexShader { get; }

    bool IsLagging { get; set; }

    string Name { get; }

    IShader PixelShader { get; set; }

    IRenderTarget RenderTarget { get; set; }

    IRenderTarget ScreenRenderTarget { get; }

    IShaderMgr ShaderMgr { get; }

    ITextureMgr TextureMgr { get; }

    ITriMeshMgr TriMeshMgr { get; }

    IShader VertexShader { get; set; }

    /*-------------------------------------
     * METHODS
     *-----------------------------------*/

    void ApplyPostFX(IRenderTarget renderSource, IShader shader);

    void BeginFrame();

    void Cleanup();

    IRenderTarget CreateRenderTarget();

    IRenderTarget[] CreateRenderTargets(int n);

    void DrawTriMesh(ITriMesh triMesh, Matrix4x4 transform);

    void EndFrame();

    void Init(Form window);
}

}
