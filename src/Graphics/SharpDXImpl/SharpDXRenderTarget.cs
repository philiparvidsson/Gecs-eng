namespace GecsEng.Graphics.SharpDXImpl {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

using Shaders;

using SharpDX.Direct3D11;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

internal class SharpDXRenderTarget: SharpDXTexture, IRenderTarget {
    /*-------------------------------------
     * PUBLIC FIELDS
     *-----------------------------------*/

    public RenderTargetView RenderTarget;

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public SharpDXRenderTarget(SharpDXGraphicsMgr  graphics,
                               Texture2D        texture,
                               int              width,
                               int              height,
                               RenderTargetView renderTarget)
        : base(graphics, texture, width, height)
    {
        RenderTarget = renderTarget;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public void Clear(Color clearColor) {
        var context = Graphics.Device.ImmediateContext;
        var color   = new SharpDX.Color(clearColor.ToIntABGR());

        context.ClearRenderTargetView(RenderTarget, color);
    }

    /*-------------------------------------
     * NON-PUBLIC METHODS
     *-----------------------------------*/

    protected override void Dispose(bool disposing) {
        base.Dispose(true);

        if (disposing) {
            if (RenderTarget != null) {
                RenderTarget.Dispose();
                RenderTarget = null;
            }
        }
    }
}

}
