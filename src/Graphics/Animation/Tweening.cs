namespace PrimusGE.Graphics.Animation {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class Tweening {
    /*-------------------------------------
     * NON-PUBLIC FIELDS
     *-----------------------------------*/

    private float m_X;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public float A { get; }

    public float B { get; }

    public bool IsDone {
        get { return m_X == 1.0f; }
    }
    
    public Func<float, float, float, float> TweeningFunc { get; }

    public float X {
        get { return m_X; }
        set {
            if (value < 0.0f) value = 0.0f;
            if (value > 1.0f) value = 1.0f;

            m_X = value;
        }
    }

    #region Tweening Functions

    public static Func<float, float, float, float> Linear {
        get { return (a, b, x) => (1.0f - x)*a + x*b; }
    }

    #endregion

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public Tweening(Func<float, float, float, float> tweeningFunc, float a = 0.0f, float b = 0.0f) {
        A = a;
        B = b;

        TweeningFunc = tweeningFunc;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public float Calc() {
        return TweeningFunc(A, B, X);
    }
}

}
