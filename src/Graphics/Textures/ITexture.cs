namespace PrimusGE.Graphics.Textures {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

/*-------------------------------------
 * INTERFACES
 *-----------------------------------*/

public interface ITexture: IDisposable {
    /*-------------------------------------
     * PROPERTIES
     *-----------------------------------*/

    int Height { get; }

    int Width { get; }
}

}
