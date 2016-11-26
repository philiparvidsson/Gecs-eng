namespace GecsEng.Graphics.Shaders {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

/*-------------------------------------
 * INTERFACES
 *-----------------------------------*/

public interface IShaderMgr {
    /*-------------------------------------
     * METHODS
     *-----------------------------------*/

    IShader LoadPS<T>(string path) where T: struct;

    IShader LoadPS(string path, Type constantsType=null);

    IShader LoadVS<T>(string path) where T: struct;

    IShader LoadVS(string path, Type constantsType=null);

}

}
