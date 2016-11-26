namespace GecsEng.Components.Input {
    
/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System.Collections.Generic;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public sealed class ControlsComponent {
    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public Dictionary<string, float> Controls { get; } =
        new Dictionary<string, float>();
}

}
