namespace GecsEng.Math {

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class Rectangle {
    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public float Bottom { get; set; }
    public float Left { get; set; }
    public float Right { get; set; }
    public float Top { get; set; }

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public Rectangle(float top, float right, float bottom, float left) {
        Bottom = bottom;
        Left   = left;
        Right  = right;
        Top    = top;
    }
}

}
