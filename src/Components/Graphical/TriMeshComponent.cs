namespace PrimusGE.Components.Graphical {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Graphics;
using Math;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public sealed class TriMeshComponent {
    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public Matrix4x4 Transform { get; set; } = Matrix4x4.Identity();

    public ITriMesh TriMesh { get; set; }
}

}
