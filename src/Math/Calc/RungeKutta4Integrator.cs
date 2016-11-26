namespace GecsEng.Math.Calc {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class RungeKutta4Integrator: IIntegrator {
    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public void Solve(IOde eq, float dt, Action<float[], float[]> derivsFn) {
        float[] tmp = new float[6];
        float[] k1  = new float[6];
        float[] k2  = new float[6];
        float[] k3  = new float[6];
        float[] k4  = new float[6];
        
        var derivs = eq.Derivs;
        var state  = eq.State;

        derivsFn(state, k1);

        for (var i = 0; i < 6; i++) {
            tmp[i] = state[i] + k1[i]*0.5f*dt;
        }

        derivsFn(tmp, k2);

        for (var i = 0; i < 6; i++) {
            tmp[i] = state[i] + k2[i]*0.5f*dt;
        }

        derivsFn(tmp, k3);

        for (var i = 0; i < 6; i++) {
            tmp[i] = state[i] + k3[i]*dt;
        }

        derivsFn(tmp, k4);

        for (var i = 0; i < 6; i++) {
            state[i] += (1.0f/6.0f)*(k1[i] + 2.0f*(k2[i] + k3[i]) + k4[i])*dt;
        }
    }
}

}
