namespace PrimusGE.Graphics.Shaders {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

using Textures;

/*-------------------------------------
 * INTERFACES
 *-----------------------------------*/

public interface IShader: IDisposable {
    /*-------------------------------------
     * METHODS
     *-----------------------------------*/

    void SetConstants<T>(T constants) where T: struct;

    void SetTextures(params ITexture[] textures);
}

}
