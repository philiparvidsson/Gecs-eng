namespace GecsEng.Graphics.Shaders {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Textures;

/*-------------------------------------
 * INTERFACES
 *-----------------------------------*/

public interface IRenderTarget: ITexture {
    /*-------------------------------------
     * METHODS
     *-----------------------------------*/

    void Clear(Color clearColor);
}

}
