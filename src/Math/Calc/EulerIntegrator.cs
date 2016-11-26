namespace GecsEng.Math.Calc {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class EulerIntegrator: IIntegrator {
    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public void Solve(IOde eq, float dt, Action<float[], float[]> derivsFn) {
        var derivs = eq.Derivs;
        var state  = eq.State;

        derivsFn(state, derivs);
        
        var n = state.Length;
        for (var i = 0; i < n; i++) {
            state[i] += derivs[i] * dt;
        }
        
    }
}

}
