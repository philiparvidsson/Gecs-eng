namespace PrimusGE.Components.Physical {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

using Math;
using Math.Calc;
using Math.Geom;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public sealed class BodyComponent: IOde {
    /*-------------------------------------
     * NON-PUBLIC CONSTANTS
     *-----------------------------------*/

    private const int N = 6;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public float Angle {
        get { return State[2]; }
        set { State[2] = value; }
    }

    public float AngularVelocity {
        get { return State[5]; }
        set { State[5] = value; }
    }

    public float[] Derivs { get; } = new float[N];

    public Action<float[], float[]> DerivsFn;

    public bool IsStatic { get; set; }

    public Vector2 Position {
        get { return new Vector2(State[0], State[1]); }
        set { State[0] = value.X; State[1] = value.Y;; }
    }

    public float[] PrevState { get; } = new float[N];

    public Shape Shape { get; set; }

    public float[] State { get; } = new float[N];

    public Vector2 Velocity {
        get { return new Vector2(State[3], State[4]); }
        set { State[3] = value.X; State[4] = value.Y; }
    }

    public float Friction    { get; set; } = 0.5f;
    public float LinearDrag  { get; set; } = 0.0f;
    public float InvMoI      { get; set; } = 1.0f / 0.001067f;
    public float InvMass     { get; set; } = 1.0f;
    public float Restitution { get; set; } = 0.7f;
    
    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public BodyComponent() {
        DerivsFn = DefDerivsFn;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public void ApplyImpulse(Vector2 i, Vector2 p) {
        Vector2 a = i * InvMass;
        State[3] += a.X;
        State[4] += a.Y;

        float b = p.PerpDot(i);
        State[5] += b * InvMoI;
    }

    /*-------------------------------------
     * NON-PUBLIC PROPERTIES
     *-----------------------------------*/

    private void DefDerivsFn(float[] state, float[] derivs) {
        // state[0] = x.x
        // state[1] = x.y
        // state[2] = o
        // state[3] = v.x
        // state[4] = v.y
        // state[5] = w

        // derivs[0] = v.x
        // derivs[1] = v.y
        // derivs[2] = w
        // derivs[3] = a.x
        // derivs[4] = a.y
        // derivs[5] = t

        derivs[0] = state[3];
        derivs[1] = state[4];
        derivs[2] = state[5];

        derivs[3] = -derivs[0] * LinearDrag;
        derivs[4] = -derivs[1] * LinearDrag;
        derivs[5] = 0.0f;
    }
}

}
