namespace GecsEng.Graphics.SharpDXImpl {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Drawing;
using System.Collections.Generic;

using Textures;

using SharpDX.Direct3D11;
using SharpDX.DXGI;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

internal class SharpDXTextureMgr: IDisposable, ITextureMgr {
    /*-------------------------------------
     * NON-PUBLIC FIELDS
     *-----------------------------------*/

    private SharpDXGraphicsMgr m_Graphics;

    private List<SharpDXTexture> m_Textures = new List<SharpDXTexture>();
    
    private SharpDXTexture m_White;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public ITexture White {
        get {
            if (m_White == null) {
                m_White = CreateTexture(1, 1);

                var context = m_Graphics.Device.ImmediateContext;
                var one = ToFloat16(1.0f);
                var data    = new [] { one, one, one, one };

                context.UpdateSubresource(data, m_White.Texture);
            }

            return m_White;
        }
    }

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public SharpDXTextureMgr(SharpDXGraphicsMgr graphics) {
        m_Graphics = graphics;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public SharpDXTexture CreateTexture(int width, int height) {
        var texDesc = new Texture2DDescription {
            ArraySize         = 1,
            BindFlags         = BindFlags.ShaderResource,
            Format            = Format.R16G16B16A16_Float,
            MipLevels         = 1,
            SampleDescription = new SampleDescription(1, 0),
            Width             = width,
            Height            = height
        };

        var device = m_Graphics.Device;
        var gpuTex = new Texture2D(device, texDesc);
        var tex    = new SharpDXTexture(m_Graphics, gpuTex, width, height);

        m_Textures.Add(tex);

        return tex;
    }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public ITexture Load(string path) {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        var context = m_Graphics.Device.ImmediateContext;
        var bitmap  = new Bitmap(path);
        var tex     = CreateTexture(bitmap.Width, bitmap.Height);

        var data = new short[4*bitmap.Width*bitmap.Height];

        /*var bmp = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format64bppArgb);
        System.Runtime.InteropServices.Marshal.Copy(bmp.Scan0, data, 0, data.Length);
        bitmap.UnlockBits(bmp);*/

        int i = 0;
        for (var y = 0; y < bitmap.Height; y++) {
            for (var x = 0; x < bitmap.Width; x++) {
                var c = bitmap.GetPixel(x, y);
                data[i++] = ToFloat16(c.R/255.0f);
                data[i++] = ToFloat16(c.G/255.0f);
                data[i++] = ToFloat16(c.B/255.0f);
                data[i++] = ToFloat16(c.A/255.0f);

            }
        }

        context.UpdateSubresource(data, tex.Texture, 0, 8*bitmap.Width, 1);
        
        return tex;
    }

    short ToFloat16(float value) {
        if (value == 0.0f) {
            return 0;
        }

        var  e = (int)Math.Floor(Math.Log(value));
        uint f = 0;
        uint s = 0;

        if (value < 0.0f) {
            s  = 1;
            value = -value;
        }

        var frac = 1.0f;
        for (var i = 0; i < 10; i++) {
            var x = frac + 1.0f/(float)Math.Pow(2.0f, i + 1);
            if (x*Math.Pow(2.0, e) < value) {
                frac = x;
                f |= (uint)(1 << (9-i));
            }
        }

        e += 15;

        var r = (short)((s<<15) | (((uint)e & 31) << 10) | (f & 1023));
    
        return r;
    }

    /*-------------------------------------
     * NON-PUBLIC METHODS
     *-----------------------------------*/

    protected virtual void Dispose(bool disposing) {
        if (disposing) {
            foreach (var texture in m_Textures) {
                texture.Dispose();
            }

            m_Textures.Clear();
            m_Textures = null;

            m_White    = null;
            m_Graphics = null;
        }
    }
}

}
