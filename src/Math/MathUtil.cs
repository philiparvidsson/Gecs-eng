namespace GecsEng.Math {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Geom;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public static class MathUtil {
    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public static float RectInvMoI(float mass, float width, float height) {
        if (mass == 0.0f) {
            return 0.0f;
        }

        var moi = (mass/12.0f)*(width*width + height*height);

        return 1.0f/moi;
    }

    public static float ShapeInvMoI(float mass, Shape shape) {
        if (mass == 0.0f) {
            return 0.0f;
        }

        var moi = 0.0f;

        foreach (var p in shape.Points) {
            moi += p.X*p.X + p.Y*p.Y;
        }

        return 1.0f/(mass*moi);
    }
}

}
