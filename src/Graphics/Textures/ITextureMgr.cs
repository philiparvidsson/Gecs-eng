namespace GecsEng.Graphics.Textures {

/*-------------------------------------
 * INTERFACES
 *-----------------------------------*/

public interface ITextureMgr {
    /*-------------------------------------
     * PROPERTIES
     *-----------------------------------*/

    ITexture White { get; }

    /*-------------------------------------
     * METHODS
     *-----------------------------------*/

    ITexture Load(string path);
}

}
