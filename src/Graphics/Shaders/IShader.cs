namespace PrimusGE.Graphics.Shaders {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Textures;

/*-------------------------------------
 * INTERFACES
 *-----------------------------------*/

public interface IShader {
    /*-------------------------------------
     * METHODS
     *-----------------------------------*/

    void SetConstants<T>(T constants) where T: struct;

    void SetTextures(params ITexture[] textures);
}

}
