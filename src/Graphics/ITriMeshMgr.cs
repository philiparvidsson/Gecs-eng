namespace GecsEng.Graphics {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Math.Geom;

/*-------------------------------------
 * INTERFACES
 *-----------------------------------*/

public interface ITriMeshMgr {
    /*-------------------------------------
     * METHODS
     *-----------------------------------*/

    ITriMesh CreateQuad(float width, float height);

    ITriMesh FromShape(Shape shape);
}

}
